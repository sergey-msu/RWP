using System;
using System.Linq;
using RWP.App.Infrastructure.Mvvm;
using RWP.Data;

namespace RWP.App.Models
{
  public class ResearchListItemModel : ModelBase<MedicalResearch>
  {
    private const string SCOPES_SEPARATOR = ", ";

    public ResearchListItemModel(MedicalResearch entity)
      : base(entity)
    {
    }

    #region Properties

    public DateTime ExaminationDate { get; set; }

    public DateTime ResearchDate { get; set; }

    public string Scopes { get; set; }

    #endregion Properties

    #region Overrides

    protected override void ResetFromSourceImpl()
    {
      ExaminationDate = Entity.ExaminationDate;
      ResearchDate = Entity.ResearchDate;

      var scopes = Entity.MedicalResearchScopes.Select(s => s.ResearchScope.Name);
      Scopes = string.Join(SCOPES_SEPARATOR, scopes);
    }

    protected override void SaveToSourceImpl()
    {
      throw new NotSupportedException();
    }

    #endregion Overrides
  }
}