using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EasySave.Model.Remote;
using EasySave_RemoteClient.src;

namespace EasySave_RemoteClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window    
    {
        private enum LangEnum { FR, EN, RU, AR }

        private LangEnum lang = LangEnum.EN;
        private Backend backend = new Backend();
        private ObservableCollection<string> lstr = new ObservableCollection<string>();
        private ObservableCollection<ClientObjectFormat> grid = new ObservableCollection<ClientObjectFormat>();

        public MainWindow()
        {
            InitializeComponent();

            listbox_backup.DataContext = this;
            dataGrid.DataContext = this;

            listbox_backup.ItemsSource = lstr;
            dataGrid.ItemsSource = grid;

            input_ip.Text = "localhost";
        }

        private void Button_Connect(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstr.Count > 0)
                    lstr.Clear();
                backend.StartSocketThread(input_ip.Text, "Data");
                
                Thread.Sleep(1500);
                if (backend.StrList.Count > 0)
                    foreach (string str in backend.StrList)
                        lstr.Add(str);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server not found " + ex.ToString());
            }
        }


        private void Button_Disconnect(object sender, RoutedEventArgs e)
        {
            try
            {
                backend.StartSocketThread(input_ip.Text, "closeConnection");
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server not found " + ex.ToString());
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
           
            if (lstr.Count == 0)
            {
                MessageBox.Show("You must be connected to a server.");
                return;
            }

            if (grid.Count > 0)
                grid.Clear();

            if (lstr.Count == 0)
                return;

            if (listbox_backup.SelectedItems.Count > 0)
            {
                foreach (string item in listbox_backup.SelectedItems)
                    foreach (ClientObjectFormat clientObject in backend.cof)
                        if(clientObject.Name == item)
                        {
                            grid.Add(clientObject);
                            break;
                        }
                backend.StartSocketThread(input_ip.Text, "Progress");

            }

            else
            {
                if (lang == LangEnum.EN)
                    MessageBox.Show("No backup selected");
                else
                    MessageBox.Show("Aucune sauvegarde séléctionnée");
            }
        }
        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Pause_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {

        }


        
    }
}
