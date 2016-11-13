using System;
using System.Linq;
using System.Collections.Generic;
using iTextSharp.text;
using RWP.App.Models;
using RWP.Data;
using RWP.App.Infrastructure;
using iTextSharp.text.pdf;

namespace RWP.App.Reporting.Reports
{
  public class GeneralReportContext
  {
    public GeneralReportContext(IEnumerable<GeneralReportItemModel> items)
    {
      Items = (items != null) ? items.ToList() : new List<GeneralReportItemModel>();
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Doctor Doctor { get; set; }
    public IEnumerable<GeneralReportItemModel> Items { get; private set; }
  }

  public class GeneralPDFReport : PDFReportBase<GeneralReportContext>
  {
    private static readonly string HEADER_FORMAT = Utils.GetResource("GeneralReportHeaderFormat");
    private static readonly string DOCTOR_FORMAT = Utils.GetResource("GeneralReportDoctorFormat");
    private static readonly string PATIENT_INIT_FORMAT = Utils.GetResource("GeneralReportPatientInitialsFormat");
    private static readonly string RESEARCH_NUMBER_FORMAT = Utils.GetResource("GeneralReportResearchNumberFormat");
    private static readonly string RESEARCH_SCOPES_FORMAT = Utils.GetResource("GeneralReportResearchScopesFormat");
    private static readonly string RESEARCH_SCOPES_CNT_FORMAT = Utils.GetResource("GeneralReportResearchScopesCountFormat");
    private static readonly string RESEARCH_DATE_FORMAT = Utils.GetResource("GeneralReportResearchDateFormat");
    private static readonly string FOOTER_FORMAT = Utils.GetResource("GeneralReportFooterFormat");

    public GeneralPDFReport(GeneralReportContext context, ReportSettingsModel settings) : base(context, settings)
    {
    }

    protected override void CreateImpl(Document doc)
    {
      BuildHeader(doc);
      BuildDoctorInfo(doc);
      BuildItems(doc);
      BuildFooter(doc);
    }


    private void BuildHeader(Document doc)
    {
      CreateParagraph(doc, string.Format(HEADER_FORMAT, Context.StartDate, Context.EndDate), Element.ALIGN_CENTER);
    }

    private void BuildDoctorInfo(Document doc)
    {
      CreateParagraph(doc,
                      string.Format(DOCTOR_FORMAT, Utils.ToFullName(Context.Doctor.FirstName, Context.Doctor.MiddleName, Context.Doctor.LastName, true)),
                      Element.ALIGN_CENTER,
                      spaceAfter: LargeSpacing);
    }

    private void BuildItems(Document doc)
    {
      var table = new PdfPTable(6);
      table.SpacingBefore = BigSpacing;
      table.SpacingAfter = BigSpacing;
      table.WidthPercentage = 100;

      var colWidthPercentages = new[] { 7f, 28f, 10f, 25f, 13f, 17f };
      table.SetWidths(colWidthPercentages);

      // headers

      var cell = new PdfPCell();
      cell.VerticalAlignment = Element.ALIGN_BOTTOM;
      table.AddCell(cell);

      cell = new PdfPCell(new Phrase(PATIENT_INIT_FORMAT, GetFont()));
      cell.VerticalAlignment = Element.ALIGN_BOTTOM;
      table.AddCell(cell);

      cell = new PdfPCell(new Phrase(RESEARCH_NUMBER_FORMAT, GetFont()));
      cell.VerticalAlignment = Element.ALIGN_BOTTOM;
      table.AddCell(cell);

      cell = new PdfPCell(new Phrase(RESEARCH_SCOPES_FORMAT, GetFont()));
      cell.VerticalAlignment = Element.ALIGN_BOTTOM;
      table.AddCell(cell);

      cell = new PdfPCell(new Phrase(RESEARCH_SCOPES_CNT_FORMAT, GetFont()));
      cell.VerticalAlignment = Element.ALIGN_BOTTOM;
      table.AddCell(cell);

      cell = new PdfPCell(new Phrase(RESEARCH_DATE_FORMAT, GetFont()));
      cell.VerticalAlignment = Element.ALIGN_BOTTOM;
      table.AddCell(cell);

      // rows

      int counter = 0;

      foreach (var item in Context.Items)
      {
        counter++;

        // counter
        cell = new PdfPCell(new Phrase(counter.ToString(), GetFont()));
        cell.VerticalAlignment = Element.ALIGN_BOTTOM;
        table.AddCell(cell);

        // patient
        cell = new PdfPCell(new Phrase(item.PatientName, GetFont()));
        cell.VerticalAlignment = Element.ALIGN_BOTTOM;
        table.AddCell(cell);

        // number
        cell = new PdfPCell(new Phrase(item.ResearchNumber, GetFont()));
        cell.VerticalAlignment = Element.ALIGN_BOTTOM;
        table.AddCell(cell);

        //scopes
        cell = new PdfPCell(new Phrase(item.ResearchScopes, GetFont()));
        cell.VerticalAlignment = Element.ALIGN_BOTTOM;
        table.AddCell(cell);

        //scopes count
        cell = new PdfPCell(new Phrase(item.ScopesNum.ToString(), GetFont()));
        cell.VerticalAlignment = Element.ALIGN_BOTTOM;
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        table.AddCell(cell);

        //date
        cell = new PdfPCell(new Phrase(string.Format(DATE_FORMAT, item.ExaminationDate), GetFont()));
        cell.VerticalAlignment = Element.ALIGN_BOTTOM;
        table.AddCell(cell);
      }

      doc.Add(table);
    }

    private void BuildFooter(Document doc)
    {
      var font = GetFont(style: Font.BOLD);
      CreateParagraph(doc,
                      string.Format(FOOTER_FORMAT, Context.Items.Count(), Context.Items.Sum(i => i.ScopesNum)),
                      Element.ALIGN_LEFT,
                      font: font);
    }
  }
}
