using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using RWP.Data.Contracts;

namespace RWP.App.Infrastructure
{
  /// <summary>
  /// Various utilities
  /// </summary>
  public static class Utils
  {
    public const string COMMA_SEPARATOR = ", ";
    public const string PLUS_SEPARATOR = " + ";

    private const string DEFAULT_DISPLAY_STR = "-";
    private const string FULL_NAME_FORMAT = "{0} {1} {2}";
    private const string INIT_NAME_FORMAT = "{0}.";

    /// <summary>
    /// Fetch string resource by name
    /// </summary>
    public static string GetResource(string key)
    {
      return (string)Application.Current.FindResource(key);
    }

    /// <summary>
    /// Fetch typed resource by name
    /// </summary>
    public static T GetResource<T>(string key)
    {
      return (T)Application.Current.FindResource(key);
    }

    /// <summary>
    /// Re-fetch item from collection by ID
    /// </summary>
    public static T Rejuvenate<T>(IList<T> collection, T old)
      where T : class, IEntityWithId
    {
      if (collection == null || old == null)
        return null;

      return collection.FirstOrDefault(e => e.Id == old.Id);
    }

    /// <summary>
    /// Re-fill 'old' collecton with 'new' items by IDs
    /// </summary>
    public static void Rejuvenate<T>(IList<T> collection, IList<T> old)
      where T : class, IEntityWithId
    {
      if (collection == null || old == null)
        return;

      var oldItems = old.ToList();
      old.Clear();
      foreach (var newItem in collection)
      {
        var item = oldItems.FirstOrDefault(o => o.Id == newItem.Id);
        if (item != null)
          old.Add(newItem);
      }
    }

    /// <summary>
    /// Exchange input string with default value if empty
    /// </summary>
    public static string ToDisplay(this string src, string dft = DEFAULT_DISPLAY_STR)
    {
      return string.IsNullOrWhiteSpace(src) ? dft : src;
    }

    /// <summary>
    /// String normalization: accepts only letters, digits and some 'good' specs, replace other by normalizator character
    /// </summary>
    public static string Normalize(string source, char normalizatior = '_')
    {
      if (source == null)
        return null;

      var builder = new StringBuilder();
      foreach (var c in source)
      {
        if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ||
            (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') ||
            (c >= '0' && c <= '9') ||
            c == '_' || c == ' ' || c == '.' || c == ',')
          builder.Append(c);
        else
          builder.Append(normalizatior);
      }

      return builder.ToString();
    }

    /// <summary>
    /// Returns full name by 1st, 2nd and midle name (full or only initials)
    /// </summary>
    public static string ToFullName(string firstName, string middleName, string lastName, bool useInitials = false)
    {
      if (useInitials)
      {
        firstName = !string.IsNullOrWhiteSpace(firstName) ?
                      string.Format(INIT_NAME_FORMAT, firstName[0]) :
                      string.Empty;
        middleName = !string.IsNullOrWhiteSpace(middleName) ?
                      string.Format(INIT_NAME_FORMAT, middleName[0]) :
                      string.Empty;
      }

      lastName = lastName ?? string.Empty;
      var fullName = string.Format(FULL_NAME_FORMAT, lastName, firstName, middleName);
      fullName = Regex.Replace(fullName, @"\s+", " ");
      fullName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fullName);

      return fullName;
    }

    /// <summary>
    /// Truncates string with ellipsis
    /// </summary>
    public static string TruncateString(string str, string ellipsis, int length)
    {
      if (string.IsNullOrEmpty(str) || length <= 0 || str.Length < ellipsis.Length)
        return string.Empty;

      if (str.Length <= length)
        return str;

      var trunc = str.Substring(0, length - ellipsis.Length) + ellipsis;

      return trunc;
    }

    /// <summary>
    /// Decodes image from bytes to bitmap
    /// </summary>
    public static BitmapImage DecodeImage(byte[] imageBytes)
    {
      if (imageBytes == null || imageBytes.Length == 0) return null;
      var image = new BitmapImage();
      using (var ms = new MemoryStream(imageBytes))
      {
        ms.Position = 0;
        image.BeginInit();
        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.UriSource = null;
        image.StreamSource = ms;
        image.EndInit();
      }
      image.Freeze();

      return image;
    }

    /// <summary>
    /// Encodes image from bitmap into bytes
    /// </summary>
    public static byte[] EncodeImage(BitmapImage source)
    {
      var encoder = new PngBitmapEncoder();
      encoder.Frames.Add(BitmapFrame.Create(source));
      using (MemoryStream ms = new MemoryStream())
      {
        encoder.Save(ms);
        return ms.ToArray();
      }
    }

    /// <summary>
    /// 'less than' DateTime extension
    /// </summary>
    public static bool IsLessThan(this DateTime source, DateTime date)
    {
      return source.Year < date.Year ||
             (source.Year == date.Year && source.Month < date.Month) ||
             (source.Year == date.Year && source.Month == date.Month && source.Day < date.Day);
    }

    /// <summary>
    /// 'less than or equals' DateTime extension
    /// </summary>
    public static bool IsLessThanOrEquals(this DateTime source, DateTime date)
    {
      return source.Year < date.Year ||
             (source.Year == date.Year && source.Month < date.Month) ||
             (source.Year == date.Year && source.Month == date.Month && source.Day <= date.Day);
    }
  }
}
