using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Models;
using RWP.Data;
using RWP.Data.Filters;
using RWP.Data.Repositories;

namespace RWP.App.ViewModels
{
  public class ScopeListViewModel : ItemListViewModelBase<ResearchScope, ScopeModel, ResearchScopeRepository>
  {
    private readonly string TITLE = Utils.GetResource("ScopeListTitle");
    private readonly string SCOPE_HAS_RESEARCHES_MESSAGE = Utils.GetResource("ScopeHasResearchesMessage");
    private readonly MedicalResearchRepository _researchRepository;

    public ScopeListViewModel()
    {
      _researchRepository = new MedicalResearchRepository();
    }

    public override string Header { get { return TITLE; } }

    protected override bool CheckCanDelete(out string message)
    {
      message = null;

      if (SelectedItem.Entity.IsSystem)
      {
        message = ITEM_IS_SYSTEM_MESSAGE;
        return false;
      }

      var hasResearches = _researchRepository.HasResearchesWithScope(SelectedItem.Entity.Id);
      if (hasResearches)
      {
        message = SCOPE_HAS_RESEARCHES_MESSAGE;
        return false;
      }

      return true;
    }

    protected override FilterBase<ResearchScope> GetFilter()
    {
      return new ScopeNameFilter { Name = Filter };
    }
  }
}