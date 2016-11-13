using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using RWP.Data;
using RWP.App.Models;
using RWP.App.Infrastructure;

namespace RWP.App.Reporting.Reports
{
  public class PatientResearchPDFReport : PDFReportBase<MedicalResearch>
  {
    private static readonly string PATIENT_NAME_FORMAT = Utils.GetResource("ReportPatientNameFormat");
    private static readonly string PATIENT_DATA_FORMAT = Utils.GetResource("ReportPatientDataFormat");
    private static readonly string PATIENT_MALE = Utils.GetResource("ReportPatientMale");
    private static readonly string PATIENT_FEMALE = Utils.GetResource("ReportPatientFemale");
    private static readonly string SCOPE_FORMAT = Utils.GetResource("ReportScopeFormat");
    private static readonly string SCAN_REGIME_FORMAT = Utils.GetResource("ReportScanRegimeFormat");
    private static readonly string NUMBER_FORMAT = Utils.GetResource("ReportNumberFormat");
    private static readonly string THINKNESS_FORMAT = Utils.GetResource("ReportThicknessFormat");
    private static readonly string CONTRAST_FORMAT = Utils.GetResource("ReportContrastFormat");
    private static readonly string YES = Utils.GetResource("ReportYes");
    private static readonly string NO = Utils.GetResource("ReportNo");
    private static readonly string CONCLUSION_FORMAT = Utils.GetResource("ReportConclusionFormat");
    private static readonly string DOSE_FORMAT = Utils.GetResource("ReportDoseFormat");
    private static readonly string DOCTOR_FORMAT = Utils.GetResource("ReportDoctorFormat");

    private const string COMMA = ", ";

    public PatientResearchPDFReport(MedicalResearch context, ReportSettingsModel settings) : base(context, settings)
    {
    }


    protected override void CreateImpl(Document doc)
    {
      BuildCustomerName(doc);
      BuildCustomerAddress(doc);
      BuildCustomerResearchPlace(doc);

      BuildPatientName(doc);
      BuildPatientData(doc);
      BuildScope(doc);
      BuildScanRegime(doc);
      BuildNumber(doc);
      BuildThickness(doc);
      BuildContrast(doc);

      BuildResearchContent(doc);
      BuildResearchConclusion(doc);

      BuildDose(doc);
      BuildDoctorInfo(doc);
    }

    private void BuildCustomerName(Document doc)
    {
      CreateParagraph(doc, Context.Customer.Name, Element.ALIGN_CENTER, spaceAfter: BigSpacing);
    }

    private void BuildCustomerAddress(Document doc)
    {
      CreateParagraph(doc, Context.Customer.Address, Element.ALIGN_CENTER, spaceAfter: BigSpacing);
    }

    private void BuildCustomerResearchPlace(Document doc)
    {
      var place = Context.Customer.ResearchPlace;
      if (string.IsNullOrWhiteSpace(place))
        return;

      var table = new PdfPTable(1) { SpacingBefore = MedSpacing, SpacingAfter = LargeSpacing };
      var cell = new PdfPCell(new Phrase(place, GetFont()));
      cell.HorizontalAlignment = Element.ALIGN_CENTER;
      table.AddCell(cell);

      doc.Add(table);
    }

    private void BuildPatientName(Document doc)
    {
      var patientName = Utils.ToFullName(Context.Patient.FirstName, Context.Patient.MiddleName, Context.Patient.LastName, true);
      var line = string.Format(PATIENT_NAME_FORMAT, patientName);

      CreateParagraph(doc, line, Element.ALIGN_JUSTIFIED);
    }

    private void BuildPatientData(Document doc)
    {
      var line = string.Format(PATIENT_DATA_FORMAT,
                                Context.Patient.DOB,
                                Context.Patient.Sex ? PATIENT_MALE : PATIENT_FEMALE,
                                Context.ExaminationDate);

      CreateParagraph(doc, line, Element.ALIGN_JUSTIFIED);
    }

    private void BuildScope(Document doc)
    {
      var scopes = Context.MedicalResearchScopes.Select(rs => rs.ResearchScope.Name.ToLowerInvariant());
      var scopesString = string.Join(COMMA, scopes);
      var line = string.Format(SCOPE_FORMAT, scopesString);

      CreateParagraph(doc, line, Element.ALIGN_JUSTIFIED);
    }

    private void BuildScanRegime(Document doc)
    {
      var regimes = Context.MedicalScanRegimes.Select(sr => sr.ScanRegime.Name.ToLowerInvariant());
      var regimesString = string.Join(COMMA, regimes);
      var line = string.Format(SCAN_REGIME_FORMAT, regimesString);

      CreateParagraph(doc, line, Element.ALIGN_JUSTIFIED);
    }

    private void BuildNumber(Document doc)
    {
      var line = string.Format(NUMBER_FORMAT, Context.Number ?? string.Empty);

      CreateParagraph(doc, line, Element.ALIGN_JUSTIFIED);
    }

    private void BuildThickness(Document doc)
    {
      var line = string.Format(THINKNESS_FORMAT, Context.SliceThickness ?? string.Empty);

      CreateParagraph(doc, line, Element.ALIGN_JUSTIFIED);
    }

    private void BuildContrast(Document doc)
    {
      var line = string.Format(CONTRAST_FORMAT, Context.UseContrast ? YES : NO);

      CreateParagraph(doc, line, Element.ALIGN_JUSTIFIED);
    }

    private void BuildResearchContent(Document doc)
    {
      CreateParagraph(doc, Context.Content, Element.ALIGN_JUSTIFIED);
    }

    private void BuildResearchConclusion(Document doc)
    {
      var conslusion = string.Format(CONCLUSION_FORMAT, Context.Conclusion ?? string.Empty);
      CreateParagraph(doc, conslusion, Element.ALIGN_JUSTIFIED, spaceBefore: LargeSpacing, spaceAfter: LargeSpacing);
    }

    private void BuildDose(Document doc)
    {
      var line = string.Format(DOSE_FORMAT, Context.Dose);

      CreateParagraph(doc, line, Element.ALIGN_JUSTIFIED);
    }

    private void BuildDoctorInfo(Document doc)
    {
      if (Context.Doctor == null)
        return;

      var table = new PdfPTable(3);
      table.HorizontalAlignment = Element.ALIGN_RIGHT;

      var position = string.Format(DOCTOR_FORMAT, Context.Doctor.Position ?? string.Empty);
      var cell = new PdfPCell(new Phrase(position, GetFont()));
      cell.Border = Rectangle.NO_BORDER;
      cell.VerticalAlignment = Element.ALIGN_BOTTOM;
      table.AddCell(cell);

      if (Context.Doctor.Print != null && Context.Doctor.Print.Length > 0)
      {
        var printBytes = Context.Doctor.Print.ToArray();
        var print = Image.GetInstance(printBytes);
        print.ScaleAbsolute(100 * print.ScaledWidth / print.ScaledHeight, 100);
        print.Alignment = Element.ALIGN_RIGHT;
        cell = new PdfPCell(print);
        cell.Border = Rectangle.NO_BORDER;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);
      }

      var name = Utils.ToFullName(Context.Doctor.FirstName, Context.Doctor.MiddleName, Context.Doctor.LastName, true);
      cell = new PdfPCell(new Phrase(name, GetFont()));
      cell.Border = Rectangle.NO_BORDER;
      cell.VerticalAlignment = Element.ALIGN_BOTTOM;
      table.AddCell(cell);

      doc.Add(table);
    }
  }
}
