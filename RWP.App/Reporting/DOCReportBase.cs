using RWP.App.Models;

namespace RWP.App.Reporting
{
  public abstract class DOCReportBase<TContext> : ReportBase<TContext>
  {
    protected DOCReportBase(TContext context, ReportSettingsModel settings) : base(context, settings)
    {
    }
  }
}
