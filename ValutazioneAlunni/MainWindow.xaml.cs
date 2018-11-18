using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ValutazioneAlunni.Data;
using ValutazioneAlunni.MVVMmodels;
using ValutazioneAlunni.MVVMviewmodels;
using ValutazioneAlunni.MVVMviews;
using ValutazioneAlunni.Utilities;

namespace ValutazioneAlunni
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    #region private members

    private Log _log = Log.Instance;
    private ApplicationSettings _settings;

    #endregion

    public MainWindow()
    {
      InitializeComponent();

      _log.Info("");
      _log.Info("***************************************************");
      _log.Info("Start application at " + DateTime.Now.ToString());
      window_init();
      _log.Info(Title);

      settings_init();
      data_init();
      mvvm_init();

      _log.Info("Settings:");
      _log.Info(_settings.ToString());
    }

    private void window_init()
    {
      Title = Utils.GetApplicationName() + " " + Utils.GetVersion();
    }

    private void settings_init()
    {
      _settings = ApplicationSettings.Instance;
    }

    private void data_init()
    {
      //DataContainer.Instance.LoadFakeData();
      DataContainer.Instance.LoadData();
    }

    private void mvvm_init()
    {
      MainGrid.DataContext = new MainViewModel();

      TabStudentsContainer.Children.Add(new StudentsView());
      TabStudentsContainer.DataContext = new StudentsViewModel();

      TabEvaluationSchemeContainer.Children.Add(new EvaluationSchemeView());
      TabEvaluationSchemeContainer.DataContext = new EvaluationSchemeViewModel();
    }
  }
}
