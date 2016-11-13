using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class MedicalResearchAttachment : IAssociation<MedicalResearch, Attachment >
  {
    public MedicalResearch First
    {
      get { return this.MedicalResearch; }
      set { this.MedicalResearch = value; }
    }

    public Attachment  Second 
    {
      get { return this.Attachment ; }
      set { this.Attachment  = value; }
    }
  }
}
