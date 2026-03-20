namespace CSDB_UtopiaModel.Model;

public class LogEventArgs : EventArgs
{
    public string Message { get; init; }

    public LogEventArgs(string message) => Message = message;
}