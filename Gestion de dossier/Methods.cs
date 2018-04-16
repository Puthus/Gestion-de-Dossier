using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Gestion_de_dossier
{
    class Methods //: INotifyPropertyChanged
    {
        public static List<DataTable> Results;
        public static MainWindow Main;
        public static Grid Second;
        public static int Numitems;
        private static int _currentPage = 1;
        public static DataTable Filler(MainWindow a)
        {
            Main = a;
            MySqlConnection connection = new MySqlConnection(connection_string);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Parameters.Add(new MySqlParameter("@debut", MySqlDbType.Date));
            cmd.Parameters.Add(new MySqlParameter("@fin", MySqlDbType.Date));
            DataTable dt = new DataTable();


            if (a.Debut.SelectedDate != null)
            {
                DateTime date = a.Debut.SelectedDate.Value;
                cmd.Parameters["@debut"].Value = Getdate(date.ToShortDateString());
            }

            if (a.Fin.SelectedDate != null)
            {
                DateTime date = a.Fin.SelectedDate.Value;
                cmd.Parameters["@fin"].Value = Getdate(date.ToShortDateString());
            }

            cmd.CommandText = "select * from dossier where date_dossier >= @debut and date_dossier <= @fin";
            cmd.Connection = connection;

            try
            {
                connection.Open();
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error : " + exception.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        private static string Getdate(string x)
        {
            string[] a = x.Split('/');
            return a[2] + "-" + a[0] + "-" + a[1];
        }


        public static int CurrentPage
        {
            set
            {
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (_currentPage != value && value <= Results.Count && value > 0)
                {
                    _currentPage = value;
                }
            }
        }

        public static List<DataTable> GetResults(DataTable x, int nbr, Grid se)
        {
            Second = se;
            Numitems = nbr;
            DataTable v = x.Clone();
            List<DataTable> result = new List<DataTable>();
            int k = 1;
            int j = 0;

            v.TableName = "Table_" + k;
            for (int i = 0; i < x.Rows.Count; i++)
            {
                DataRow newRow = v.NewRow();
                newRow.ItemArray = x.Rows[i].ItemArray;
                v.Rows.Add(newRow);
                j++;
                if (j == nbr)
                {
                    k++;
                    result.Add(v);
                    v = x.Clone();
                    v.TableName = "Table_" + k;
                    v.Clear();
                    j = 0;
                }
                else if (i == x.Rows.Count - 1)
                {
                    result.Add(v);
                }
            }
            return result;
        }
        public static void Databind()
        {
            if (CurrentPage <= Results.Count && CurrentPage > 0)
            {
                Second._grid.DataContext = Results[CurrentPage - 1];
            }
        }
    }
}
