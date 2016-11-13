using System.Windows;
using System.Windows.Controls;

namespace RWP.App.Infrastructure.Mvvm
{
  public abstract class RwpTabBase : TabItem, IHostAware
  {
    public RwpTabBase(TabManager manager, string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new RwpException("RwpViewBase.ctor(name=null|empty)");
      if (manager == null)
        throw new RwpException("RwpViewBase.ctor(manager=null)");

      _manager = manager;
      _hostID = id;
      _navigate = new NavigationManager(this);

      this.Style = Utils.GetResource<Style>("RwpTabItemStyle");

      RwpRoot.Context.Tabs.Add(this);
    }

    private readonly TabManager _manager;
    private readonly NavigationManager _navigate;
    private readonly string _hostID;

    public TabManager Manager { get { return _manager; } }

    public NavigationManager Navigate { get { return _navigate; } }

    public string HostID
    {
      get { return _hostID; }
      set
      {
        throw new RwpException("Cannot set window HostID directly");
      }
    }

    public virtual void NavigateTo<TView>(params object[] args)
      where TView : FrameworkElement
    {
      _navigate.To<TView>(args);
    }

    public virtual bool Execute(string command, params object[] args)
    {
      if (this.DataContext is ICommandExecuteAware)
        return ((ICommandExecuteAware)this.DataContext).Execute(command, args);

      return true;
    }

    public bool CanClose()
    {
      if ((this.DataContext is INavigationAware) &&
          !((INavigationAware)this.DataContext).OnNavigatedFrom())
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    public void OnClosed()
    {
      RwpRoot.Context.Tabs.Remove(this);
    }
  }
}
