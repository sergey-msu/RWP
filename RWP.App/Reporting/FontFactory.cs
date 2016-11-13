using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using RWP.App.Infrastructure;

namespace RWP.App.Reporting
{
  public static class FontFactory
  {
    public static readonly string TTF_DIR = Path.GetDirectoryName(RwpRoot.EntryPoint) + @"\Resources\Fonts\";

    public static Font CreateFont(string family, int size, int style)
    {
      var ttf = GetTTF(family, style);
      var baseFont = BaseFont.CreateFont(TTF_DIR + ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
      return new Font(baseFont, size, style);
    }

    private static string GetTTF(string family, int style)
    {
      switch (family)
      {
        case (Constants.FONT_TIMES):
          return ForTimes(style);
        case (Constants.FONT_ARIAL):
          return ForArial(style);
        default:
          return "TIMCYR.TTF";
      }
    }

    private static string ForTimes(int style)
    {
      switch (style)
      {
        case (Font.NORMAL): return "TIMCYR.TTF";
        case (Font.BOLD): return "TIMCYRB.TTF";
        case (Font.BOLDITALIC): return "TIMCYRBI.TTF";
        case (Font.ITALIC): return "TIMCYRI.TTF";
        default: return "TIMCYR.TTF";
      }
    }

    private static string ForArial(int style)
    {
      switch (style)
      {
        case (Font.NORMAL): return "ARIAL.TTF";
        case (Font.BOLD): return "ARIALBD.TTF";
        case (Font.BOLDITALIC): return "ARIALBI.TTF";
        case (Font.ITALIC): return "ARIALI.TTF";
        default: return "ARIAL.TTF";
      }
    }
  }
}
