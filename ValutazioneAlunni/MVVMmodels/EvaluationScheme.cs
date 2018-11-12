using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValutazioneAlunni.MVVMmodels
{
  [Serializable]
  class EvaluationSection
  {
    public string Name;         // Dimensione
    public string Description;  // Indicatore di competenza
    List<EvaluationItem> Items;
  }

  [Serializable]
  class EvaluationItem
  {
    public string Name;
    public string Description;
    public int Level;
    public string LevelDescription;
    public string InternalNote;
  }

  [Serializable]
  class EvaluationScheme
  {
    public string Release;
    public DateTime DatePubblication;
    public string Notes;
    List<EvaluationSection> Sections;
  }
}
