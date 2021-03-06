﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ValutazioneAlunni.Data;
using ValutazioneAlunni.MVVMmodels;
using ValutazioneAlunni.MVVMutils;
using ValutazioneAlunni.Utilities;

namespace ValutazioneAlunni.MVVMviews
{
  /// <summary>
  /// Interaction logic for StudentsView.xaml
  /// </summary>
  public partial class StudentsView : UserControl
  {
    #region private fields

    private static Log _log = Log.Instance;

    private StudentData _student;
    private EvaluationScheme _evaluation_scheme;
    private bool _loading_evaluation_scheme;
    private bool _load_student;

    #endregion

    #region init and deinit 

    public StudentsView()
    {
      InitializeComponent();

      messenger_init();

      _load_student = false;
    }

    #endregion

    #region messenger

    private void messenger_init()
    {
      Messenger.Default.Register<EvaluationScheme>(this, messenger_set_evaluation_scheme, "SetEvaluationScheme");
      Messenger.Default.Register<StudentData>(this, messenger_set_student, "SetCurrentStudent");
      Messenger.Default.Register<bool>(this, messenger_set_edit_mode, "SetEditMode");
    }

    private void messenger_deinit()
    {
      Messenger.Default.Unregister(this, "SetEvaluationScheme");
      Messenger.Default.Unregister(this, "SetCurrentStudent");
      Messenger.Default.Unregister(this, "SetEditMode");
    }

    private void messenger_set_evaluation_scheme(EvaluationScheme scheme)
    {
      _evaluation_scheme = scheme;
      on_set_evaluation_scheme();
    }

    private void messenger_set_student(StudentData student)
    {
      _student = student;
      on_set_student();
    }

    private void messenger_set_edit_mode(bool edit_mode)
    {
      if (_student == null) return;
      if (_student.EvaluationItems == null) return;

      foreach (StudentEvaluationItem ei in _student.EvaluationItems)
      {
        int idx;
        for (idx = 0; idx < grdEvaluationScheme.Children.Count; idx++)
        {
          UIElement ui_element = grdEvaluationScheme.Children[idx];
          if (ui_element is ComboBox)
          {
            ComboBox cmb = (ComboBox)ui_element;
            cmb.IsEnabled = edit_mode;
          }
        }
      }
    }

    #endregion

    #region provate functions

    private void on_set_evaluation_scheme()
    {
      int chapter_idx = 0;
      int section_idx = 0;
      int grid_row = 0;
      RowDefinition rd;
      TextBox tb;
      ComboBox cmb;
      int idx;

      // Load evaluation scheme info on UI

      _loading_evaluation_scheme = true;

      grdEvaluationScheme.Children.Clear();
      grdEvaluationScheme.RowDefinitions.Clear();

      if (_evaluation_scheme == null) return;

      foreach (EvaluationChapter chapter in _evaluation_scheme.Chapters)
      {
        // Chapter title
        rd = new RowDefinition();
        rd.Height = GridLength.Auto;
        grdEvaluationScheme.RowDefinitions.Add(rd);
        tb = new TextBox();
        tb.Text = (chapter_idx + 1).ToString() + ". " + chapter.Name;
        tb.TextWrapping = TextWrapping.Wrap;
        tb.IsReadOnly = true;
        tb.FontSize = 22;
        tb.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
        tb.BorderThickness = new Thickness(0,0,0,1);
        //tb.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        tb.Padding = new Thickness(0, 2, 0, 2);
        tb.Margin = new Thickness(3, 40, 3, 3);
        grdEvaluationScheme.Children.Add(tb);
        Grid.SetRow(tb, grid_row);
        grid_row++;

        // Chapter description
        rd = new RowDefinition();
        rd.Height = GridLength.Auto;
        grdEvaluationScheme.RowDefinitions.Add(rd);
        tb = new TextBox();
        tb.Text = chapter.Description;
        tb.TextWrapping = TextWrapping.Wrap;
        tb.IsReadOnly = true;
        tb.FontSize = 12;
        tb.BorderThickness = new Thickness(0);
        grdEvaluationScheme.Children.Add(tb);
        Grid.SetRow(tb, grid_row);
        grid_row++;

        // Sections
        section_idx = 0;
        foreach (EvaluationSection sec in chapter.Sections)
        {
          // Section title (name)
          rd = new RowDefinition();
          rd.Height = GridLength.Auto;
          grdEvaluationScheme.RowDefinitions.Add(rd);
          tb = new TextBox();
          tb.Text = (chapter_idx + 1).ToString() + "." + (section_idx + 1).ToString() + ". "  + sec.Name;
          tb.TextWrapping = TextWrapping.Wrap;
          tb.IsReadOnly = true;
          tb.FontSize = 14;
          tb.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
          tb.BorderThickness = new Thickness(0);
          tb.Margin = new Thickness(3, 20, 3, 3);
          grdEvaluationScheme.Children.Add(tb);
          Grid.SetRow(tb, grid_row);
          grid_row++;

          // Section description
          rd = new RowDefinition();
          rd.Height = GridLength.Auto;
          grdEvaluationScheme.RowDefinitions.Add(rd);
          tb = new TextBox();
          tb.Text = sec.Description;
          tb.TextWrapping = TextWrapping.Wrap;
          tb.IsReadOnly = true;
          tb.FontSize = 12;
          tb.BorderThickness = new Thickness(0);
          grdEvaluationScheme.Children.Add(tb);
          Grid.SetRow(tb, grid_row);
          grid_row++;

          // Level
          rd = new RowDefinition();
          rd.Height = GridLength.Auto;
          grdEvaluationScheme.RowDefinitions.Add(rd);
          cmb = new ComboBox();
          cmb.Tag = StudentEvaluationItem.EncodeTag(chapter_idx, section_idx);
          _log.Info("Create combobox tag " + cmb.Tag.ToString());
          cmb.Width = 160;
          cmb.Margin = new Thickness(3, 3, 3, 3);
          cmb.SelectionChanged += Cmb_SelectionChanged;
          cmb.HorizontalAlignment = HorizontalAlignment.Left;
          for (idx=0; idx<sec.Levels.Count; idx++)
          {
            ComboBoxItem i = new ComboBoxItem();
            i.Tag = sec.Levels[idx].Level;
            i.Content = "Livello " + sec.Levels[idx].Level.ToString() + " " + EvaluationLevel.GetLevelDescription(sec.Levels[idx].Level);
            cmb.Items.Add(i);
          }
          grdEvaluationScheme.Children.Add(cmb);
          Grid.SetRow(cmb, grid_row);
          grid_row++;

          // Level description
          rd = new RowDefinition();
          rd.Height = GridLength.Auto;
          grdEvaluationScheme.RowDefinitions.Add(rd);
          tb = new TextBox();
          tb.Text = "";
          tb.Tag = StudentEvaluationItem.EncodeTag(chapter_idx, section_idx);
          _log.Info("Create textbox tag " + tb.Tag.ToString());
          tb.TextWrapping = TextWrapping.Wrap;
          tb.IsReadOnly = true;
          tb.FontStyle = FontStyles.Italic;
          tb.FontSize = 12;
          tb.BorderThickness = new Thickness(0);
          grdEvaluationScheme.Children.Add(tb);
          Grid.SetRow(tb, grid_row);
          grid_row++;

          section_idx++;
        }

        chapter_idx++;
      }

      rd = new RowDefinition();
      grdEvaluationScheme.RowDefinitions.Add(rd);

      _loading_evaluation_scheme = false;
    }

    private void Cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (_loading_evaluation_scheme) return;

      update_current_student((ComboBox)sender);
    }

    private void on_set_student()
    {
      if (_evaluation_scheme == null) return;
      if (_student == null) return;

      // Load student evaluation levels on UI
      _load_student = true;

      if (_student.EvaluationItems == null) return;
      if (_student.EvaluationItems.Count == 0) return;

      foreach (StudentEvaluationItem ei in _student.EvaluationItems)
      {
        int idx;
        for (idx=0; idx< grdEvaluationScheme.Children.Count; idx++)
        {
          UIElement ui_element = grdEvaluationScheme.Children[idx];
          if (ui_element is ComboBox)
          {
            ComboBox cmb = (ComboBox)ui_element;
            if (cmb.Tag.ToString() == ei.Tag)
            {
              cmb.SelectedIndex = ei.EvalNumber;
              update_level_description(ei.Tag, ei.EvalNumber);
            }
          }
        }
      }

      _load_student = false;
    }

    private void update_level_description(string tag, int level)
    {
      int idx;

      for (idx = 0; idx < grdEvaluationScheme.Children.Count; idx++)
      {
        UIElement ui_element = grdEvaluationScheme.Children[idx];
        if (ui_element is TextBox)
        {
          TextBox txt = (TextBox)ui_element;
          if (txt.Tag != null)
          {
            _log.Info("  textbox " + txt.Tag.ToString());
            if (txt.Tag.ToString() == tag)
            {
              _log.Info("Found textbox " + txt.Tag.ToString());
              if (level < 0)
              {
                txt.Text = "";
                txt.Background = new SolidColorBrush(Color.FromArgb(0,0,0,0));
              }
              else
              {
                int chapter_idx = 0;
                int section_idx = 0;
                StudentEvaluationItem.DecodeTag(tag, out chapter_idx, out section_idx);
                string level_description = DataContainer.Instance.EvaluationScheme.GetLevelDescription(chapter_idx, section_idx, level);
                txt.Text = level_description;
                txt.Background = new SolidColorBrush(EvaluationLevel.GetLevelColor(level));
              }
              return;
            }
          }
        }
      }
    }

    private void update_current_student(ComboBox cmb)
    {
      if (_evaluation_scheme == null) return;
      if (_student == null) return;

      if (_load_student) return;
      foreach (StudentEvaluationItem ei in _student.EvaluationItems)
      {
        if (cmb.Tag.ToString() == ei.Tag)
        {
          _log.Info("Found combobox " + cmb.Tag.ToString());
          ei.EvalNumber = cmb.SelectedIndex;
          update_level_description(ei.Tag, ei.EvalNumber);
        }
      }


      //foreach (StudentEvaluationItem ei in _student.EvaluationItems)
      //{
      //  int idx;
      //  for (idx = 0; idx < grdEvaluationScheme.Children.Count; idx++)
      //  {
      //    UIElement ui_element = grdEvaluationScheme.Children[idx];
      //    if (ui_element is ComboBox)
      //    {
      //      ComboBox cmb = (ComboBox)ui_element;
      //      if (cmb.Tag.ToString() == ei.Tag)
      //      {
      //        if (cmb.SelectedIndex < 0)
      //        {
      //          ei.EvalNumber = -1;
      //        }
      //        else
      //        {
      //          ei.EvalNumber = cmb.SelectedIndex;
      //        }
      //      }
      //    }
      //  }
      //}
    }

    #endregion
  }
}
