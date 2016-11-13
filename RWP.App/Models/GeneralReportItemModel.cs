using System;

namespace RWP.App.Models
{
  public class GeneralReportItemModel
  {
    public string PatientName { get; set; }
    public string ResearchNumber { get; set; }
    public string ResearchScopes { get; set; }
    public int ScopesNum { get; set; }
    public DateTime ExaminationDate { get; set; }
  }
}
