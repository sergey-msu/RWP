using System;
using System.Linq;
using System.Collections.Generic;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.Data;

namespace RWP.App.Models
{
  public class PatientListItemModel : ModelBase<Patient>
  {
    public PatientListItemModel(Patient entity)
      : base(entity)
    {
      Researches = new List<ResearchListItemModel>();
    }

    #region Properties

    public string FullName { get; private set; }

    public DateTime DOB { get; private set; }

    public DateTime? LastResearchDate { get; private set; }

    public List<ResearchListItemModel> Researches { get; private set; }

    #endregion Properties

    #region Overrides

    protected override void ResetFromSourceImpl()
    {
      FullName = Utils.ToFullName(Entity.FirstName, Entity.MiddleName, Entity.LastName);
      DOB = Entity.DOB;
      LastResearchDate = Entity.MedicalResearches.Any() ?
                          Entity.MedicalResearches.Max(r => r.ResearchDate) :
                          (DateTime?)null;
    }

    protected override void SaveToSourceImpl()
    {
      throw new NotSupportedException();
    }

    #endregion Overrides
  }
}