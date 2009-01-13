using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace WPFExtensions.AttachedBehaviours
{
	public class AnimatedScrollBehaviour : FrameworkElementAttachedBehaviourBase<ScrollViewer>
	{
		#region Attached Dependency Property Registration

		public static bool GetEnabled( DependencyObject obj )
		{
			return (bool)obj.GetValue( EnabledProperty );
		}

		public static void SetEnabled( DependencyObject obj, bool value )
		{
			obj.SetValue( EnabledProperty, value );
		}

		// Using a DependencyProperty as the backing store for Enabledd.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EnabledProperty =
			DependencyProperty.RegisterAttached( "Enabled", typeof( bool ), typeof( AnimatedScrollBehaviour ), new UIPropertyMetadata( false, Enabled_PropertyChanged ) );

		private static void Enabled_PropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var scrollViewer = d as ScrollViewer;
			if ( scrollViewer == null )
				return;

			if ( (bool)e.NewValue )
				Attach<AnimatedScrollBehaviour>( scrollViewer );
			else
				Detach<AnimatedScrollBehaviour>( scrollViewer );
		}


		public static Duration GetDuration( DependencyObject obj )
		{
			return (Duration)obj.GetValue( DurationProperty );
		}

		public static void SetDuration( DependencyObject obj, Duration value )
		{
			obj.SetValue( DurationProperty, value );
		}

		// Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DurationProperty =
			DependencyProperty.RegisterAttached( "Duration", typeof( Duration ), typeof( AnimatedScrollBehaviour ), new UIPropertyMetadata( new Duration( new TimeSpan( 0, 0, 0, 0, 500 ) ) ) );




		public static double GetHorizontalOffset( DependencyObject obj )
		{
			return (double)obj.GetValue( HorizontalOffsetProperty );
		}

		public static void SetHorizontalOffset( DependencyObject obj, double value )
		{
			obj.SetValue( HorizontalOffsetProperty, value );
		}

		// Using a DependencyProperty as the backing store for HorizontalOffset.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HorizontalOffsetProperty =
			DependencyProperty.RegisterAttached( "HorizontalOffset", typeof( double ), typeof( AnimatedScrollBehaviour ), new UIPropertyMetadata( 0.0, HorizontalOffset_PropertyChanged ) );

		private static void HorizontalOffset_PropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var scrollViewer = d as ScrollViewer;
			if ( scrollViewer == null )
				return;

			var asb = Get<AnimatedScrollBehaviour>( scrollViewer, false );
			if ( asb == null )
				return;

			Debug.WriteLine( "Horizontal offset:" + e.NewValue );
			asb.lastHorizontalOffset = (double)e.NewValue;
			asb.Element.ScrollToHorizontalOffset( asb.lastHorizontalOffset );
		}


		public static double GetVerticalOffset( DependencyObject obj )
		{
			return (double)obj.GetValue( VerticalOffsetProperty );
		}

		public static void SetVerticalOffset( DependencyObject obj, double value )
		{
			obj.SetValue( VerticalOffsetProperty, value );
		}

		// Using a DependencyProperty as the backing store for VerticalOffset.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty VerticalOffsetProperty =
			DependencyProperty.RegisterAttached( "VerticalOffset", typeof( double ), typeof( AnimatedScrollBehaviour ), new UIPropertyMetadata( 0.0, VerticalOffset_PropertyChanged ) );

		private static void VerticalOffset_PropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var scrollViewer = d as ScrollViewer;
			if ( scrollViewer == null )
				return;

			var asb = Get<AnimatedScrollBehaviour>( scrollViewer, false );
			if ( asb == null )
				return;

			Debug.WriteLine( "Vertical offset:" + e.NewValue );
			asb.lastVerticalOffset = (double)e.NewValue;
			asb.Element.ScrollToVerticalOffset( asb.lastVerticalOffset );
		}



		public static double GetAccelerationRatio( DependencyObject obj )
		{
			return (double)obj.GetValue( AccelerationRatioProperty );
		}

		public static void SetAccelerationRatio( DependencyObject obj, double value )
		{
			obj.SetValue( AccelerationRatioProperty, value );
		}

		// Using a DependencyProperty as the backing store for AccelerationRatio.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AccelerationRatioProperty =
			DependencyProperty.RegisterAttached( "AccelerationRatio", typeof( double ), typeof( AnimatedScrollBehaviour ), new UIPropertyMetadata( 0.0 ) );



		public static double GetDecelerationRatio( DependencyObject obj )
		{
			return (double)obj.GetValue( DecelerationRatioProperty );
		}

		public static void SetDecelerationRatio( DependencyObject obj, double value )
		{
			obj.SetValue( DecelerationRatioProperty, value );
		}

		// Using a DependencyProperty as the backing store for DecelerationRatio.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DecelerationRatioProperty =
			DependencyProperty.RegisterAttached( "DecelerationRatio", typeof( double ), typeof( AnimatedScrollBehaviour ), new UIPropertyMetadata( 0.0 ) );



		#endregion

		private bool verticalAnimationEnded = true, horizontalAnimationEnded = true;
		private DoubleAnimation verticalAnimation, horizontalAnimation;
		private double lastVerticalOffset, lastHorizontalOffset;
		private bool mouseWheelActivated;
		private DateTime mouseWheelActivationTime;
		private static readonly TimeSpan mouseWheelTrigger = new TimeSpan( 0, 0, 0, 0, 10 );

		public AnimatedScrollBehaviour( ScrollViewer element )
			: base( element )
		{ }

		protected override void OnAttach()
		{
			Element.ScrollChanged += ScrollChanged;
			Element.PreviewMouseWheel += MouseWheelActivated;
		}

		private void MouseWheelActivated( object sender, MouseWheelEventArgs e )
		{
			mouseWheelActivated = true;
			mouseWheelActivationTime = DateTime.Now;
		}

		protected override void OnDetach()
		{
			Element.ScrollChanged -= ScrollChanged;
		}

		private void ScrollChanged( object sender, ScrollChangedEventArgs e )
		{
			var enableAnimation = !Element.IsKeyboardFocusWithin && !Element.IsMouseCaptureWithin && !mouseWheelActivated && ( DateTime.Now - mouseWheelActivationTime > mouseWheelTrigger );
			if ( verticalAnimationEnded && enableAnimation && Math.Abs( e.VerticalOffset - lastVerticalOffset ) > double.Epsilon )
			{
				Debug.WriteLine( "Start vertical animation: o {" + e.VerticalOffset + "} c {" + e.VerticalChange + "}" );
				verticalAnimation = new DoubleAnimation( e.VerticalOffset - e.VerticalChange, e.VerticalOffset,
														GetDuration( Element ), FillBehavior.HoldEnd )
										{
											AccelerationRatio = GetAccelerationRatio( Element ),
											DecelerationRatio = GetDecelerationRatio( Element )
										};
				verticalAnimation.Completed += OnVerticalAnimationEnded;
				verticalAnimationEnded = false;
				Element.BeginAnimation( AnimatedScrollBehaviour.VerticalOffsetProperty, verticalAnimation );
			}

			if ( horizontalAnimationEnded && enableAnimation && Math.Abs( e.HorizontalOffset - lastHorizontalOffset ) > double.Epsilon )
			{
				Debug.WriteLine( "Start horizontal animation: o {" + e.HorizontalOffset + "} c {" + e.HorizontalChange + "}" );
				horizontalAnimation = new DoubleAnimation( e.HorizontalOffset - e.HorizontalChange, e.HorizontalOffset,
														  GetDuration( Element ), FillBehavior.HoldEnd )
										{
											AccelerationRatio = GetAccelerationRatio( Element ),
											DecelerationRatio = GetDecelerationRatio( Element )
										};
				horizontalAnimation.Completed += OnHorizontalAnimationEnded;
				horizontalAnimationEnded = false;
				Element.BeginAnimation( AnimatedScrollBehaviour.HorizontalOffsetProperty, horizontalAnimation );
			}
			mouseWheelActivated = false;
		}

		private void OnHorizontalAnimationEnded( object sender, EventArgs e )
		{
			horizontalAnimationEnded = true;
			var last = lastHorizontalOffset;
			Element.BeginAnimation( AnimatedScrollBehaviour.HorizontalOffsetProperty, null );
			SetHorizontalOffset( Element, last );
		}

		private void OnVerticalAnimationEnded( object sender, EventArgs e )
		{
			verticalAnimationEnded = true;
			var last = lastVerticalOffset;
			Element.BeginAnimation( AnimatedScrollBehaviour.VerticalOffsetProperty, null );
			SetVerticalOffset( Element, last );
		}
	}
}
