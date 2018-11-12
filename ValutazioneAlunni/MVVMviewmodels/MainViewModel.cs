using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    #region public properties

    private string _working_folder = "";
    public string WorkingFolder
    {
      get
      {
        return _working_folder;
      }
      set
      {
        _working_folder = value;
        RaisePropertyChanged("WorkingFolder");
      }
    }

    #endregion
  }
}
