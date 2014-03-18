using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Win32;
using ScreenShot.Utilities;

namespace ScreenShot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _filePath;
        private ImageFormat _format = ImageFormat.Png;
        private CaptureMode _mode = CaptureMode.Screen;

        public List<ImageFormat> ImageFormats
        {
            get
            {
                return new List<ImageFormat>
                {
                    ImageFormat.Bmp,
                    ImageFormat.Gif,
                    ImageFormat.Jpeg,
                    ImageFormat.Png,
                    ImageFormat.Tiff,
	            };
            }
        }

        public List<CaptureMode> CaptureModes
        {
            get
            {
                return new List<CaptureMode>
                {
                    //CaptureMode.Window, BUG: this doesn't really work for GUI, unless you want to screenshot this app
                    CaptureMode.Desktop,
                    CaptureMode.Screen,
	            };
            }
        }

        public CaptureMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                OnPropertyChanged();
            }
        }

        public ImageFormat Format
        {
            get { return _format; }
            set
            {
                _format = value;
                OnPropertyChanged();
                UpdateFileExtension();
            }
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                if (string.IsNullOrWhiteSpace(Path.GetExtension(_filePath)))
                    _filePath = Path.ChangeExtension(_filePath, Format.GetImageFormatFileExtension());
                else if (Path.GetExtension(_filePath) != Format.GetImageFormatFileExtension())
                {
                    UpdateFormat();
                }
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Capture-%NOW%");
        }

        private void CaptureScreenshot(object sender, RoutedEventArgs e)
        {
            ScreenCapturer.CaptureAndSave(FilePath, Mode, Format);
        }

        private void ChangeSavePath(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                DefaultExt = Format.GetImageFormatFileExtension(),
                CheckPathExists = true,
                AddExtension = true,
                FileName = Path.GetFileName(FilePath),
                Filter = Format.GetFilterFileExtensions() + "| All Files |*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Title = "Screenshot save path"
            };

            if (dlg.ShowDialog(this).Value)
            {
                FilePath = dlg.FileName;
            }
        }

        private void UpdateFormat()
        {
            /* Compare the extensions between the desired FilePath and the selected Format */
            var ext = Path.GetExtension(FilePath);
            var formatExt = Format.GetImageFormatFileExtension();
            if (ext == formatExt) return;

            /* They do not match - find the right format for the extension and update the Format property */
            var enc = ImageCodecInfo.GetImageEncoders().FirstOrDefault(info =>
            {
                var possibleExtensions = info.FilenameExtension.ToLower().Replace("*", "").Split(';');
                return ext != null && possibleExtensions.Contains(ext.ToLower());
            });

            if (enc != null)
                Format = ImageFormats.First(format => format.Guid == enc.FormatID);
        }

        private void UpdateFileExtension()
        {
            /* Ensure the FilePath extension reflects the Format selected */
            if (string.IsNullOrWhiteSpace(FilePath))
                return;

            var oldExt = Path.GetExtension(FilePath);
            var newExt = Format.GetImageFormatFileExtension();
            if (oldExt != newExt)
            {
                FilePath = Path.ChangeExtension(FilePath, newExt);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
