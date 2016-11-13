using System.Windows;
using System.Windows.Threading;

namespace RWP.App.Infrastructure.Mvvm
{
	public abstract class ViewModelBase : ValidatableBase, INavigationAware, IHostAware, ICommandExecuteAware
	{
    protected readonly string VALIDATION_ERRORS_MESSAGE = Utils.GetResource("ValidationErrorsMessage");
    protected readonly string DATA_SUCCESSFULLY_SAVED_MESSAGE = Utils.GetResource("DataSuccessfullySavedMessage");
    protected readonly string EDIT_IN_PROGRESS_MESSAGE = Utils.GetResource("EditInProgressMessage");
    private string _displayName;
    private string _hostID;

    public string HostID
    {
      get { return _hostID; }
      set
      {
        _hostID = value;
        OnPropertyChanged("HostID");
      }
    }

    public string DisplayName
    {
      get { return _displayName; }
      set
      {
        _displayName = value;
        OnPropertyChanged("DisplayName");
      }
    }

    protected Dispatcher Dispather { get { return Application.Current.Dispatcher; } }

    public virtual bool OnNavigatedFrom()
    {
      return true;
    }

    public virtual bool Execute(string command, params object[] args)
    {
      return true;
    }
	}
}