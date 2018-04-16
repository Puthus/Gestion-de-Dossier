using System;
using System.Windows;
using System.Windows.Controls;

namespace Gestion_de_dossier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    //, IValueConverter
    {
        public MainWindow()
        {
            InitializeComponent();
            txtNum.Text = _numValue.ToString();
            DataContext = this;
            Debut.SelectedDate = DateTime.Today.AddDays(-7);
            Fin.SelectedDate = DateTime.Today;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Methods.CurrentPage = 1;
            Hide();
            Grid _g1 = new Grid();
            Methods.Second = _g1;
            Methods.Second.Show();
            Refrech();
        }

        //num up down
        private int _numValue;

        private int NumValue
        {
            get => _numValue;
            set
            {
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtNum.Text, out _numValue))
            {
                if (_numValue > 0)
                {
                    NumValue--;
                }
            }
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            if (!int.TryParse(txtNum.Text, out _numValue))
                txtNum.Text = _numValue.ToString();
        }

        private void Refrech()
        {
            Methods.Results = Methods.GetResults(Methods.Filler(this), Int32.Parse(txtNum.Text), Methods.Second);
            if (Methods.CurrentPage > Methods.Results.Count)
            {
                Methods.CurrentPage = Methods.Results.Count;
            }
            Methods.Databind();
        }
    }
}
