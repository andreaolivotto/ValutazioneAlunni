using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValutazioneAlunni.MVVMmodels
{
  [Serializable]
  public class StudentEvaluationItem
  {
    public int EvalNumber;
    public DateTime LastChange;
  }

  [Serializable]
  class StudentData
  {
    public string EvaluationSchemeRelease;
    public string EvaluationSchemeDate;

    public string FirstName;
    public string LastName;
    public DateTime BirthDate;
    public string Note;

    public List<StudentEvaluationItem> EvaluationItems;

    public override string ToString()
    {
      return LastName + " " + FirstName;
    }
  }
}
