using RWP.App.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RWP.App.Infrastructure
{
  public class ErrorHandler
  {
    private readonly string PK_VIOLATION_ERROR = "Violation of UNIQUE KEY constraint".ToLower();
    private readonly string PK_VIOLATION_MESSAGE = Utils.GetResource("UniqueKeyViolationMessage");
    private readonly string CONNECTION_ERROR = "The server was not found or was not accessible".ToLower();
    private readonly string CONNECTION_MESSAGE = Utils.GetResource("ServerConnectionMessage");
    private readonly string DB_NOTFOUND_ERROR = "Cannot open database".ToLower();
    private readonly string DB_NOTFOUND_MESSAGE = Utils.GetResource("DbNotFoundMessage");
    private readonly string DATA_ERROR_MESSAGE = Utils.GetResource("DataErrorMessage");
    private readonly string RWP_ERROR_MESSAGE = Utils.GetResource("RWPErrorMessage");
    private readonly string UNAUTH_ACCESS_ERROR_MESSAGE = Utils.GetResource("UnauthAccessErrorMessage");

    public virtual void Handle(Exception error)
    {
      if (error is SqlException)
        HandleSql((SqlException)error);
      if (error is RwpException)
        HandleRwp((RwpException)error);
      if (error is UnauthorizedAccessException)
        HandleUnauthorizedAccess((UnauthorizedAccessException)error);
      else
        HandleGeneral(error);
    }

    public virtual void HandleSql(SqlException error)
    {
      if (error.Message.ToLower().Contains(PK_VIOLATION_ERROR))
        ShowErrorMessage(PK_VIOLATION_MESSAGE);
      else if (error.Message.ToLower().Contains(CONNECTION_ERROR))
        ShowErrorMessage(CONNECTION_MESSAGE);
      else if (error.Message.ToLower().Contains(DB_NOTFOUND_ERROR))
        ShowErrorMessage(DB_NOTFOUND_MESSAGE);
      else
        ShowErrorMessage(string.Format(DATA_ERROR_MESSAGE, error.Message));
    }

    public virtual void HandleRwp(RwpException error)
    {
      ShowErrorMessage(string.Format(RWP_ERROR_MESSAGE, error.Message));
    }

    public virtual void HandleUnauthorizedAccess(UnauthorizedAccessException error)
    {
      ShowErrorMessage(UNAUTH_ACCESS_ERROR_MESSAGE);
    }

    public void HandleGeneral(Exception error)
    {
      ShowErrorMessage(string.Format(RWP_ERROR_MESSAGE, error.Message));
    }
    

    private void ShowErrorMessage(string message, string caption = null)
    {
      RwpMessageBox.Show(message);
    }
  }
}
