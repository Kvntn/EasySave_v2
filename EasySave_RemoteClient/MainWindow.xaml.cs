using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using EasySave_RemoteClient.src;

namespace EasySave_RemoteClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientSocket cs;

        public MainWindow()
        {
            InitializeComponent();
            this.cs = new ClientSocket();
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {

           /* if (CONNECTION SUCCESSFUL)
            {
                foreach (string str in listbox_backup.SelectedItems)
                    jsp encore FindBackupByName(str);

                outputSave.Text = "Backup successfully done !";

            }

            else
            {
                outputSave.Text = "A program prevents from saving !";
            }*/

        }

        private void Button_Connect(object sender, RoutedEventArgs e)
        {
            cs.StartSocketThread(input_ip.Text);
        }

        private void EN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FR_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AR_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RU_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
