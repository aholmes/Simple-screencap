using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace ScreenShot.Utilities
{
  public static class Extensions 
  {
    public static string GetImageFormatFileExtension(this ImageFormat format) 
    {
      return "." + format.ToString().ToLower();
    }

    /// <summary>
    /// Create FileDialog filter string
    /// </summary>
    /// <param name="format">ImageCode format</param>
    /// <returns> Example: JPEG files (*.bmp, *.jpg)|*.bmp;*.jpg</returns>
    public static string GetFilterFileExtensions(this ImageFormat format) 
    {
      string result = null;
      var enc = ImageCodecInfo.GetImageEncoders().FirstOrDefault(info => info.FormatID == format.Guid);
      if (enc == null) return null;

      var exts = enc.FilenameExtension.Replace(';', ',');
      if (!string.IsNullOrWhiteSpace(exts))
        result = string.Format("{0} files ({1})|{2}", enc.FormatDescription, exts.ToLower(), enc.FilenameExtension);
      return result;
    }
  }
}