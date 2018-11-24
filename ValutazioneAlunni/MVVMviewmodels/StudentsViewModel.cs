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
    private StudentData _edit_student;
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

    private void messenger_send_set_student(StudentData s)
    {
      Messenger.Default.Send(s, "SetCurrentStudent");
    }

    private void messenger_send_edit_mode(bool edit_mode)
    {
      Messenger.Default.Send(edit_mode, "SetEditMode");
    }

    private void messenger_send_export_student_word(StudentData s)
    {
      Messenger.Default.Send(s, "ExportStudentWord");
    }

    #endregion

    #region private functions

    private void students_init()
    {
      Students = DataContainer.Instance.Students;

      exit_edit_mode(false);
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
        _log.Info(sd.Dump());
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
      messenger_send_set_student(_selected_student);
      messenger_send_edit_mode(_edit_mode);
    }

    private void student_save_new_or_edit()
    {
      try
      {
        if (_edit_student == null) return;

        _log.Info("Salva dati studente <" + _edit_student + ">");
        _log.Info("NewMode : " + NewMode);
        _log.Info("EditMode: " + EditMode);
        _log.Info(_edit_student.Dump());

        if (NewMode)
        {
          DataContainer.Instance.Students.Add(_edit_student);
          _last_selected_student = _edit_student;
          exit_new_mode();
        }
        else
        {
          if (EditMode)
          {
            exit_edit_mode(true);
          }
        }

        // Save to file
        DataContainer.Instance.SaveStudent(SelectedStudent);
      }
      catch (Exception exc)
      {
        _log.Error("Exception in student_save(): " + exc.Message);
      }
    }

    private void student_create_new()
    {
      enter_new_mode();

      _log.Info("");
      _log.Info("******************** Crea nuovo studente");
    }

    private void enter_new_mode()
    {
      EditMode = true;
      NewMode = true;
      _edit_student = new StudentData(DataContainer.Instance.EvaluationScheme);
      messenger_send_set_student(_edit_student);
      //_last_selected_student = SelectedStudent;
      SelectedStudent = _edit_student;
    }

    private void exit_new_mode()
    {
      EditMode = false;
      NewMode = false;
      _edit_student = null;

      if (_last_selected_student == null)
      {
        students_select_first();
      }
      else
      {
        SelectedStudent = _last_selected_student;
      }
      messenger_send_set_student(SelectedStudent);
    }

    private void student_edit()
    {
      enter_edit_mode();

      _log.Info("");
      _log.Info("******************** Modifica dati studente <" + _edit_student + ">");
      _log.Info("NewMode : " + NewMode);
      _log.Info("EditMode: " + EditMode);
      _log.Info(_edit_student.ToString());
    }

    private void enter_edit_mode()
    {
      EditMode = true;
      NewMode = false;
      _edit_student = SelectedStudent.Clone();
      messenger_send_set_student(_edit_student);
      // Keep a pointer to the current item on list
      _last_selected_student = SelectedStudent;
      // Set the selected item to the edit copy
      SelectedStudent = _edit_student;
    }

    private void exit_edit_mode(bool save)
    {
      EditMode = false;
      NewMode = false;

      if (save)
      {
        // Save new student data!
        int index = Students.IndexOf(_last_selected_student);
        Students.Remove(_last_selected_student);
        Students.Insert(index, _edit_student);
        SelectedStudent = _edit_student;
        _last_selected_student = SelectedStudent;
      }

      messenger_send_set_student(SelectedStudent);
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
        RaisePropertyChanged("NewMode");
        RaisePropertyChanged("NotNewMode");
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
      private set
      {
        _edit_mode = value;
        messenger_send_edit_mode(_edit_mode);
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
        if (EditMode == true)
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
        if (EditMode == true)
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
        if (EditMode == true)
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
        if (EditMode == true)
        {
          _selected_student.Note = value;
        }
        RaisePropertyChanged("Note");
      }
    }

    #endregion

    #region commands

    private ICommand _edit_student_cmd;
    public ICommand EditCmd
    {
      get
      {
        if (_edit_student_cmd == null)
        {
          _edit_student_cmd = new RelayCommand(
              param => this.edit_student(),
              param => this.can_edit_student()
              );
        }
        return _edit_student_cmd;
      }
    }

    private bool can_edit_student()
    {
      if (_selected_student == null) return false;
      if (EditMode == true) return false;
      if (NewMode == true) return false;
      return true;
    }

    private void edit_student()
    {
      student_edit();
    }

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
      //if (_selected_student == null) return false;
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
      student_save_new_or_edit();
    }

    private ICommand _cancel_cmd;
    public ICommand CancelCmd
    {
      get
      {
        if (_cancel_cmd == null)
        {
          _cancel_cmd = new RelayCommand(
              param => this.cancel(),
              param => this.can_cancel()
              );
        }
        return _cancel_cmd;
      }
    }

    private bool can_cancel()
    {
      if (EditMode == false) return false;
      return true;
    }

    private void cancel()
    {
      if (NewMode) exit_new_mode();
      if (EditMode) exit_edit_mode(false);
    }

    #endregion
  }
}

