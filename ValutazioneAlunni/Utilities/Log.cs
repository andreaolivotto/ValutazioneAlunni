using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValutazioneAlunni.Utilities
{
  class Log
  {
    // http://csharpindepth.com/Articles/General/Singleton.aspx

    private static Log instance = null;
    private static readonly object padlock = new object();

    Log()
    {
    }

    public static Log Instance
    {
      get
      {
        lock (padlock)
        {
          if (instance == null)
          {
            instance = new Log();
          }
          return instance;
        }
      }
    }

    public void Info(string LogMessage)
    {
      Console.WriteLine("[INFO] " + LogMessage);
    }

    public void Error(string LogMessage)
    {
      Console.WriteLine("[ERROR] " + LogMessage);
    }
  }
}
