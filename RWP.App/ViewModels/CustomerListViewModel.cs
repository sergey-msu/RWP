using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Models;
using RWP.Data;
using RWP.Data.Filters;
using RWP.Data.Repositories;

namespace RWP.App.ViewModels
{
  public class CustomerListViewModel : ItemListViewModelBase<Customer, CustomerModel, CustomerRepository>
  {
    private readonly string CUSTOMER_HAS_RESEARCHES_MESSAGE = Utils.GetResource("CustomerHasResearchesMessage");
    private readonly string TITLE = Utils.GetResource("CustomerListTitle");

    private readonly MedicalResearchRepository _researchRepository;

    public CustomerListViewModel()
    {
      _researchRepository = new MedicalResearchRepository();
    }

    #region Overrides

    public override string Header { get { return TITLE; } }

    protected override bool CheckCanDelete(out string message)
    {
      message = null;
      var hasResearches = _researchRepository.HasResearchesWithCustomer(SelectedItem.Entity.Id);
      if (hasResearches)
      {
        message = string.Format(CUSTOMER_HAS_RESEARCHES_MESSAGE, SelectedItem.Name);
        return false;
      }

      return true;
    }

    protected override FilterBase<Customer> GetFilter()
    {
      return new CustomerNameFilter { Name = Filter };
    }

    #endregion Overrides
  }
}
