﻿using System;
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

    private void scheme_load()
    {
      _evaluation_scheme = DataContainer.Instance.EvaluationScheme;
      _evaluation_scheme_print = _evaluation_scheme.ToString();
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
