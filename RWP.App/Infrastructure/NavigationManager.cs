using System;
using System.Windows;
using RWP.App.Infrastructure.Mvvm;

namespace RWP.App.Infrastructure
{
  /// <summary>
  /// Facilitates navigation logic inside attached window
  /// </summary>
  public class NavigationManager
  {
    public NavigationManager(RwpTabBase host)
    {
      if (host == null)
        throw new RwpException("NavigationManager.ctor(region=null)");
      if (_host != null)
        throw new RwpException("NavigationManager.ctor(): navigation was already attached to another window");

      _host = host;
    }

    private readonly object _sync = new object();
    private RwpTabBase _host;

    /// <summary>
    /// Navigates to a view with given type
    /// </summary>
    public virtual void To<TView>(params object[] args) where TView : FrameworkElement
    {
      lock (_sync)
      {
        var canNavigate = DoNavigateFrom();
        if (!canNavigate) return;

        var view = Activator.CreateInstance(typeof(TView), args);
        _host.Content = view;

        DoNavigateTo();
      }
    }


    protected virtual bool DoNavigateFrom()
    {
      if (_host == null)
        throw new RwpException("NavigationManager.DoNavigateFrom(): navigation has to been attached to some window");
      if (_host.Content == null)
        return true;

      var prevNavVM = ((FrameworkElement)_host.Content).DataContext as INavigationAware;
      if (prevNavVM != null)
        return prevNavVM.OnNavigatedFrom();

      return true;
    }

    protected virtual void DoNavigateTo()
    {
      if (_host == null)
        throw new RwpException("NavigationManager.DoNavigateFrom(): navigation has to been attached to some window");
      if (_host.Content == null)
        return;

      var nextVM = ((FrameworkElement)_host.Content).DataContext;
      if (nextVM is IHostAware)
        ((IHostAware)nextVM).HostID = _host.HostID;
    }
  }
}
