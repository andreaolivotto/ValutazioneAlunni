﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ValutazioneAlunni.MVVMmodels;
using ValutazioneAlunni.Utilities;

namespace ValutazioneAlunni.Data
{
  class DataContainer
  {
    #region private variables

    private static Log _log = Log.Instance;
    private ApplicationSettings _settings = ApplicationSettings.Instance;

    // http://csharpindepth.com/Articles/General/Singleton.aspx
    private static DataContainer instance = null;
    private static readonly object padlock = new object();

    #endregion

    #region public variables

    public EvaluationScheme EvaluationScheme = null;
    public ObservableCollection<StudentData> Students = null;

    #endregion

    #region init and deinit

    DataContainer()
    {
    }

    ~DataContainer()
    {
    }

    #endregion

    #region public properties

    public static DataContainer Instance
    {
      get
      {
        lock (padlock)
        {
          if (instance == null)
          {
            instance = new DataContainer();
          }
          return instance;
        }
      }
    }

    #endregion

    #region public functions

    public bool LoadData()
    {
      evaluation_scheme_load_from_file();

      Students = new ObservableCollection<StudentData>();

      return students_load_from_file();
    }

    public void LoadFakeData()
    {
      evaluation_scheme_load_fake_data();
      students_load_fake_data();
    }

    public bool SaveStudent(StudentData s)
    {
      return students_save_to_file(s);
    }

    #endregion

    #region private functions - evaluation scheme

    private string evaluation_scheme_get_file_name()
    {
      return "RubricaValutativa_rev0.3.xml";
    }

    private bool evaluation_scheme_load_from_file()
    {
      try
      {
        _log.Info("Caricamento rubrica valutativa...");

        string complete_file_name = Path.Combine(_settings.WorkingFolder, evaluation_scheme_get_file_name());
        _log.Info("File: " + complete_file_name);

        XmlSerializer serializer = new XmlSerializer(typeof(EvaluationScheme));
        FileStream file_stream = new FileStream(complete_file_name, FileMode.Open);

        EvaluationScheme = (EvaluationScheme)serializer.Deserialize(file_stream);

        _log.Info("Release           : " + EvaluationScheme.Release);
        _log.Info("Data pubblicazione: " + EvaluationScheme.DatePubblication);
        _log.Info("Note              : " + EvaluationScheme.Notes);
        _log.Info("Capitoli          : " + EvaluationScheme.Chapters.Count);

        _log.Info("...Ok!");
        return true;
      }
      catch (Exception exc)
      {
        _log.Error("Exception in data_evaluation_scheme_load_from_file(): " + exc.Message);
        return false;
      }
    }

    private bool evaluation_scheme_save_to_file()
    {
      try
      {
        _log.Info("Salvataggio rubrica valutativa...");
        if (EvaluationScheme == null)
        {
          _log.Error("Errore, rubrica valutativa vuota!");
          return false;
        }

        string complete_file_name = Path.Combine(_settings.WorkingFolder, evaluation_scheme_get_file_name());
        _log.Info("File: " + complete_file_name);

        XmlSerializer serializer = new XmlSerializer(typeof(EvaluationScheme));
        StreamWriter file_writer = new StreamWriter(complete_file_name);
        serializer.Serialize(file_writer, EvaluationScheme);
        file_writer.Close();
        _log.Info("...Ok!");
        return true;
      }
      catch (Exception exc)
      {
        _log.Error("Exception in evaluation_scheme_save_to_file(): " + exc.Message);
        return false;
      }
    }

    private void evaluation_scheme_load_fake_data()
    {
      EvaluationChapter chapter;
      EvaluationSection sec;
      EvaluationLevel it;

      _log.Info("Rubrica valutativa, caricamento dati fittizi");

      EvaluationScheme scheme = new EvaluationScheme();

      scheme.Release = "1.0";
      scheme.DatePubblication = new DateTime(2018, 11, 17);
      scheme.Notes = "Fake data";
      scheme.Chapters = new List<EvaluationChapter>();

      chapter = new EvaluationChapter();
      chapter.Name = "Il sé e l’altro";
      chapter.Description = "Frequentando la scuola, il bambino ha l’opportunità di ampliare il mondo delle proprie relazioni e può così sviluppare meglio le sue “capacità non soltanto di stare genericamente con gli altri, ma anche di comprendere, condividere, aiutare e cooperare”. Le diverse esperienze che riguardano “il sé e l’altro” si intrecciano con le attività degli altri campi e tendono allo sviluppo affettivo ed emotivo, sociale, etico morale e di un corretto atteggiamento nei confronti della religiosità e delle religioni.";
      chapter.Sections = new List<EvaluationSection>();
      sec = new EvaluationSection();
      sec.Name = "Sviluppo dell’identità e dell’autonomia personale";
      sec.Description = "Il bambino e se stesso: riconosce, ed esprime i propri bisogni, esigenze, sentimenti, capacità.";
      sec.Levels = new List<EvaluationLevel>();
      it = new EvaluationLevel();
      it.Level = 1;
      it.Description = "Su invito dell’insegnante che lo sostiene emotivamente, esprime i propri bisogni primari, esigenze. Con l’aiuto dell’insegnante osserva le routine della giornata. Stimolato, scopre le proprie capacità ed inizia a maturare un positivo senso di sé.";
      sec.Levels.Add(it);
      it = new EvaluationLevel();
      it.Level = 2;
      it.Description = "Esprime i propri bisogni primari e le principali emozioni; racconta i propri vissuti con domande stimolo dell’insegnante. Inizia a scoprire le proprie capacità e ad avere fiducia in sé.";
      sec.Levels.Add(it);
      it = new EvaluationLevel();
      it.Level = 3;
      it.Description = "Esprime in modo consapevole i propri bisogni/esigenze che riconosce su di sé e sugli altri. Manifesta i propri stati d’animo / emozioni con i diversi linguaggi ed inizia a gestirli ed a controllarli. Ha fiducia nelle proprie capacità.";
      sec.Levels.Add(it);
      it = new EvaluationLevel();
      it.Level = 4;
      it.Description = "Esprime in modo autonomo, pertinente e responsabile le proprie e altrui esigenze formulando ipotesi e trovando soluzioni adeguate. Ha una buona immagine di sé: ha sicurezza e fiducia nelle proprie capacità.";
      sec.Levels.Add(it);
      chapter.Sections.Add(sec);

      sec = new EvaluationSection();
      sec.Name = "Socializzazione";
      sec.Description = "Il bambino e gli altri: accetta gli altri, coetanei e adulti e partecipa all'attività di gruppo.";
      sec.Levels = new List<EvaluationLevel>();
      it = new EvaluationLevel();
      it.Level = 1;
      it.Description = "E’ interessato ai coetanei, agli adulti e all’ambiente circostante; gioca vicino ai compagni e con l’aiuto dell’insegnante partecipa alle attività collettive mantenendo l’attenzione per brevi periodi.";
      sec.Levels.Add(it);
      it = new EvaluationLevel();
      it.Level = 2;
      it.Description = "Condivide semplici esperienze con i pari: gioca con i compagni scambiando informazioni, si relaziona positivamente con il piccolo gruppo. Nelle attività collettive inizia a manifestare interesse apportando il proprio contributo.";
      sec.Levels.Add(it);
      it = new EvaluationLevel();
      it.Level = 3;
      it.Description = "Accetta i compagni ed è disponibile ad aiutare chi è in difficoltà, riconosce i diversi ruoli. Partecipa attivamente alle esperienze del grande gruppo collaborando in modo proficuo.";
      sec.Levels.Add(it);
      it = new EvaluationLevel();
      it.Level = 4;
      it.Description = "Accetta i pari e gli adulti che rispetta e con i quali interagisce positivamente, giocando e lavorando in modo costruttivo e creativo. Si accorge delle difficoltà dei compagni che aiuta spontanea - mente; dell’adulto tiene in considerazione suggerimenti e consigli. Affronta con interesse e curiosità le varie attività proposte: pone domande, formula ipotesi.";
      sec.Levels.Add(it);
      chapter.Sections.Add(sec);

      scheme.Chapters.Add(chapter);

      chapter = new EvaluationChapter();
      chapter.Name = "Il corpo e il movimento";
      chapter.Description = "Il corpo è il primo medium dell’apprendimento. Attraverso il corpo il bambino impara a prendere consapevolezza della realtà che lo circonda nello spazio, a relazionarsi con la realtà esterna, a esprimere i suoi sentimenti (gioia, tristezza, paura, dolore , affetto...). In breve: a conoscere e a comunicare.";
      chapter.Sections = new List<EvaluationSection>();
      sec = new EvaluationSection();

      sec = new EvaluationSection();
      sec.Name = "Cura di sé";
      sec.Description = "Il bambino ha cura del proprio corpo nei comportamenti di igiene, nel vestirsi e nell’alimentazione.";
      sec.Levels = new List<EvaluationLevel>();
      it = new EvaluationLevel();
      it.Level = 1;
      it.Description = "Con l’aiuto dell’adulto o di un compagno tutor, inizia a conoscere la routine scolastica: accede ai servizi e talvolta si tiene pulito, si lava le mani; si nutre, si lascia vestire e svestire.";
      sec.Levels.Add(it);
      it = new EvaluationLevel();
      it.Level = 2;
      it.Description = "Sollecitato verbalmente dell’adulto che lo invita a tenersi pulito, a mangiare servendosi delle posate, talvolta riesce ad effettuare correttamente la routine scolastica. Prova a vestirsi/svestirsi. Comincia a chiedere aiuto all’adulto nei momenti di difficoltà.";
      sec.Levels.Add(it);
      it = new EvaluationLevel();
      it.Level = 3;
      it.Description = "In autonomia si tiene pulito e conosce ed osserva le pratiche igieniche di routine; sa vestirsi e svestirsi; mangia correttamente e sa stare seduto composto. Esprime le proprie preferenze alimentari e accetta di assaggiare alimenti nuovi.";
      sec.Levels.Add(it);
      it = new EvaluationLevel();
      it.Level = 4;
      it.Description = "Ha interiorizzato e consolidato la routine scolastica; ha cura del proprio corpo. Sa vestirsi e svestirsi manifestando la capacità di autoregolazione termica, abbottonare e sbottonare, allacciare e slacciare. Mangia correttamente e sa stare seduto composto, aiuta gli altri e sa dar loro corrette indicazioni di comportamento.";
      sec.Levels.Add(it);
      chapter.Sections.Add(sec);

      scheme.Chapters.Add(chapter);

      EvaluationScheme = scheme;

      //chapter = new EvaluationChapter();
      //chapter.Name = "";
      //chapter.Description = "";
      //chapter.Sections = new List<EvaluationSection>();

      //sec = new EvaluationSection();
      //sec.Name = "";
      //sec.Description = "";
      //sec.Items = new List<EvaluationItem>();

      //it = new EvaluationItem();
      //it.Level = 1;
      //it.Description = "";
      //sec.Items.Add(it);
      //it = new EvaluationItem();
      //it.Level = 2;
      //it.Description = "";
      //sec.Items.Add(it);
      //it = new EvaluationItem();
      //it.Level = 3;
      //it.Description = "";
      //sec.Items.Add(it);
      //it = new EvaluationItem();
      //it.Level = 4;
      //it.Description = "";
      //sec.Items.Add(it);
      //chapter.Sections.Add(sec);
    }

    #endregion

    #region private functions - student data

    private void students_load_fake_data()
    {
      StudentData sd;
      //StudentEvaluationItem ei;

      _log.Info("Anagrafica studenti, caricamento dati fittizi");

      ObservableCollection<StudentData> list_of_students = new ObservableCollection<StudentData>();

      sd = new StudentData(DataContainer.Instance.EvaluationScheme);
      sd.EvaluationSchemeDate = "FAKEDATA";
      sd.EvaluationSchemeRelease = "0.1";
      sd.FirstName = "Mario";
      sd.LastName = "Rossi";
      sd.BirthDate = new DateTime(2015, 2, 23);
      //sd.EvaluationItems = new List<StudentEvaluationItem>();
      //ei = new StudentEvaluationItem();
      //ei.Tag = StudentEvaluationItem.EncodeTag(0, 0);
      //ei.EvalNumber = 1;
      //ei.LastChange = DateTime.Now;
      //sd.EvaluationItems.Add(ei);
      //ei = new StudentEvaluationItem();
      //ei.Tag = StudentEvaluationItem.EncodeTag(0, 1);
      //ei.EvalNumber = 2;
      //ei.LastChange = DateTime.Now;
      //sd.EvaluationItems.Add(ei);
      list_of_students.Add(sd);

      sd = new StudentData(DataContainer.Instance.EvaluationScheme);
      sd.EvaluationSchemeDate = "FAKEDATA";
      sd.EvaluationSchemeRelease = "0.1";
      sd.FirstName = "Gianni";
      sd.LastName = "Bianchi";
      sd.BirthDate = new DateTime(2016, 6, 5);
      sd.Note = "Potrebbe fare meglio.";
      //sd.EvaluationItems = new List<StudentEvaluationItem>();
      //ei = new StudentEvaluationItem();
      //ei.Tag = StudentEvaluationItem.EncodeTag(0, 0);
      //ei.EvalNumber = 3;
      //ei.LastChange = DateTime.Now;
      //sd.EvaluationItems.Add(ei);
      //ei = new StudentEvaluationItem();
      //ei.Tag = StudentEvaluationItem.EncodeTag(0, 1);
      //ei.EvalNumber = 4;
      //ei.LastChange = DateTime.Now;
      //sd.EvaluationItems.Add(ei);
      list_of_students.Add(sd);

      sd = new StudentData(DataContainer.Instance.EvaluationScheme);
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

    private bool is_student_file(string complete_file_name)
    {
      string file_name = Path.GetFileName(complete_file_name);

      if (file_name.StartsWith("student_") == false)
      {
        return false;
      }
      if (file_name.EndsWith(".xml") == false)
      {
        return false;
      }
      return true;
    }

    private string get_student_file_name(StudentData s)
    {
      return "student_" + s.UUID + ".xml";
    }

    private bool students_load_from_file()
    {
      try
      {
        _log.Info("Caricamento dati studenti...");

        _log.Info("Cartella di lavoro: " + _settings.WorkingFolder);

        string[] files = Directory.GetFiles(_settings.WorkingFolder);

        XmlSerializer serializer = new XmlSerializer(typeof(StudentData));
        foreach (string f in files)
        {
          if (is_student_file(f))
          {
            // Load student
            FileStream file_stream = new FileStream(f, FileMode.Open);
            StudentData s = (StudentData)serializer.Deserialize(file_stream);
            _log.Info("Caricato studente <" + s.ToString() + ">");
            s.CheckEvaluationScheme(EvaluationScheme);
            Students.Add(s);
          }
        }

        _log.Info("...Ok!");
        return true;
      }
      catch (Exception exc)
      {
        _log.Error("Exception in students_load_from_file(): " + exc.Message);
        return false;
      }
    }

    private bool students_save_to_file(StudentData s)
    {
      try
      {
        if (s == null) return false;

        _log.Info("Salvataggio studente <" + s.ToString() + ">...");
        _log.Info("Cartella di lavoro: " + _settings.WorkingFolder);

        string complete_file_name = Path.Combine(_settings.WorkingFolder, get_student_file_name(s));
        _log.Info("File: " + complete_file_name);
        if (File.Exists(complete_file_name))
        {
          File.Delete(complete_file_name);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(StudentData));
        StreamWriter file_writer = new StreamWriter(complete_file_name);
        serializer.Serialize(file_writer, s);
        file_writer.Close();
        _log.Info("...Ok!");
        return true;
      }
      catch (Exception exc)
      {
        _log.Error("Exception in evaluation_scheme_save_to_file(): " + exc.Message);
        return false;
      }
    }

    #endregion
  }
}
