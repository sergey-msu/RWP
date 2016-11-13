using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using RWP.App.Infrastructure.Mvvm;

namespace RWP.App.Infrastructure
{
  /// <summary>
  /// Application tab manager
  /// </summary>
  public class TabManager
  {
    private static readonly TabManager _instance = new TabManager();
    public static TabManager Instance { get { return _instance; } }

    private TabManager()
    {
      _tabs = new Dictionary<string, RwpTabBase>();
    }

    private readonly object _sync = new object();
    private TabControl _host;
    private readonly Dictionary<string, RwpTabBase> _tabs;

    /// <summary>
    /// Collection of all managed tab
    /// </summary>
    public IEnumerable<RwpTabBase> Tabs { get { return _tabs.Select(w => w.Value).ToList(); } }

    /// <summary>
    /// Atteches current TabManager to the host TabControl
    /// </summary>
    public virtual void AttachTo(TabControl host)
    {
      lock (_sync)
      {
        if (host == null)
          throw new RwpException("TabManager.Attach(host=null)");
        if (_host != null)
          throw new RwpException("TabManager.Attach() error: TabManager has already been attached");

        _host = host;
      }
    }

    /// <summary>
    /// Creates new or activates existing tab
    /// </summary>
    public virtual RwpTabBase Show(Type type, string name, object[] pars = null)
    {
      lock (_sync)
      {
        RwpTabBase tab;
        if (_tabs.TryGetValue(name, out tab))
        {
          _host.SelectedItem = tab;
        }
        else
        {
          var args = new List<object> { this, name };
          if (pars != null) args.AddRange(pars);

          tab = (RwpTabBase)Activator.CreateInstance(type, args.ToArray());
          if (tab.DataContext is IHostAware)
            ((IHostAware)tab.DataContext).HostID = tab.HostID;

          _tabs[tab.HostID] = tab;

          _host.Items.Add(tab);
          _host.SelectedIndex = _host.Items.Count - 1;
        }

        return tab;
      }
    }

    /// <summary>
    /// Creates new or activates existing tab
    /// </summary>
    public TTab Show<TTab>(string name, object[] pars = null)
      where TTab : RwpTabBase
    {
      return (TTab)Show(typeof(TTab), name, pars);
    }

    /// <summary>
    /// Closes tab with a given name
    /// </summary>
    public virtual bool Close(string name)
    {
      lock (_sync)
      {
        RwpTabBase tab;
        if (_tabs.TryGetValue(name, out tab))
        {
          if (!tab.CanClose()) return false;

          _host.Items.Remove(tab);
          _tabs.Remove(tab.HostID);

          tab.OnClosed();
        }

        return true;
      }
    }

    /// <summary>
    /// Closes tab
    /// </summary>
    public bool Close(RwpTabBase tab)
    {
      return Close(tab.HostID);
    }

    public bool Execute(string name, string command, params object[] args)
    {
      RwpTabBase tab;
      if (!_tabs.TryGetValue(name, out tab))
        return false;

      return tab.Execute(command, args);
    }

    public bool Execute<TTab>(string command, params object[] args)
      where TTab : RwpTabBase
    {
      var tabs = _tabs.OfType<TTab>().ToList();
      bool result = true;

      foreach (var tab in tabs)
        result &= tab.Execute(command, args);

      return result;
    }

    public virtual RwpTabBase For(string id)
    {
      return _tabs[id];
    }
  }
}
