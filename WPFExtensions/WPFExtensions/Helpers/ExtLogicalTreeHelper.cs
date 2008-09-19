using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFExtensions.Helpers
{
	public static class ExtLogicalTreeHelper
	{
		public static T GetAncestorOfTypeExt<T>( this DependencyObject obj )
			where T : DependencyObject
		{
			T result = null;
			DependencyObject parent = obj;
			DependencyObject newParent = obj;
			while ( result == null && ( ( newParent = LogicalTreeHelper.GetParent( parent ) ) != null ||
				( parent is FrameworkElement && ( newParent = ( parent as FrameworkElement ).TemplatedParent ) != null ) ||
				( parent is FrameworkContentElement && ( newParent = ( parent as FrameworkContentElement ).TemplatedParent ) != null ) ) )
			{
				result = newParent as T;
				parent = newParent;
			}

			return result;
		}

		public static T GetAncestorOfType<T>( this DependencyObject obj )
			where T : DependencyObject
		{
			//extended GetParent(), recursively goes up in the LogicalTree
			T result = null;
			DependencyObject parent = obj;
			while ( result == null && ( ( parent = LogicalTreeHelper.GetParent( parent ) ) != null ) )
				result = parent as T;

			return result;
		}

		public static T GetAncestorOfType<T>( this DependencyObject obj, bool followTemplatedParent )
			where T : DependencyObject
		{
			return followTemplatedParent ? obj.GetAncestorOfTypeExt<T>() : obj.GetAncestorOfType<T>();
		}
	}
}
