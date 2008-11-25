using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace WPFExtensions.ViewModel.Commanding
{
	public class MapperCommandBinding : CommandBinding
	{

		private ICommand _mappedCommand = null;

		private bool mapped = false;

		/// <summary>
		/// The command which will executed instead of the 'Command'.
		/// </summary>
		public ICommand MappedCommand
		{
			get { return _mappedCommand; }
			set
			{
				//mapped command cannot be null
				if ( value == null )
					throw new ArgumentException( "value" );

				if ( mapped )
				{
					this.CanExecute -= OnCanExecute;
					this.Executed -= OnExecuted;
				}

				this._mappedCommand = value;
				mapped = true;

				this.CanExecute += OnCanExecute;
				this.Executed += OnExecuted;
			}
		}


		//
		//Mapper event handlers
		//
		protected void OnCanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			if ( MappedCommand is RoutedCommand && e.Source is IInputElement )
				e.CanExecute = ( MappedCommand as RoutedCommand ).CanExecute( e.Parameter, e.Source as IInputElement );
			else
				e.CanExecute = MappedCommand.CanExecute( e.Parameter );
			e.Handled = true;
			e.ContinueRouting = false;
		}

		protected void OnExecuted( object sender, ExecutedRoutedEventArgs e )
		{
			if ( MappedCommand is RoutedCommand && e.Source is IInputElement )
				( MappedCommand as RoutedCommand ).Execute( e.Parameter, e.Source as IInputElement );
			else
				MappedCommand.Execute( e.Parameter );
			e.Handled = true;
		}
	}
}
