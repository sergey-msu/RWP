using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class MedicalScanRegime : IAssociation<MedicalResearch, ScanRegime>
  {
    public MedicalResearch First
    {
      get { return this.MedicalResearch; }
      set { this.MedicalResearch = value; }
    }

    public ScanRegime Second 
    {
      get { return this.ScanRegime; }
      set { this.ScanRegime = value; }
    }
  }
}
