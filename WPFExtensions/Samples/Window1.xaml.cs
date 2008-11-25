using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Samples
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			InitializeComponent();
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
	}
}
