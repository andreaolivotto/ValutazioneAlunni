﻿using System;
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
  public class ApplicationSettings
  {
    public string WorkingFolder = "ERR";

    private const string APPLICATION_SETTINGS_FILE_NAME = "application_settings.xml";

    private Log _log = Log.Instance;

    // http://csharpindepth.com/Articles/General/Singleton.aspx
    private static ApplicationSettings _instance = null;
    private static readonly object _padlock = new object();

    ApplicationSettings()
    {
    }

    ~ApplicationSettings()
    {
      save(this);
    }

    public static ApplicationSettings Instance
    {
      get
      {
        lock (_padlock)
        {
          if (_instance == null)
          {
            ApplicationSettings.load(ref _instance);
            _instance.check_data();
          }
          return _instance;
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

    private static bool load(ref ApplicationSettings appsettings)
    {
      try
      {
        string settings_file = get_settings_file();

        XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
        FileStream file_stream = new FileStream(settings_file, FileMode.Open);

        appsettings = (ApplicationSettings)serializer.Deserialize(file_stream);

        return true;
      }
      catch (Exception)
      {
        _instance = new ApplicationSettings();
        return false;
      }
    }

    private void check_data()
    {
      bool set_default;

      set_default = false;
      if (WorkingFolder == "") set_default = true;
      if (Directory.Exists(WorkingFolder) == false) set_default = true;
      if (set_default)
      {
        string base_path = Path.GetDirectoryName(Utils.GetApplicationPath());
        string new_working_folder = DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + "_dati_valutazionealunni";
        string complete_path = Path.Combine(base_path, new_working_folder);
        Directory.CreateDirectory(complete_path);
        if (Directory.Exists(complete_path) == true)
        {
          WorkingFolder = complete_path;
        }
        else
        {
          WorkingFolder = "";
        }
      }
    }

    private static bool save(ApplicationSettings appsettings)
    {
      // https://azuliadesigns.com/saving-user-preferences-settings-application/

      try
      {
        string settings_file = get_settings_file();

        XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
        StreamWriter file_writer = new StreamWriter(settings_file);
        serializer.Serialize(file_writer, appsettings);
        file_writer.Close();

        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    private static string get_settings_file()
    {
      try
      {
        string settings_path = Path.GetDirectoryName(Utils.GetApplicationPath());
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
