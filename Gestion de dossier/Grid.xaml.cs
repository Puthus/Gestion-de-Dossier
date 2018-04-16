using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Gestion_de_dossier
{
    /// <summary>
    /// Interaction logic for Grid.xaml
    /// </summary>
    public partial class Grid : INotifyPropertyChanged
    {

        public Grid()
        {
            InitializeComponent();
            DataContext = this;
            CurrentPage = Methods.CurrentPage;
        }

        private static int _currentPage;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]  string propretyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propretyName));
        }

        private void Button_Next(object sender, RoutedEventArgs e)
        {
            //if (Methods.CurrentPage < Methods.Results.Count)
            //{
            Pageer("Plus");
            Databind();
            Pager_restart();
            //}
        }

        private void Button_Previous(object sender, RoutedEventArgs e)
        {
            //if (Methods.CurrentPage > 1)
            //{
            Pageer("Minus");
            Databind();
            Pager_restart();
            //}
        }
        private void Button_Refrech(object sender, RoutedEventArgs e)
        {
            Refrech();
        }

        private void Databind()
        {
            if (Methods.CurrentPage <= Methods.Results.Count && Methods.CurrentPage > 0)
            {
                _grid.DataContext = Methods.Results[Methods.CurrentPage - 1];
            }
        }

        private void Refrech()
        {
            Methods.Results = Methods.GetResults(Methods.Filler(Methods.Main), Methods.Numitems, this);
            if (Methods.CurrentPage > Methods.Results.Count)
            {
                Methods.CurrentPage = Methods.Results.Count;
            }
            Databind();
        }

        private void Pageer(string x)
        {
            if (x == "Plus")
            {
                Methods.CurrentPage++;
            }
            else if (x == "Minus")
            {
                Methods.CurrentPage--;
            }
            else if (x == "First")
            {
                Methods.CurrentPage = 1;
            }
            else if (x == "Last")
            {
                Methods.CurrentPage = Methods.Results.Count;
            }
            CurrentPage = Methods.CurrentPage;
            Databind();
        }

        private void Grid_OnClosed(object sender, EventArgs e)
        {
            //Methods.Main.Show();
        }

        DispatcherTimer Pager = new DispatcherTimer();
        DispatcherTimer Fader = new DispatcherTimer();

        private void Grid_OnLoaded(object sender, RoutedEventArgs e)
        {
            Pager.Interval = TimeSpan.FromSeconds(5);
            Pager.Tick += PagerTicker;
            Pager.Start();
        }

        private void PagerTicker(Object sender, EventArgs e)
        {
            Pageer("Plus");
        }

        private void FaderTicker(Object sender, EventArgs e)
        {

        }

        public void Pager_restart()
        {
            Pager.Stop();
            Pager.Start();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_First(object sender, RoutedEventArgs e)
        {
            Pageer("First");
        }

        private void Button_last(object sender, RoutedEventArgs e)
        {
            Pageer("Last");
        }

        private void Button_pause_play(object sender, RoutedEventArgs e)
        {
            if (Pager.IsEnabled)
            {
                Pager.Start();
            }
            else
            {
                Pager.Stop();
            }
        }

        private void Button_back(object sender, RoutedEventArgs e)
        {
            Methods.Second.Close();
            Methods.Main.Show();
        }
    }
}
