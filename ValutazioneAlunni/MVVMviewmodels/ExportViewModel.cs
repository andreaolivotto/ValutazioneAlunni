using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ValutazioneAlunni.MVVMmodels;
using ValutazioneAlunni.MVVMutils;
using ValutazioneAlunni.Utilities;
using System.Windows;
using ValutazioneAlunni.Data;

namespace ValutazioneAlunni.MVVMviewmodels
{
  class ExportViewModel : BaseViewModel
  {
    #region private fields

    private static readonly Log _log = Log.Instance;
    private ApplicationSettings _settings = ApplicationSettings.Instance;

    private bool _edit_mode;

    #endregion

    #region init and deinit

    public ExportViewModel()
    {
      EditMode = true;

      messenger_init();
    }

    #endregion

    #region messenger

    private void messenger_init()
    {
      Messenger.Default.Register<StudentData>(this, messenger_export_student_word, "ExportStudentWord");
    }

    private void messenger_export_student_word(StudentData s)
    {
      word_export_student(s);
    }

    #endregion

    #region private functions - word

    private bool word_export_student(StudentData s)
    {
      XWPFParagraph p;
      XWPFRun r;
      int chapter_idx;
      int section_idx;

      try
      {
        _log.Info("Esportazione sudente in Word...");
        _log.Info(s.Dump());
        _log.Info("Cartella di esportazione: " + _settings.ExportFolder);

        if (Directory.Exists(_settings.ExportFolder) == false)
        {
          _log.Error("Errore! La cartella di esportazione non esiste!");
          return false;
        }

        // see https://github.com/tonyqus/npoi/blob/master/examples/xwpf/SimpleDocument/Program.cs

        XWPFDocument doc = new XWPFDocument();

        string font_family = "Courier New";

        // Title
        p = doc.CreateParagraph();
        p.Alignment = ParagraphAlignment.CENTER;
        p.VerticalAlignment = NPOI.XWPF.UserModel.TextAlignment.CENTER;
        p.SpacingAfter = 500;
        r = p.CreateRun();
        r.IsBold = true;
        r.FontSize = 16;
        r.FontFamily = font_family;
        r.SetText(_settings.EvaluationTitle);

        // Header: teacher, data
        p = doc.CreateParagraph();
        p.Alignment = ParagraphAlignment.LEFT;
        r = p.CreateRun();
        r.FontSize = 12;
        r.FontFamily = font_family;
        r.SetText("Insegnante: " + _settings.TeacherFirstName + " " + _settings.TeacherLastName);
        p = doc.CreateParagraph();
        p.Alignment = ParagraphAlignment.LEFT;
        p.SpacingAfter = 500;
        r = p.CreateRun();
        r.FontSize = 12;
        r.FontFamily = font_family;
        r.SetText("Data: " + DateStringName);

        // Student
        p = doc.CreateParagraph();
        p.SpacingAfter = 500;
        p.Alignment = ParagraphAlignment.LEFT;
        r = p.CreateRun();
        r.FontSize = 12;
        r.FontFamily = font_family;
        r.SetText("Studente: " + s.FirstName + " " + s.LastName);

        // Evaluation
        chapter_idx = 0;
        foreach (EvaluationChapter chapter in DataContainer.Instance.EvaluationScheme.Chapters)
        {
          // Student
          p = doc.CreateParagraph();
          p.Alignment = ParagraphAlignment.LEFT;
          p.SpacingAfter = 300;
          r = p.CreateRun();
          r.FontSize = 16;
          r.IsBold = true;
          r.FontFamily = font_family;
          r.SetText(chapter.Name);

          section_idx = 0;
          StringBuilder sb = new StringBuilder();
          foreach (EvaluationSection section in chapter.Sections)
          {
            int level = s.GetEvaluationLevel(chapter_idx, section_idx);
            if (level >= 0)
            {
              sb.Append(DataContainer.Instance.EvaluationScheme.GetLevelDescription(chapter_idx, section_idx, level) + " ");
            }
            section_idx++;
          }
          p = doc.CreateParagraph();
          p.Alignment = ParagraphAlignment.LEFT;
          p.SpacingAfter = 300;
          r = p.CreateRun();
          r.FontSize = 12;
          r.FontFamily = font_family;
          r.SetText(sb.ToString());
          chapter_idx++;
        }


        // Save to disk
        string word_file_name = s.LastName + "_" + s.FirstName + "_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm") + ".docx";
        _log.Info("Nome file               : " + word_file_name);
        string complete_word_file_name = Path.Combine(_settings.ExportFolder, word_file_name);
        FileStream out1 = new FileStream(complete_word_file_name, FileMode.Create);
        doc.Write(out1);
        out1.Close();

        MessageBoxResult result = MessageBox.Show("Valutazione studente esportata in formato Word!", "Esportazione riuscita!", MessageBoxButton.OK);

        return true;
      }
      catch (Exception exc)
      {
        _log.Error("Exception in export_student_word(): " + exc.Message);
        return false;
      }
    }

    #endregion

    #region public properties

    public bool EditMode
    {
      get
      {
        return _edit_mode;
      }
      private set
      {
        _edit_mode = value;
        RaisePropertyChanged("ReadOnlyMode");
        RaisePropertyChanged("EditMode");
      }
    }

    public bool ReadOnlyMode
    {
      get
      {
        return !EditMode;
      }
    }

    public string TeacherFirstName
    {
      get
      {
        return _settings.TeacherFirstName;
      }
      set
      {
        if (EditMode == true)
        {
          _settings.TeacherFirstName = value;
        }
        RaisePropertyChanged("TeacherFirstName");
      }
    }

    public string TeacherLastName
    {
      get
      {
        return _settings.TeacherLastName;
      }
      set
      {
        if (EditMode == true)
        {
          _settings.TeacherLastName = value;
        }
        RaisePropertyChanged("TeacherLastName");
      }
    }

    public string EvaluationTitle
    {
      get
      {
        return _settings.EvaluationTitle;
      }
      set
      {
        if (EditMode == true)
        {
          _settings.EvaluationTitle = value;
        }
        RaisePropertyChanged("EvaluationTitle");
      }
    }

    private string _date = DateTime.Now.ToString("dd/MM/yyyy");
    public string DateStringName
    {
      get
      {
        return _date;
      }
      set
      {
        if (EditMode == true)
        {
          _date = value;
        }
        RaisePropertyChanged("DateStringName");
      }
    }

    public string ExportFolder
    {
      get
      {
        return _settings.ExportFolder;
      }
      private set
      { 
        _settings.ExportFolder = value;
        RaisePropertyChanged("ExportFolder");
      }
    }

    #endregion

    #region commands

    private ICommand _set_export_folder_cmd;
    public ICommand SetExportFolderCmd
    {
      get
      {
        if (_set_export_folder_cmd == null)
        {
          _set_export_folder_cmd = new RelayCommand(
              param => this.set_export_folder(),
              param => this.can_set_export_folder()
              );
        }
        return _set_export_folder_cmd;
      }
    }

    private bool can_set_export_folder()
    {
      return true;
    }

    private void set_export_folder()
    {
      using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
      {
        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
        if (result == System.Windows.Forms.DialogResult.OK)
        {
          ExportFolder = dialog.SelectedPath;
        }
      }
    }

    private ICommand _export_all_word_cmd;
    public ICommand ExportAllWordCmd
    {
      get
      {
        if (_export_all_word_cmd == null)
        {
          _export_all_word_cmd = new RelayCommand(
              param => this.export_all_word(),
              param => this.can_export_all_word()
              );
        }
        return _export_all_word_cmd;
      }
    }

    private bool can_export_all_word()
    {
      return false;
    }

    private void export_all_word()
    {
      // TODO
    }

    #endregion
  }
}
