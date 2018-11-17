using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ValutazioneAlunni.Data;
using ValutazioneAlunni.MVVMmodels;
using ValutazioneAlunni.Utilities;

namespace ValutazioneAlunni.MVVMviewmodels
{
  class EvaluationSchemeViewModel : BaseViewModel
  {
    #region private fields

    private Log _log = Log.Instance;
    private ApplicationSettings _settings = ApplicationSettings.Instance;
    private EvaluationScheme _evaluation_scheme = null;
    private string _evaluation_scheme_print = "";

    #endregion

    #region init and deinit

    public EvaluationSchemeViewModel()
    {
      scheme_init();
    }

    ~EvaluationSchemeViewModel()
    {
      //scheme_save();
    }

    #endregion

    #region private functions

    private void scheme_init()
    {
      scheme_load();
    }

    private string get_file_name()
    {
      return "RubricaValutativa_rev" + _evaluation_scheme.Release + ".xml";
    }

    private void scheme_load()
    {
      _evaluation_scheme = DataContainer.Instance.EvaluationScheme;
      _evaluation_scheme_print = _evaluation_scheme.ToString();
    }

    private void scheme_save()
    {
      try
      {
        string complete_file_name = Path.Combine(_settings.WorkingFolder, get_file_name());
        XmlSerializer serializer = new XmlSerializer(typeof(EvaluationScheme));
        StreamWriter file_writer = new StreamWriter(complete_file_name);
        serializer.Serialize(file_writer, _evaluation_scheme);
        file_writer.Close();
      }
      catch (Exception exc)
      {
        _log.Error("Exception in scheme_save(): " + exc.Message);
      }
    }

    #endregion

    #region public properties

    public string EvaluationSchemePrint
    {
      get
      {
        return _evaluation_scheme_print;
      }
    }

    #endregion
  }
}
