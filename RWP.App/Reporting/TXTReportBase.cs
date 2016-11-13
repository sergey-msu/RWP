using System.IO;
using RWP.App.Models;

namespace RWP.App.Reporting
{
  public abstract class TXTReportBase<TContext> : ReportBase<TContext>
  {
    protected TXTReportBase(TContext context, ReportSettingsModel settings) : base(context, settings)
    {
    }

    protected override void CreateImpl(Stream output)
    {
      using (var writer = new StreamWriter(output))
      {
        CreateImpl(writer);
      }
    }

    protected abstract void CreateImpl(StreamWriter writer);
  }
}
