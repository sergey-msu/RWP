using System;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Models;
using RWP.Data;
using RWP.Data.Repositories;
using RWP.App.Views;
using RWP.Data.Filters;

namespace RWP.App.ViewModels
{
  public class PatientListViewModel : ItemListViewModelBase<Patient, PatientModel, PatientRepository>
  {
    private readonly string PATIENT_HAS_RESEARCHES_MESSAGE = Utils.GetResource("PatientHasResearchesMessage");
    private readonly string TITLE = Utils.GetResource("PatientListTitle");

    private readonly MedicalResearchRepository _researchRepository;

    public PatientListViewModel()
    {
      _researchRepository = new MedicalResearchRepository();
    }

    #region Overrides

    public override string Header { get { return TITLE; } }

    protected override bool CheckCanDelete(out string message)
    {
      message = null;
      var hasResearches = _researchRepository.HasResearchesWithPatient(SelectedItem.Entity.Id);
      if (hasResearches)
      {
        message = string.Format(PATIENT_HAS_RESEARCHES_MESSAGE, Utils.ToFullName(SelectedItem.FirstName, SelectedItem.MiddleName, SelectedItem.LastName, true));
        return false;
      }

      return true;
    }

    public override bool Execute(string command, params object[] args)
    {
      if (command == Constants.COMMAND_CREATE_NEW_PATIENT)
      {
        if (IsEdit)
          return false;

        CreateItem();
        return true;
      }

      return true;
    }

    protected override PatientModel CreateItem(Patient entity)
    {
      var model = new PatientModel(entity);
      if (entity.Id == 0)
        model.DOB = DateTime.Now;

      return model;
    }

    protected override int SaveImpl()
    {
      var isNew = SelectedItem.Entity.Id == 0;
      SelectedItem.SaveToSource();
      int id = ItemRepository.Upsert(SelectedItem.Entity);

      RwpRoot.Execute(Constants.WINDOW_RESEARCH_LIST, Constants.COMMAND_REFRESH);
      if (isNew)
      {
        RwpRoot.Show<ResearchListTab>(Constants.WINDOW_RESEARCH_LIST);
        RwpRoot.Execute(Constants.WINDOW_RESEARCH_LIST, Constants.COMMAND_FOCUS_PATIENT, id);
      }

      return id;
    }

    protected override FilterBase<Patient> GetFilter()
    {
      return new PatientNameFilter { Name = Filter };
    }

    #endregion Overrides
  }
}
