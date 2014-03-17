using System;
using System.Drawing.Imaging;

namespace ScreenShot
{
	// http://stackoverflow.com/a/6156665/1801382
	public class EntryPoint
	{
		[STAThread]
		public static void Main(string[] args)
		{
			if (args != null && args.Length > 0)
			{
				foreach (var arg in args)
				{
					switch (arg)
					{
						case "-noWindow":
							CaptureNow();
						return;
					}
				}
			}

			StartMainWindow();
		}

		public static void StartMainWindow()
		{
			var app = new App();
			var mainWindow = new MainWindow();
			app.Run(mainWindow);
		}

		public static void CaptureNow()
		{
			var filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			var filename = "Capture-%NOW%.png";

			ScreenCapturer.CaptureAndSave(filepath + "\\" + filename, CaptureMode.Screen, ImageFormat.Png);
		}

	}
}
