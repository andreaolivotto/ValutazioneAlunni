using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValutazioneAlunni.MVVMmodels;
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
      sd.Surname = "Rossi";
      sd.BirthDate = new DateTime(2015, 2, 23);
      sd.EvaluationItems = new List<StudentEvaluationItem>();
      list_of_students.Add(sd);

      sd = new StudentData();
      sd.EvaluationSchemeDate = "FAKEDATA";
      sd.EvaluationSchemeRelease = "0.1";
      sd.FirstName = "Gianni";
      sd.Surname = "Bianchi";
      sd.BirthDate = new DateTime(2016, 6, 5);
      sd.EvaluationItems = new List<StudentEvaluationItem>();
      list_of_students.Add(sd);

      sd = new StudentData();
      sd.EvaluationSchemeDate = "FAKEDATA";
      sd.EvaluationSchemeRelease = "0.1";
      sd.FirstName = "Marco";
      sd.Surname = "Verdi";
      sd.BirthDate = new DateTime(2017, 11, 29);
      sd.EvaluationItems = new List<StudentEvaluationItem>();
      list_of_students.Add(sd);

      Students = list_of_students;
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

    #endregion
  }
}
