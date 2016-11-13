using System;
using System.Reflection;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;

namespace RWP.App
{
  /// <summary>
  /// Application root object
  /// </summary>
  internal static class RwpRoot
  {
    /// <summary>
    /// Global application context
    /// </summary>
    public static RwpContext Context { get { return RwpContext.Instance; } }

    public static ErrorHandler _errorHandler;
    /// <summary>
    /// Application error handler
    /// </summary>
    public static ErrorHandler ErrorHandler
    {
      get
      {
        if (_errorHandler == null)
          _errorHandler = new ErrorHandler();

        return _errorHandler;
      }
    }

    /// <summary>
    /// Returns existing managed window by its' name
    /// </summary>
    public static RwpTabBase For(string name)
    {
      return TabManager.Instance.For(name);
    }

    /// <summary>
    /// Creates new or activates existing window
    /// </summary>
    public static RwpTabBase Show(Type type, string name, object[] pars = null)
    {
      return TabManager.Instance.Show(type, name, pars);
    }

    /// <summary>
    /// Creates new or activates existing window
    /// </summary>
    public static TTab Show<TTab>(string id, object[] pars = null)
      where TTab : RwpTabBase
    {
      return TabManager.Instance.Show<TTab>(id, pars);
    }

    /// <summary>
    /// Closes window with a given name
    /// </summary>
    public static bool Close(string id)
    {
      return TabManager.Instance.Close(id);
    }

    /// <summary>
    /// Closes window
    /// </summary>
    public static bool Close<TTab>(TTab tab)
      where TTab : RwpTabBase
    {
      return TabManager.Instance.Close(tab);
    }

    /// <summary>
    /// Executes named command
    /// </summary>
    public static bool Execute(string name, string command, params object[] args)
    {
      return TabManager.Instance.Execute(name, command, args);
    }

    /// <summary>
    /// Executes named command
    /// </summary>
    public static bool Execute<TTab>(string command, params object[] args)
      where TTab : RwpTabBase
    {
      return TabManager.Instance.Execute<TTab>(command, args);
    }

    /// <summary>
    /// Application entry point path
    /// </summary>
    public static string EntryPoint { get { return Assembly.GetEntryAssembly().Location; } }
  }
}
