using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace PacUPrintHelper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private FolderBrowserDialog cInDirDialog = new FolderBrowserDialog();
		private SaveFileDialog cOutFileDialog = new SaveFileDialog();
		private ViewModel cCtx;

		private void RunBtn_Click(object sender, RoutedEventArgs e)
		{
			cCtx.DoMagic();
			return;
		}

		public MainWindow()
		{
			cCtx = new ViewModel();
			InitializeComponent();
			this.DataContext = cCtx;
		}

		private void InDirBtn_Click(object sender, RoutedEventArgs e)
		{
			var result = cInDirDialog.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
			{
				cCtx.cInFolder = cInDirDialog.SelectedPath;
				cCtx.ListFilesInDialog();
			}
		}

		private void OutDirBtn_Click(object sender, RoutedEventArgs e)
		{
			var result = cOutFileDialog.ShowDialog();
			if (result.GetValueOrDefault(false))
			{
				cCtx.cOutFile = cOutFileDialog.FileName;
			}
		}
	}
}
