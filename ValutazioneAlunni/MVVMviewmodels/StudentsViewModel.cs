using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ValutazioneAlunni.MVVMmodels;
using ValutazioneAlunni.MVVMutils;
using ValutazioneAlunni.Utilities;

namespace ValutazioneAlunni.MVVMviewmodels
{
  class StudentsViewModel : BaseViewModel
  {
    #region private fields

    private static Log _log = Log.Instance;

    #endregion

    #region init and deinit

    public StudentsViewModel()
    {
      students_init();
    }

    #endregion

    #region private functions

    private void students_init()
    {
      students_load_fake_data();
      students_print_all();
      students_select_first();
    }

    private void students_print_all()
    {
      int idx;

      if (Students == null)
      {
        _log.Info("Students list is null!");
      }
      if (Students.Count == 0)
      {
        _log.Info("Students list is empty!");
      }

      _log.Info("Total students: " + Students.Count);
      idx = 1;
      foreach (StudentData sd in Students)
      {
        _log.Info("[" + idx.ToString("D2") + "] " + sd.ToString());
        idx++;
      }
    }

    private void students_load_fake_data()
    {
      StudentData sd;

      ObservableCollection<StudentData> list_of_students = new ObservableCollection<StudentData>();

      sd = new StudentData();
      sd.EvaluationSchemeDate = "FAKEDATA";
      sd.EvaluationSchemeRelease = "0.1";
      sd.FirstName = "Mario";
      sd.LastName = "Rossi";
      sd.BirthDate = new DateTime(2015, 2, 23);
      sd.EvaluationItems = new List<StudentEvaluationItem>();
      list_of_students.Add(sd);

      sd = new StudentData();
      sd.EvaluationSchemeDate = "FAKEDATA";
      sd.EvaluationSchemeRelease = "0.1";
      sd.FirstName = "Gianni";
      sd.LastName = "Bianchi";
      sd.BirthDate = new DateTime(2016, 6, 5);
      sd.Note = "Potrebbe fare meglio.";
      sd.EvaluationItems = new List<StudentEvaluationItem>();
      list_of_students.Add(sd);

      sd = new StudentData();
      sd.EvaluationSchemeDate = "FAKEDATA";
      sd.EvaluationSchemeRelease = "0.1";
      sd.FirstName = "Marco";
      sd.LastName = "Verdi";
      sd.BirthDate = new DateTime(2017, 11, 29);
      sd.Note = "Bravo studente.\nMa non si applica.";
      sd.EvaluationItems = new List<StudentEvaluationItem>();
      list_of_students.Add(sd);

      Students = list_of_students;
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

    #endregion

    #region public properties

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
        }
        studens_refresh_detail_data();
        RaisePropertyChanged("SelectedStudent");
      }
    }

    private bool _edit_mode;
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

    private ICommand _save_cmd;
    public ICommand SaveCmd
    {
      get
      {
        if (_save_cmd == null)
        {
          _save_cmd = new RelayCommand(
              param => this.SaveObject(),
              param => this.CanSave()
              );
        }
        return _save_cmd;
      }
    }

    private bool CanSave()
    {
      return true;
    }

    private void SaveObject()
    {
      // TODO
      _log.Info("Salva dati studente");
    }

    #endregion
  }
}

