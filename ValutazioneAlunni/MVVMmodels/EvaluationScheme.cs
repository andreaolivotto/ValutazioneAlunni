using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValutazioneAlunni.MVVMmodels
{
  [Serializable]
  class EvaluationLevel
  {
    public int Level;
    public string Description;
  }

  [Serializable]
  class EvaluationSection
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
  class EvaluationChapter
  {
    public string Name;         // Dimensione
    public string Description;  // Indicatore di competenza
    public int Index;
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
  class EvaluationScheme
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
  }
}
