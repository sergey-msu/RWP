using RWP.App.ViewModels;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;

namespace RWP.App.Views
{
  public partial class ResearchListTab : RwpTabBase
  {
    public ResearchListTab(TabManager manager, string id) : base(manager, id)
    {
      InitializeComponent();
      this.DataContext = new ResearchListViewModel();
    }
  }
}
