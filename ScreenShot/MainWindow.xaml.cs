using System;
using System.Drawing.Imaging;
using System.Windows;

namespace ScreenShot
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void CaptureScreenshot(object sender, RoutedEventArgs e)
		{
			var filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			var filename = "Capture-%NOW%.png";

			ScreenCapturer.CaptureAndSave(filepath + "\\" + filename , CaptureMode.Screen, ImageFormat.Png);
		}
	}
}
