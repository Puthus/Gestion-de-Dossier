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
            CurrentPage = Methods.CurrentPage;
        }

        private void Grid_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        DispatcherTimer Pager = new DispatcherTimer();

        private void Grid_OnLoaded(object sender, RoutedEventArgs e)
        {
            Pager.Interval = TimeSpan.FromSeconds(5);
            Pager.Tick += PagerTicker;
            Pager.Start();
        }

        private void PagerTicker(Object sender, EventArgs e)
        {
            Pageer("Plus");
            Databind();
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
    }
}
