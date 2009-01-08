using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace Samples
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1
	{
		public Window1()
		{
			InitializeComponent();

			DataContext = new TreeViewDataSource();
		}

		public void ExecuteFirst( object sender, ExecutedRoutedEventArgs e )
		{
			MessageBox.Show( "Executing 1st command!" );
		}

		public void ExecuteSecond( object sender, ExecutedRoutedEventArgs e )
		{
			MessageBox.Show( "Executing 2nd command!" );
		}

		public void ExecuteThird( object sender, ExecutedRoutedEventArgs e )
		{
			MessageBox.Show( "Executing 3rd command!" );
		}

		private void TextBoxCommand_Execute( object sender, ExecutedRoutedEventArgs e )
		{
			MessageBox.Show( "TextBox Command Executed with parameter: " + e.Parameter );
		}

		private void btnExpandAll_Click(object sender, RoutedEventArgs e)
		{
			var ds = DataContext as TreeViewDataSource;
			ds.ExpandAll();
		}

		private void btnSelectOne_Click(object sender, RoutedEventArgs e)
		{
			var ds = DataContext as TreeViewDataSource;
			ds.Items[2].IsExpanded = true;
			ds.Items[2].Children[2].IsExpanded = true;
			ds.Items[2].Children[2].Children[2].IsExpanded = true;
			ds.Items[2].Children[2].Children[2].Children[2].IsSelected = true;
		}
	}
}
