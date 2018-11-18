using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ValutazioneAlunni.Data;
using ValutazioneAlunni.MVVMmodels;
using ValutazioneAlunni.MVVMutils;
using ValutazioneAlunni.Utilities;

namespace ValutazioneAlunni.MVVMviewmodels
{
  class StudentsViewModel : BaseViewModel
  {
    #region private fields

    private static Log _log = Log.Instance;

    private bool _edit_mode;
    private bool _new_mode;
    private StudentData _new_student;
    private StudentData _last_selected_student;

    #endregion

    #region init and deinit

    public StudentsViewModel()
    {
      students_init();
    }

    #endregion

    #region messenger

    private void messenger_send_set_evaluation_scheme()
    {
      Messenger.Default.Send(DataContainer.Instance.EvaluationScheme, "SetEvaluationScheme");
    }

    private void messenger_send_set_student()
    {
      Messenger.Default.Send(_selected_student, "SetCurrentStudent");
    }

    #endregion

    #region private functions

    private void students_init()
    {
      Students = DataContainer.Instance.Students;

      exit_new_mode();
      students_print_all();
      students_select_first();
    }

    private void students_print_all()
    {
      int idx;

      if (Students == null)
      {
        _log.Info("Students list is null!");
        return;
      }
      if (Students.Count == 0)
      {
        _log.Info("Students list is empty!");
        return;
      }

      _log.Info("Total students: " + Students.Count);
      idx = 1;
      foreach (StudentData sd in Students)
      {
        _log.Info("[" + idx.ToString("D2") + "] " + sd.ToString());
        idx++;
      }
    }

    private void students_select_first()
    {
      if (Students == null) return;
      if (Students.Count == 0) return;

      SelectedStudent = Students[0];
    }

    private void studens_refresh_detail_data()
    {
      RaisePropertyChanged("FirstName");
      RaisePropertyChanged("LastName");
      RaisePropertyChanged("BirthDate");
      RaisePropertyChanged("Note");
    }

    private void on_student_selection()
    {
      messenger_send_set_evaluation_scheme();
      messenger_send_set_student();
    }

    private void student_save()
    {
      try
      {
        if (_selected_student == null) return;

        _log.Info("Salva dati studente <" + _selected_student + ">");

        _last_selected_student = _selected_student;
        if (NewMode)
        {
          DataContainer.Instance.Students.Add(_selected_student);
          exit_new_mode();
        }
      }
      catch (Exception exc)
      {
        _log.Error("Exception in student_save(): " + exc.Message);
      }
    }

    private void student_create_new()
    {
      enter_new_mode();
    }

    private void enter_new_mode()
    {
      EditMode = true;
      NewMode = true;
      _new_student = new StudentData(DataContainer.Instance.EvaluationScheme);
      _last_selected_student = SelectedStudent;
      SelectedStudent = _new_student;

      RaisePropertyChanged("NewMode");
      RaisePropertyChanged("NotNewMode");
    }

    private void exit_new_mode()
    {
      EditMode = false;
      NewMode = false;
      _new_student = null;

      if (_last_selected_student == null)
      {
        students_select_first();
      }
      else
      {
        SelectedStudent = _last_selected_student;
      }

      RaisePropertyChanged("NewMode");
      RaisePropertyChanged("NotNewMode");
    }

    #endregion

    #region public properties

    public bool NewMode
    {
      get
      {
        return _new_mode;
      }
      private set
      {
        _new_mode = value;
      }
    }

    public bool NotNewMode
    {
      get
      {
        return !_new_mode;
      }
    }

    public bool EditMode
    {
      get
      {
        return _edit_mode;
      }
      set
      {
        _edit_mode = value;
        RaisePropertyChanged("EditMode");
      }
    }

    private ObservableCollection<StudentData> _students = null;
    public ObservableCollection<StudentData> Students
    {
      get
      {
        return _students;
      }
      set
      {
        if (value != _students)
        {
          _students = value;
        }
        RaisePropertyChanged("FileObjectCollection");
      }
    }

    private StudentData _selected_student;
    public StudentData SelectedStudent
    {
      get
      {
        return _selected_student;
      }
      set
      {
        if (value != _selected_student)
        {
          _selected_student = value;
          on_student_selection();
        }
        studens_refresh_detail_data();
        RaisePropertyChanged("SelectedStudent");
      }
    }

    public string FirstName
    {
      get
      {
        if (_selected_student == null) return "";
        return _selected_student.FirstName;
      }
      set
      {
        if (_edit_mode == true)
        {
          _selected_student.FirstName = value;
        }
        RaisePropertyChanged("FirstName");
      }
    }

    public string LastName
    {
      get
      {
        if (_selected_student == null) return "";
        return _selected_student.LastName;
      }
      set
      {
        if (_edit_mode == true)
        {
          _selected_student.LastName = value;
        }
        RaisePropertyChanged("LastName");
      }
    }

    public DateTime BirthDate
    {
      get
      {
        if (_selected_student == null) return DateTime.Now;
        return _selected_student.BirthDate;
      }
      set
      {
        if (_edit_mode == true)
        {
          _selected_student.BirthDate = value;
        }
        RaisePropertyChanged("BirthDate");
      }
    }

    public string Note
    {
      get
      {
        if (_selected_student == null) return "";
        return _selected_student.Note;
      }
      set
      {
        if (_edit_mode == true)
        {
          _selected_student.Note = value;
        }
        RaisePropertyChanged("Note");
      }
    }

    #endregion

    #region commands

    private ICommand _new_student_cmd;
    public ICommand NewCmd
    {
      get
      {
        if (_new_student_cmd == null)
        {
          _new_student_cmd = new RelayCommand(
              param => this.new_student(),
              param => this.can_new_student()
              );
        }
        return _new_student_cmd;
      }
    }

    private bool can_new_student()
    {
      if (_selected_student == null) return false;
      if (EditMode == true) return false;
      if (NewMode == true) return false;
      return true;
    }

    private void new_student()
    {
      student_create_new();
    }

    private ICommand _save_cmd;
    public ICommand SaveCmd
    {
      get
      {
        if (_save_cmd == null)
        {
          _save_cmd = new RelayCommand(
              param => this.save(),
              param => this.can_save()
              );
        }
        return _save_cmd;
      }
    }

    private bool can_save()
    {
      if (_selected_student == null) return false;
      if (_selected_student.FirstName == "") return false;
      if (_selected_student.LastName == "") return false;
      if (EditMode == false) return false;
      return true;
    }

    private void save()
    {
      student_save();
    }

    #endregion
  }
}

