using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ValutazioneAlunni.MVVMmodels;
using ValutazioneAlunni.MVVMutils;
using ValutazioneAlunni.Utilities;

namespace ValutazioneAlunni.MVVMviewmodels
{
  class ExportViewModel : BaseViewModel
  {
    #region private fields

    private static readonly Log _log = Log.Instance;
    private ApplicationSettings _settings = ApplicationSettings.Instance;

    private bool _edit_mode;

    #endregion

    #region init and deinit

    public ExportViewModel()
    {
      EditMode = true;
    }

    #endregion

    #region messenger

    private void messenger_init()
    {
      Messenger.Default.Register<StudentData>(this, messenger_export_student_word, "ExportStudentWord");
    }

    private void messenger_export_student_word(StudentData s)
    {
    }

    #endregion

    #region private functions

    private void choose_export_folder()
    {

    }

    private void export_student_word(StudentData s)
    {

    }

    #endregion

    #region public properties

    public bool EditMode
    {
      get
      {
        return _edit_mode;
      }
      private set
      {
        _edit_mode = value;
        RaisePropertyChanged("ReadOnlyMode");
        RaisePropertyChanged("EditMode");
      }
    }

    public bool ReadOnlyMode
    {
      get
      {
        return !EditMode;
      }
    }

    public string TeacherFirstName
    {
      get
      {
        return _settings.TeacherFirstName;
      }
      set
      {
        if (EditMode == true)
        {
          _settings.TeacherFirstName = value;
        }
        RaisePropertyChanged("TeacherFirstName");
      }
    }

    public string TeacherLastName
    {
      get
      {
        return _settings.TeacherLastName;
      }
      set
      {
        if (EditMode == true)
        {
          _settings.TeacherLastName = value;
        }
        RaisePropertyChanged("TeacherLastName");
      }
    }

    public string EvaluationTitle
    {
      get
      {
        return _settings.EvaluationTitle;
      }
      set
      {
        if (EditMode == true)
        {
          _settings.EvaluationTitle = value;
        }
        RaisePropertyChanged("EvaluationTitle");
      }
    }

    private string _date = DateTime.Now.ToString("dd/MM/yyyy");
    public string DateStringName
    {
      get
      {
        return _date;
      }
      set
      {
        if (EditMode == true)
        {
          _date = value;
        }
        RaisePropertyChanged("DateStringName");
      }
    }

    public string ExportFolder
    {
      get
      {
        return _settings.ExportFolder;
      }
      private set
      { 
        _settings.ExportFolder = value;
        RaisePropertyChanged("ExportFolder");
      }
    }

    #endregion

    #region commands

    private ICommand _set_export_folder_cmd;
    public ICommand SetExportFolderCmd
    {
      get
      {
        if (_set_export_folder_cmd == null)
        {
          _set_export_folder_cmd = new RelayCommand(
              param => this.set_export_folder(),
              param => this.can_set_export_folder()
              );
        }
        return _set_export_folder_cmd;
      }
    }

    private bool can_set_export_folder()
    {
      return true;
    }

    private void set_export_folder()
    {
      using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
      {
        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
        if (result == System.Windows.Forms.DialogResult.OK)
        {
          ExportFolder = dialog.SelectedPath;
        }
      }
    }

    private ICommand _export_all_word_cmd;
    public ICommand ExportAllWordCmd
    {
      get
      {
        if (_export_all_word_cmd == null)
        {
          _export_all_word_cmd = new RelayCommand(
              param => this.export_all_word(),
              param => this.can_export_all_word()
              );
        }
        return _export_all_word_cmd;
      }
    }

    private bool can_export_all_word()
    {
      return true;
    }

    private void export_all_word()
    {
    }

    #endregion
  }
}
