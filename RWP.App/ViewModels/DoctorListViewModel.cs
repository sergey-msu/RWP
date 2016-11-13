using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;
using RWP.App.Models;
using RWP.Data;
using RWP.Data.Repositories;
using RWP.App.Views;
using RWP.Data.Filters;

namespace RWP.App.ViewModels
{
  public class DoctorListViewModel : ItemListViewModelBase<Doctor, DoctorModel, DoctorRepository>
  {
    private const int IMG_MAX_SIZE = 2 * 1024 * 1024;
    private const string DEFAULT_IMAGE_EXT = ".png";
    private readonly string IMAGE_FILES_FILTER = Utils.GetResource("ImageFilesFilter");
    private readonly string LOAD_PRINT_IMAGE_HEADER = Utils.GetResource("LoadPrintImageHeader");
    private readonly string MAX_IMAGE_SIZE_ERROR = Utils.GetResource("MaxImageSizeError");
    private readonly string DOCTOR_HAS_RESEARCHES_MESSAGE = Utils.GetResource("DoctorHasResearchesMessage");
    private readonly string DELETE_PRINT_MESSAGE = Utils.GetResource("DeletePrintMessage");
    private readonly string TITLE = Utils.GetResource("DoctorListTitle");
    
    private readonly MedicalResearchRepository _researchRepository;

    public DoctorListViewModel()
    {
      _researchRepository = new MedicalResearchRepository();

      LoadPrintCommand = new DelegateCommand(LoadPrint, CanLoadPrint);
      RemovePrintCommand = new DelegateCommand(RemovePrint, CanRemovePrint);
    }
    
    #region RemovePrintCommand

    public DelegateCommand RemovePrintCommand { get; private set; }

    private void RemovePrint()
    {
      if (SelectedItem == null)
        throw new InvalidOperationException("Incorrect mode: no selected doctor");

      if (SelectedItem.PrintBitmap == null)
        return;

      var result = RwpMessageBox.Show(DELETE_PRINT_MESSAGE, buttons: MessageBoxButton.YesNo);
      if (result != MessageBoxResult.Yes)
        return;

      SelectedItem.PrintBitmap = null;
      ((DelegateCommand)RemovePrintCommand).RaiseCanExecuteChanged();
    }

    private bool CanRemovePrint()
    {
      return SelectedItem != null && SelectedItem.PrintBitmap != null;
    }

    #endregion RemovePrintCommand

    #region LoadPrintCommand

    public DelegateCommand LoadPrintCommand { get; private set; }

    private void LoadPrint()
    {
      if (SelectedItem == null)
        throw new InvalidOperationException("Incorrect mode: no selected Doctor");

      var dialog = new OpenFileDialog
      {
        AddExtension = true,
        CheckFileExists = true,
        CheckPathExists = true,
        DefaultExt = DEFAULT_IMAGE_EXT,
        Filter = IMAGE_FILES_FILTER,
        Multiselect = false,
        ShowReadOnly = true,
        ValidateNames = true,
        Title = LOAD_PRINT_IMAGE_HEADER
      };

      var result = dialog.ShowDialog();

      if (!result.HasValue || !result.Value)
        return;

      var path = dialog.FileName;
      var file = new FileInfo(path);
      if (file.Length > IMG_MAX_SIZE)
      {
        RwpMessageBox.Show(MAX_IMAGE_SIZE_ERROR);
        return;
      }

      byte[] imageBytes = File.ReadAllBytes(path);
      var src = Utils.DecodeImage(imageBytes);
      SelectedItem.PrintBitmap = src;

      ((DelegateCommand)RemovePrintCommand).RaiseCanExecuteChanged();
    }

    private bool CanLoadPrint()
    {
      return SelectedItem != null;
    }

    #endregion LoadPrintCommand

    #region Overrides
    
    public override string Header { get { return TITLE; } }

    protected override bool CheckCanDelete(out string message)
    {
      message = null;
      var hasResearches = _researchRepository.HasResearchesWithDoctor(SelectedItem.Entity.Id);
      if (hasResearches)
      {
        message = string.Format(DOCTOR_HAS_RESEARCHES_MESSAGE, Utils.ToFullName(SelectedItem.FirstName, SelectedItem.MiddleName, SelectedItem.LastName));
        return false;
      }

      return true;
    }

    protected override void RaiseCanCommandsExecute()
    {
      base.RaiseCanCommandsExecute();

      if (LoadPrintCommand != null) LoadPrintCommand.RaiseCanExecuteChanged();
      if (RemovePrintCommand != null) RemovePrintCommand.RaiseCanExecuteChanged();
    }
    
    protected override FilterBase<Doctor> GetFilter()
    {
      return new DoctorNameFilter { Name = Filter };
    }

    #endregion Overrides
  }
}
