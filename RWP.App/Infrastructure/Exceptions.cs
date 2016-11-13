using System;
using System.Runtime.Serialization;

namespace RWP.App.Infrastructure
{
  /// <summary>
  /// Base exception thrown by the RWP
  /// </summary>
  [Serializable]
  public class RwpException : Exception
  {
    public RwpException()
    {
    }

    public RwpException(string message) : base(message)
    {
    }

    public RwpException(string message, Exception inner) : base(message, inner)
    {
    }

    protected RwpException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
