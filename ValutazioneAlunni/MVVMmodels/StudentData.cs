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

    public static void DecodeTag(string tag, out int chapter_idx, out int section_idx)
    {
      try
      {
        chapter_idx = -1;
        section_idx = -1;

        string[] tokens = tag.Split('_');
        if (tokens.Length != 2) return;

        chapter_idx = int.Parse(tokens[0].Replace("CHAPTER=", ""));
        section_idx = int.Parse(tokens[1].Replace("SECTION=", ""));
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

    public string UUID;
    public string FirstName;
    public string LastName;
    public DateTime BirthDate = DateTime.Now;
    public string Note;

    public List<StudentEvaluationItem> EvaluationItems;

    #region init and deinit 

    public StudentData()
    {
      UUID = Guid.NewGuid().ToString("N");
    }

    public StudentData(EvaluationScheme evaluation_scheme)
    {
      UUID = Guid.NewGuid().ToString("N");
      load_evaluation_scheme(evaluation_scheme);
    }

    public void CheckEvaluationScheme(EvaluationScheme evaluation_scheme)
    {
      int chapter_idx = 0;
      int section_idx = 0;
      
      if (EvaluationSchemeRelease != evaluation_scheme.Release)
      {
        foreach (EvaluationChapter chapter in evaluation_scheme.Chapters)
        {
          section_idx = 0;
          foreach (EvaluationSection sec in chapter.Sections)
          {
            if (GetEvaluationLevel(chapter_idx, section_idx) <= -2)
            {
              StudentEvaluationItem ei = new StudentEvaluationItem();
              ei.Tag = StudentEvaluationItem.EncodeTag(chapter_idx, section_idx);
              ei.LastChange = DateTime.Now;
              ei.EvalNumber = -1;
              EvaluationItems.Add(ei);
            }
            section_idx++;
          }
          chapter_idx++;
        }
        EvaluationSchemeRelease = evaluation_scheme.Release;
        EvaluationSchemeDate = evaluation_scheme.DatePubblication.ToString("dd/MM/yyyy");
      }
    }

    public int GetEvaluationLevel(int chapter_idx, int section_idx)
    {
      try
      {
        foreach (StudentEvaluationItem ei in EvaluationItems)
        {
          int ch = 0;
          int sec = 0;
          StudentEvaluationItem.DecodeTag(ei.Tag, out ch, out sec);
          if ((chapter_idx == ch) && (section_idx == sec))
          {
            return ei.EvalNumber;
          }
        }
        return -2;
      }
      catch
      {
        return -3;
      }
    }

    #endregion

    #region overrides

    public override string ToString()
    {
      return LastName + " " + FirstName;
    }

    #endregion

    #region public functions

    public StudentData Clone()
    {
      return (StudentData)this.MemberwiseClone();
    }

    public string Dump()
    {
      StringBuilder sb = new StringBuilder();

      sb.AppendLine("UUID     : " + UUID);
      sb.AppendLine("FirstName: " + FirstName);
      sb.AppendLine("LastName : " + LastName);
      sb.AppendLine("BirthDate: " + BirthDate.ToString());
      sb.AppendLine("Note     : " + Note);

      foreach (StudentEvaluationItem ei in EvaluationItems)
      {
        sb.AppendLine(ei.Tag + ": " + ei.EvalNumber + " (" + ei.LastChange.ToString() + ")");
      }

      return sb.ToString();
    }

    #endregion

    #region private functions

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

    #endregion
  }
}
