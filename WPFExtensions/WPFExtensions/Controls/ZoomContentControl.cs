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
            return constraint;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            UIElement child = VisualChildrenCount > 0
                                  ? VisualTreeHelper.GetChild(this, 0) as UIElement
                                  : null;
            if (child == null)
                return arrangeBounds;

            ContentSize = child.DesiredSize;
            child.Arrange(new Rect(child.DesiredSize));
            return arrangeBounds;
        }
    }
}
