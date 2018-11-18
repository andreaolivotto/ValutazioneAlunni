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
  public class StudentData
  {
    public string EvaluationSchemeRelease;
    public string EvaluationSchemeDate;

    public string FirstName;
    public string LastName;
    public DateTime BirthDate;
    public string Note;

    public List<StudentEvaluationItem> EvaluationItems;

    public StudentData()
    {
    }

    public StudentData(EvaluationScheme evaluation_scheme)
    {
      load_evaluation_scheme(evaluation_scheme);
    }

    public override string ToString()
    {
      return LastName + " " + FirstName;
    }

    private void load_evaluation_scheme(EvaluationScheme evaluation_scheme)
    {
      int chapter_idx;
      int section_idx;

      try
      {
        if (evaluation_scheme == null) return;

        EvaluationSchemeRelease = evaluation_scheme.Release;
        EvaluationSchemeDate = evaluation_scheme.DatePubblication.ToString("dd/MM/yyyy");

        EvaluationItems = new List<StudentEvaluationItem>();
        chapter_idx = 0;
        foreach (EvaluationChapter chapter in evaluation_scheme.Chapters)
        {
          section_idx = 0;
          foreach (EvaluationSection section in chapter.Sections)
          {
            StudentEvaluationItem i = new StudentEvaluationItem();
            i.Tag = StudentEvaluationItem.EncodeTag(chapter_idx, section_idx);
            i.LastChange = DateTime.Now;
            i.EvalNumber = -1;
            EvaluationItems.Add(i);
            section_idx++;
          }
          chapter_idx++;
        }
      }
      catch
      {
      }
    }
  }
}
