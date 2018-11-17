using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ValutazioneAlunni.Utilities;

namespace ValutazioneAlunni.MVVMmodels
{
  [Serializable]
  class ApplicationSettings
  {
    public static string WorkingFolder = "";

    private const string APPLICATION_SETTINGS_FILE_NAME = "application_settings.xml";

    private static Log _log = Log.Instance;

    // http://csharpindepth.com/Articles/General/Singleton.aspx
    private static ApplicationSettings instance = null;
    private static readonly object padlock = new object();

    ApplicationSettings()
    {
    }

    ~ApplicationSettings()
    {
      save();
    }

    public static ApplicationSettings Instance
    {
      get
      {
        lock (padlock)
        {
          if (instance == null)
          {
            load();
          }
          return instance;
        }
      }
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      sb.AppendLine("Application settings");
      sb.AppendLine("WorkingFolder: " + WorkingFolder);

      return sb.ToString();
    }

    #region private functions

    private static void load_default_values()
    {
      WorkingFolder = Utils.GetApplicationPath();
    }

    private static bool load()
    {
      try
      {
        string settings_file = get_settings_file();

        _log.Info("Loading application settings from <" + settings_file + ">");

        XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
        FileStream file_stream = new FileStream("c:/prefs.xml", FileMode.Open);

        instance = (ApplicationSettings)serializer.Deserialize(file_stream);

        return true;
      }
      catch (Exception exc)
      {
        _log.Error("Exception in Load(): " + exc.Message);
        instance = new ApplicationSettings();
        load_default_values();
        return false;
      }
    }

    private static bool save()
    {
      // https://azuliadesigns.com/saving-user-preferences-settings-application/

      try
      {
        string settings_file = get_settings_file();

        _log.Info("Saving application settings from <" + settings_file + ">");

        XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
        StreamWriter file_writer = new StreamWriter(settings_file);
        serializer.Serialize(file_writer, instance);
        file_writer.Close();

        return true;
      }
      catch (Exception exc)
      {
        _log.Error("Exception in Save(): " + exc.Message);
        return false;
      }
    }

    private static string get_settings_file()
    {
      try
      {
        string settings_path = Utils.GetApplicationPath();
        return Path.Combine(settings_path, APPLICATION_SETTINGS_FILE_NAME);
      }
      catch
      {
        return "";
      }
    }

    #endregion
  }
}
