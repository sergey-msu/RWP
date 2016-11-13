using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Models;
using RWP.Data;
using RWP.Data.Filters;
using RWP.Data.Repositories;

namespace RWP.App.ViewModels
{
  public class TemplateListViewModel : ItemListViewModelBase<ResearchTemplate, TemplateModel, ResearchTemplateRepository>
  {
    private readonly string TITLE = Utils.GetResource("TemplateListTitle");

    public TemplateListViewModel()
    {
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

      return true;
    }
    
    protected override FilterBase<ResearchTemplate> GetFilter()
    {
      return new TemplateNameFilter { Name = Filter };
    }

  }
}