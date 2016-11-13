using RWP.App.Infrastructure;
using RWP.App.ViewModels;

namespace RWP.App.Views
{
  public class DoctorListTab : ItemListTabBase<DoctorDetailsView>
  {
    public DoctorListTab(TabManager manager, string id) : base(manager, id)
    {
      InitializeComponent();
      this.DataContext = new DoctorListViewModel();
    }
  }

  public class CustomerListTab : ItemListTabBase<CustomerDetailsView>
  {
    public CustomerListTab(TabManager manager, string id) : base(manager, id)
    {
      InitializeComponent();
      this.DataContext = new CustomerListViewModel();
    }
  }

  public class PatientListTab : ItemListTabBase<PatientDetailsView>
  {
    public PatientListTab(TabManager manager, string id) : base(manager, id)
    {
      InitializeComponent();
      this.DataContext = new PatientListViewModel();
    }
  }

  public class TemplateListTab : ItemListTabBase<TemplateDetailsView>
  {
    public TemplateListTab(TabManager manager, string id) : base(manager, id)
    {
      InitializeComponent();
      this.DataContext = new TemplateListViewModel();
    }
  }

  public class ScopeListTab : ItemListTabBase<ScopeDetailsView>
  {
    public ScopeListTab(TabManager manager, string id) : base(manager, id)
    {
      InitializeComponent();
      this.DataContext = new ScopeListViewModel();
    }
  }
}
