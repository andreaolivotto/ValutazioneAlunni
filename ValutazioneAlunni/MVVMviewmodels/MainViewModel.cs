using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValutazioneAlunni.Data;
using ValutazioneAlunni.MVVMmodels;

namespace ValutazioneAlunni.MVVMviewmodels
{
  class MainViewModel : BaseViewModel
  {
    #region private members

    private ApplicationSettings _settings = ApplicationSettings.Instance;

    #endregion

    #region init and deinit 

    public MainViewModel()
    {
    }

    #endregion

    #region private functions

    #endregion

    #region public properties

    public string WorkingFolder
    {
      get
      {
        return _settings.WorkingFolder;
      }
      set
      {
        _settings.WorkingFolder = value;
        RaisePropertyChanged("WorkingFolder");
      }
    }

    #endregion
  }
}
