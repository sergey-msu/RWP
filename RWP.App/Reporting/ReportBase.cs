using System.IO;
using RWP.App.Infrastructure;
using RWP.App.Models;

namespace RWP.App.Reporting
{
  public abstract class ReportBase<TContext> : IReport
  {
    private readonly TContext _context;
    private readonly ReportSettingsModel _settings;

    protected ReportBase(TContext context, ReportSettingsModel settings)
    {
      if (context == null)
        throw new RwpException("ReportBase.ctor(context=null)");

      _context = context;
      _settings = settings;
    }

    protected TContext Context { get { return _context; } }
    protected ReportSettingsModel Settings { get { return _settings; } }

    public void Create(string path)
    {
      if (File.Exists(path))
        File.Delete(path);

      using (var file = File.Create(path))
      {
        CreateImpl(file);
      }
    }

    protected abstract void CreateImpl(Stream file);
  }
}
