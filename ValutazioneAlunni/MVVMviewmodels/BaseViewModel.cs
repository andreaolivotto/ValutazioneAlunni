using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValutazioneAlunni.MVVMviewmodels
{
  class BaseViewModel : INotifyPropertyChanged
  {
    #region INotifyPropertyChanged

    protected void RaisePropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }
}
