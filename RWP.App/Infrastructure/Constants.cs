using RWP.App.Infrastructure.Enums;
using System.Collections.Generic;

namespace RWP.App.Infrastructure
{
  public static class Constants
  {
    public const int MIN_YEAR = 1900;

    public const string WINDOW_TEMPLATE_LIST = "TemplateListWindow";
    public const string WINDOW_SCOPE_LIST = "ScopeListWindow";
    public const string WINDOW_DOCTOR_LIST = "DoctorListWindow";
    public const string WINDOW_CUSTOMER_LIST = "CustomerListWindow";
    public const string WINDOW_PATIENT_LIST = "PatientListWindow";
    public const string WINDOW_RESEARCH_LIST = "ResearchListWindow";
    public const string WINDOW_PATIENT_RESERCH = "PatientResearchWindow_{0}_{1}";

    public const string COMMAND_REFRESH = "RefreshCommand";
    public const string COMMAND_FOCUS_PATIENT = "FocusPatientCommand";
    public const string COMMAND_CREATE_NEW_PATIENT = "CreateNewPatientCommand";

    public const string FONT_TIMES = "Times New Roman";
    public const string FONT_VERDANA = "Verdana";
    public const string FONT_ARIAL = "Arial";

    public static readonly List<int> ITEMS_ON_SCREEN = new List<int> { 10, 20, 50 };
    public static readonly List<string> FONTS = new List<string> { FONT_TIMES }; //, FONT_VERDANA, FONT_ARIAL };
    public static readonly List<int> FONT_SIZES = new List<int> { 9, 10, 11, 12, 13, 14, 15, 16 };
    public static readonly List<int> LINE_MARGINS = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    public static readonly List<int> DOC_MARGINS = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90 };

    public static readonly string DFT_FONT = FONT_TIMES;
    public static readonly int DFT_FONT_SIZE = 12;
    public static readonly int DFT_LINE_MARGIN = 2;
    public static readonly int DFT_DOC_MARGIN = 60;

    public const string FONT_FAMILY_SETTING = "FontFamily";
    public const string FONT_SIZE_SETTING = "FontSize";
    public const string LINE_MARGIN_SETTING = "LineMargin";
    public const string DOC_MARGIN_SETTING = "DocMargin";

    public static readonly Dictionary<string, object> DFT_SETTINGS = new Dictionary<string, object>
      {
        { "FontFamily", DFT_FONT },
        { "FontSize", DFT_FONT_SIZE },
        { "LineMargin", DFT_LINE_MARGIN },
        { "DocMargin", DFT_DOC_MARGIN }
      };

    public const string PATIENT_RESEARCH_REPORT_NAME = "PatientResearch";
    public const string GENERAL_REPORT_NAME = "General";
    public const string PDF_REPORT_TYPE = "PDF";
  }
}