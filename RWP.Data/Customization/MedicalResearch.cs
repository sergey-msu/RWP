using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class MedicalResearch : IEntityWithId, IUpdatable<MedicalResearch>
  {
    void IUpdatable.UpdateFrom(object other, System.Data.Linq.DataContext dataContext)
    {
      UpdateFrom((MedicalResearch)other, dataContext);
    }

    public void UpdateFrom(MedicalResearch other, System.Data.Linq.DataContext dataContext)
    {
      var context = (DataContext)dataContext;

      this.ExaminationDate = other.ExaminationDate;
      this.ResearchDate = other.ResearchDate;
      this.Number = other.Number;
      this.SliceThickness = other.SliceThickness;
      this.UseContrast = other.UseContrast;
      this.Dose = other.Dose;
      this.Content = other.Content;
      this.Conclusion = other.Conclusion;

      this.Customer = DataUtils.Rejuvenate(context.Customers, other.Customer);
      this.Doctor = DataUtils.Rejuvenate(context.Doctors, other.Doctor);
      this.Patient = DataUtils.Rejuvenate(context.Patients, other.Patient);
      this.ResearchTemplate = DataUtils.Rejuvenate(context.ResearchTemplates, other.ResearchTemplate);

      DataUtils.UpdateAssociations<MedicalScanRegime, MedicalResearch, ScanRegime>(context, this.MedicalScanRegimes, other.MedicalScanRegimes);
      DataUtils.UpdateAssociations<MedicalResearchScope, MedicalResearch, ResearchScope>(context, this.MedicalResearchScopes, other.MedicalResearchScopes);
      DataUtils.UpdateAssociations<MedicalResearchAttachment, MedicalResearch, Attachment>(context, this.MedicalResearchAttachments, other.MedicalResearchAttachments);
    }
  }
}
