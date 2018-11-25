using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ValutazioneAlunni.MVVMmodels
{
  [Serializable]
  public class EvaluationLevel
  {
    public int Level;
    public string Description;

    public static string GetLevelDescription(int level)
    {
      switch (level)
      {
        case 1:
          return "Iniziale";
        case 2:
          return "Base";
        case 3:
          return "Intermedio";
        case 4:
          return "Avanzato";
        default:
          return "ERR";
      }
    }

    public static Color GetLevelColor(int level)
    {
      switch (level)
      {
        case 0:
          return Color.FromRgb(255, 226, 210);
        case 1:
          return Color.FromRgb(255, 235, 182);
        case 2:
          return Color.FromRgb(207, 237, 251);
        case 3:
          return Color.FromRgb(229, 239, 199);
        default:
          return Color.FromRgb(244, 244, 244);
      }
    }

  }

  [Serializable]
  public class EvaluationSection
  {
    public string Name;         // Dimensione
    public string Description;  // Indicatore di competenza
    public List<EvaluationLevel> Levels;

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      sb.AppendLine("Nome       : " + Name);
      sb.AppendLine("Descrizione: " + Description);
      if (Levels == null)
      {
        sb.AppendLine("nessun livello [ERR1]!");
      }
      else
      {
        if (Levels.Count == 0)
        {
          sb.AppendLine("nessuna livello [ERR2]!");
        }
        else
        {
          foreach (EvaluationLevel level in Levels)
          {
            sb.AppendLine("Livello " + level.Level + ": " + level.Description);
          }
        }
      }
      return sb.ToString();
    }
  }

  [Serializable]
  public class EvaluationChapter
  {
    public string Name;         // Dimensione
    public string Description;  // Indicatore di competenza
    public List<EvaluationSection> Sections;

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      sb.AppendLine("Nome       : " + Name);
      sb.AppendLine("Descrizione: " + Description);
      if (Sections == null)
      {
        sb.AppendLine("nessuna sezione [ERR1]!");
      }
      else
      {
        if (Sections.Count == 0)
        {
          sb.AppendLine("nessuna sezione [ERR2]!");
        }
        else
        {
          foreach (EvaluationSection sec in Sections)
          {
            sb.AppendLine(sec.ToString());
          }
        }
      }
      return sb.ToString();
    }
  }

  [Serializable]
  public class EvaluationScheme
  {
    public string Release;
    public DateTime DatePubblication;
    public string Notes;
    public List<EvaluationChapter> Chapters;

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("Release           : " + Release);
      sb.AppendLine("Data pubblicazione: " + DatePubblication);
      sb.AppendLine("Note              : " + Notes);
      sb.AppendLine("");
      sb.AppendLine("Capitoli:");
      if (Chapters == null)
      {
        sb.AppendLine("nessun capitolo [ERR1]!");
      }
      else
      {
        if (Chapters.Count == 0)
        {
          sb.AppendLine("nessun capitolo [ERR2]!");
        }
        else
        {
          foreach (EvaluationChapter ch in Chapters)
          {
            sb.AppendLine(ch.ToString());
          }
        }
      }

      return sb.ToString();
    }

    public string GetLevelDescription(int chapter_idx, int section_idx, int level_value)
    {
      try
      {
        return Chapters[chapter_idx].Sections[section_idx].Levels[level_value].Description;
      }
      catch (Exception exc)
      {
        return "ERROR: " + exc.Message;
      }
    }
  }
}
