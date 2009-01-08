using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace WPFExtensions.Helpers
{
	public static class TreeViewHelper
	{
		public static IEnumerable<TreeViewItem> FindTreeViewItemRoute( this TreeView treeView, object item )
		{
			if ( item == null )
				return new TreeViewItem[0];

			//find the TreeViewItem with DFS
			IList<TreeViewItem> route = new List<TreeViewItem>();

			var selectedTreeViewItem = treeView.FindTreeViewItemFor( item );
			if ( selectedTreeViewItem != null )
			{
				for ( FrameworkElement element = selectedTreeViewItem; element != treeView && item != null; item = element.Parent as FrameworkElement )
					route.Insert( 0, element as TreeViewItem );
			}

			return route;
		}

		public static IEnumerable<object> FindItemRoute( this TreeView treeView, object item )
		{
			//find the TreeViewItem with DFS
			IList<object> route = new List<object>();

			if ( item == null )
				return route;

			var selectedTreeViewItem = treeView.FindTreeViewItemFor( item );
			if ( selectedTreeViewItem != null )
			{
				for ( ItemsControl element = selectedTreeViewItem;
					element != treeView && element != null && element.Parent != null;
					element = element.Parent as ItemsControl )
				{
					var parent = element.Parent as ItemsControl;
					if ( parent == null )
						break;
					route.Insert( 0, ( parent.ItemContainerGenerator.ItemFromContainer( element ) ) );
				}
			}

			return route;
		}

		public static TreeViewItem FindTreeViewItemFor( this TreeView treeView, object selectedItem )
		{
			return treeView.FindTreeViewItemInItemsControlHierarhically( selectedItem );
		}

		public static TreeViewItem FindTreeViewItemFor( this TreeViewItem treeViewItem, object selectedItem )
		{
			return treeViewItem.FindTreeViewItemInItemsControlHierarhically( selectedItem );
		}

		private static TreeViewItem FindTreeViewItemInItemsControlHierarhically( this ItemsControl itemsControl, object selectedItem )
		{
			TreeViewItem treeViewItem = itemsControl.ItemContainerGenerator.ContainerFromItem( selectedItem ) as TreeViewItem;
			if ( treeViewItem != null )
				return treeViewItem;

			if ( itemsControl.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated )
				return null;

			foreach ( var item in itemsControl.Items )
			{
				treeViewItem = itemsControl.ItemContainerGenerator.ContainerFromItem( item ) as TreeViewItem;
				if ( ( treeViewItem = treeViewItem.FindTreeViewItemInItemsControlHierarhically( selectedItem ) ) != null )
					//the selected TreeViewItem has been found
					return treeViewItem;
			}
			return null;
		}
	}
}
