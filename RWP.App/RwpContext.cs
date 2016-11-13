using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using RWP.App.Infrastructure.Mvvm;

namespace RWP.App
{
  /// <summary>
  /// Application-wide context
  /// </summary>
  internal class RwpContext : NotifierBase
  {
    private static readonly object _sync = new object();

    private RwpContext()
    {
      _tabs = new ObservableCollection<RwpTabBase>();
      _tabs.CollectionChanged += (o, e) => OnPropertyChanged("Tabs");

      CloseTabCommand = new DelegateCommand<RwpTabBase>(CloseTab);
    }


    private static RwpContext _instance;
    /// <summary>
    /// Context singleton
    /// </summary>
    public static RwpContext Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (_sync)
          {
            if (_instance == null)
              _instance = new RwpContext();
          }
        }

        return _instance;
      }
    }

    private string _status;
    /// <summary>
    /// Current application status (to be shown in status bar)
    /// </summary>
    public string Status
    {
      get { return _status; }
      set
      {
        _status = value;
        OnPropertyChanged("Status");
      }
    }

    private readonly ObservableCollection<RwpTabBase> _tabs;
    /// <summary>
    /// Currently opened application windows
    /// </summary>
    public IList<RwpTabBase> Tabs { get { return _tabs; } }

    /// <summary>
    /// A command to close all application tabs
    /// </summary>
    public ICommand CloseTabCommand { get; private set; }

    private void CloseTab(RwpTabBase tab)
    {
      RwpRoot.Close(tab);
    }
  }
}
