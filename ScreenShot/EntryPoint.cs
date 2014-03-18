using System;
using System.Drawing.Imaging;
using System.IO;

namespace ScreenShot
{
    // http://stackoverflow.com/a/6156665/1801382
    public class EntryPoint
    {
        private const string Filename = "Capture-%NOW%";

        private static ImageFormat _format = ImageFormat.Png;
        private static CaptureMode _mode = CaptureMode.Screen;
        private static bool _noWindow;

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
                        _noWindow = true;
                        break;
                        case "-jpg":
                        case "-jpeg":
                        _format = ImageFormat.Jpeg;
                        break;
                        case "-bmp":
                        _format = ImageFormat.Bmp;
                        break;
                        case "-png":
                        _format = ImageFormat.Png;
                        break;
                        case "-screen":
                        _mode = CaptureMode.Screen;
                        break;
                        case "-window":
                        _mode = CaptureMode.Window;
                        break;
                        case "-desktop":
                        _mode = CaptureMode.Desktop;
                        break;
                    }
                }
                if (_noWindow)
                {
                    CaptureNow();
                    return;
                }
            }

            StartMainWindow();
        }

        public static void StartMainWindow()
        {
            var app = new App();
            app.InitializeComponent();
            var mainWindow = new MainWindow();
            app.Run(mainWindow);
        }

        public static void CaptureNow(string basePath = null)
        {
            string filepath;
            if (basePath == null || !Directory.Exists(basePath))
                filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            else filepath = basePath;

            ScreenCapturer.CaptureAndSave(filepath + "\\" + Filename, _mode, _format);
        }
    }
}