using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class MedicalResearchScope : IAssociation<MedicalResearch, ResearchScope>
  {
    public MedicalResearch First
    {
      get { return this.MedicalResearch; }
      set { this.MedicalResearch = value; }
    }

    public ResearchScope Second 
    {
      get { return this.ResearchScope; }
      set { this.ResearchScope = value; }
    }
  }
}
