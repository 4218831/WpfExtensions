using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFExtensions.Controls
{
    public class ZoomContentPresenter : ContentPresenter
    {
        private Size _contentSize;

        public Size ContentSize
        {
            get { return _contentSize; }
            private set { _contentSize = value; }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var max = 1000000000;
            var x = double.IsInfinity(constraint.Width) ? max : constraint.Width;
            var y = double.IsInfinity(constraint.Height) ? max : constraint.Height;
            return new Size(x, y);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            UIElement child = VisualChildrenCount > 0
                                  ? VisualTreeHelper.GetChild(this, 0) as UIElement
                                  : null;
            if (child == null)
                return arrangeBounds;

            //set the ContentSize
            ContentSize = child.DesiredSize;
            child.Arrange(new Rect(child.DesiredSize));

            ////set the RenderTransformOrigin
            //var x = arrangeBounds.Width <= 0 ? 0 : ContentSize.Width / arrangeBounds.Width / 2.0;
            //var y = arrangeBounds.Height <= 0 ? 0 : ContentSize.Height / arrangeBounds.Height / 2.0;
            //RenderTransformOrigin = new Point(x, y);

            return arrangeBounds;
        }
    }
}
