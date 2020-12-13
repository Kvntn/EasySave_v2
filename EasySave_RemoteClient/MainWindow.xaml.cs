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

        public MainWindow()
        {
            InitializeComponent();
            ClientSocket cs = new ClientSocket();
            Thread CTh = new Thread(new ThreadStart(cs.StartClient));
            CTh.Name = "Client Server Socket";
            CTh.Start();
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






    }
}
