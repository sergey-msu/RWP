using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using RWP.App.Models;
using RWP.App.Infrastructure;
using System.Collections.Generic;
using System;

namespace RWP.App.Reporting
{
  public abstract class PDFReportBase<TContext> : ReportBase<TContext>
  {
    public static readonly string DATE_FORMAT = Utils.GetResource("DateFormatString");

    private static Dictionary<Tuple<string, int, int>, Font> _fontCache = new Dictionary<Tuple<string, int, int>, Font>();

    private int _lineSpacing;
    private int _medSpacing;
    private int _bigSpacing;
    private int _largeSpacing;
    private string _family;
    private int _size;


    protected PDFReportBase(TContext context, ReportSettingsModel settings) : base(context, settings)
    {
    }

    public int LineSpacing { get { return _lineSpacing; } }
    public int MedSpacing { get { return _medSpacing; } }
    public int BigSpacing { get { return _bigSpacing; } }
    public int LargeSpacing { get { return _largeSpacing; } }


    protected Font GetFont(string family = null, int? size = null, int? style = null)
    {
      family = family ?? _family;
      size = size ?? _size;
      style = style ?? Font.NORMAL;

      Font font;
      var key = Tuple.Create(family, size.Value, style.Value);
      if (_fontCache.TryGetValue(key, out font))
        return font;

      var cache = new Dictionary<Tuple<string, int, int>, Font>(_fontCache);
      font = FontFactory.CreateFont(family, size.Value, style.Value);
      cache[key] = font;
      _fontCache = cache;

      return font;
    }

    protected override void CreateImpl(Stream output)
    {
      using (var doc = new Document(PageSize.A4, 0, 0, 0, 0))
      using (var writer = PdfWriter.GetInstance(doc, output))
      {
        doc.Open();
        Init(doc);
        CreateImpl(doc);
        doc.Close();
      }
    }

    protected abstract void CreateImpl(Document doc);


    protected virtual void Init(Document doc)
    {
      _family = Settings.GetSetting<string>(Constants.FONT_FAMILY_SETTING);
      _size = Settings.GetSetting<int>(Constants.FONT_SIZE_SETTING);
      _lineSpacing = Settings.GetSetting<int>(Constants.LINE_MARGIN_SETTING);
      _medSpacing = _lineSpacing + 1;
      _bigSpacing = _lineSpacing + 3;
      _largeSpacing = _lineSpacing * 2 + 12;

      var docMargin = Settings.GetSetting<int>(Constants.DOC_MARGIN_SETTING);
      doc.SetMargins(docMargin, docMargin, docMargin, docMargin);
      doc.NewPage();
    }

    protected virtual void CreateParagraph(Document doc,
                                           string content,
                                           int alignment,
                                           float? spaceBefore = null,
                                           float? spaceAfter = null,
                                           Font font = null)
    {
      if (string.IsNullOrWhiteSpace(content))
        return;

      font = font ?? GetFont();

      var before = spaceBefore ?? MedSpacing;
      var after = spaceAfter ?? MedSpacing;

      var phrase = new Phrase(content, font);
      var paragraph = new Paragraph
      {
        Alignment = alignment,
        SpacingBefore = before,
        SpacingAfter = after,
        Leading = font.Size + LineSpacing,
        Font = font
      };

      paragraph.Add(phrase);

      doc.Add(paragraph);
    }

  }
}
