using System;
using RWP.App.Views;

namespace RWP.Shell
{
  public class Program
  {
    /// <summary>
    /// Application entry point
    /// </summary>
    [STAThread]
    public static void Main(string[] agrs)
    {
      try
      {
        var app = new App();
        app.Run(new ShellWindow());
      }
      catch (Exception ex)
      {
        Console.WriteLine("Critical error: " + ex.Message);
        Environment.ExitCode = 1;
      }
    }
  }
}
