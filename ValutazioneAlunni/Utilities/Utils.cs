using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ValutazioneAlunni.Utilities
{
  public static class Utils
  {
    public static string GetApplicationPath()
    {
      System.Reflection.Assembly asm;
      asm = System.Reflection.Assembly.GetExecutingAssembly();

      return asm.Location;
    }

    public static string GetApplicationName()
    {
      System.Reflection.Assembly asm;
      asm = System.Reflection.Assembly.GetExecutingAssembly();

      return asm.GetName().Name;
    }

    public static string GetVersion()
    {
      System.Reflection.Assembly asm;
      asm = System.Reflection.Assembly.GetExecutingAssembly();

      return asm.GetName().Version.Major.ToString() + "." + asm.GetName().Version.Minor.ToString() + "." + asm.GetName().Version.Revision.ToString();
    }

  }
}
