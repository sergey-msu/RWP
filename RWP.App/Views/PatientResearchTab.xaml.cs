using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.ViewModels;
using RWP.Data;

namespace RWP.App.Views
{
  public partial class PatientResearchTab: RwpTabBase
  {
    public PatientResearchTab(TabManager manager, string id, MedicalResearch research) : base(manager, id)
    {
      InitializeComponent();
      this.DataContext = new PatientResearchViewModel(research);
    }
  }
}
