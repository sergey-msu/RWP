using RWP.App.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWP.App.Infrastructure
{
  public interface INamed
  {
    string Name { get; }
  }

  /// <summary>
  /// Represents entity that can react to some external command
  /// </summary>
  public interface ICommandExecuteAware
  {
    bool Execute(string command, params object[] args);
  }

  /// <summary>
  /// Represents entity (usually View Model) that knows about it's parent host ID (usually host window name) 
  /// </summary>
  public interface IHostAware
  {
    string HostID { get; set; }
  }

  public interface INavigationAware
  {
    bool OnNavigatedFrom();
  }
}
