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
    public string Tag;
    public int EvalNumber;
    public DateTime LastChange;

    public static string EncodeTag(int chapter_idx, int section_idx)
    {
      return "CHAPTER=" + chapter_idx.ToString() + "_" + "SECTION=" + section_idx.ToString();
    }

    public static void DecodeTag(string tag, ref int chapter_idx, ref int section_idx)
    {
      try
      {
        chapter_idx = -1;
        section_idx = -1;

        string[] tokens = tag.Split('_');
        if (tokens.Length != 2) return;

        chapter_idx = int.Parse(tokens[0].Replace("CHAPTER=", ""));
        section_idx = int.Parse(tokens[0].Replace("SECTION=", ""));
      }
      catch
      {
        chapter_idx = -1;
        section_idx = -1;
      }
    }
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
