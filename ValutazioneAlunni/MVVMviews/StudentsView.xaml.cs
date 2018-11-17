using System;
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

    #endregion

    public StudentsView()
    {
      InitializeComponent();

      messenger_init();
    }

    private void messenger_init()
    {
      Messenger.Default.Register<EvaluationScheme>(this, messenger_set_evaluation_scheme, "SetEvaluationScheme");
      Messenger.Default.Register<StudentData>(this, messenger_set_student, "SetCurrentStudent");
    }

    private void messenger_deinit()
    {
      Messenger.Default.Unregister(this, "SetEvaluationScheme");
      Messenger.Default.Unregister(this, "SetCurrentStudent");
    }

    private void messenger_set_evaluation_scheme(EvaluationScheme scheme)
    {
      _evaluation_scheme = scheme;
      on_set_evaluation_scheme();
    }

    private void messenger_set_student(StudentData student)
    {
      _student = student;
    }

    private void on_set_evaluation_scheme()
    {
      int grid_row = 0;
      RowDefinition rd;
      TextBox tb;

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
        tb.Text = chapter.Name;
        tb.TextWrapping = TextWrapping.Wrap;
        tb.IsReadOnly = true;
        tb.FontSize = 22;
        tb.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
        tb.BorderThickness = new Thickness(0,0,0,1);
        tb.Margin = new Thickness(3, 20, 3, 3);
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
        foreach(EvaluationSection sec in chapter.Sections)
        {
          // Section title (name)
          rd = new RowDefinition();
          rd.Height = GridLength.Auto;
          grdEvaluationScheme.RowDefinitions.Add(rd);
          tb = new TextBox();
          tb.Text = sec.Name;
          tb.TextWrapping = TextWrapping.Wrap;
          tb.IsReadOnly = true;
          tb.FontSize = 14;
          tb.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
          tb.BorderThickness = new Thickness(0);
          tb.Margin = new Thickness(3, 10, 3, 3);
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
        }
      }

      rd = new RowDefinition();
      grdEvaluationScheme.RowDefinitions.Add(rd);
    }

    private void on_set_student()
    {
      if (_evaluation_scheme == null) return;
    }
  }
}
