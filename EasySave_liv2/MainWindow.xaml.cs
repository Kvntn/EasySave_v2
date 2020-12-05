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
using EasySave_liv2.ViewModel;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace EasySave_liv2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private View_Model vm = new View_Model();
        private bool EN = true;
        public List<string> names = new List<string>();
        private readonly static string configFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EasySave\config.json";

        public MainWindow()
        {
            InitializeComponent();
            listbox_backup.ItemsSource = vm.listBackup;

        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_EncryptExtensions(object sender, TextChangedEventArgs e)
        {

        }

        private void ListBox_BackupList(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Text_SourcePath(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_SrcPath(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Creates(object sender, RoutedEventArgs e)
        {
            bool diff = Check_Differential.IsChecked ?? false;
            vm.CreateBackupUI(input_src.Text, input_dst.Text, input_name.Text, diff);

            vm.listBackup.Add(input_name.Text);
            listbox_backup.Items.Insert(names.Count() ,input_name.Text);
            listbox_backup.UpdateLayout();
            

            if (EN)
                outputCreate.Text = "Backup successfully created !";
            else
                outputCreate.Text = "Sauvegarde créée !";
        }
        
        private void FR_Click(object sender, RoutedEventArgs e)
        {
            txt_Create.Text = "Créez une sauvegarde :";
            txt_Extensions.Text = "Extensions à chiffrer :" +
                "\n Séparez-les avec ';' (.docx;.txt)";
            txt_Use.Text = "Effectuez une sauvegarde";
            Button_Create.Content = "Créer";
            Button_Start.Content = "Démarrer";
            Check_Differential.Content = "Différentielle (par défaut, la sauvegarde est complète)";
            src_Path.Text = "Chemin source";
            dest_Path.Text = "Chemin de dest.";
            backup_Name.Text = "Nom de la sauv.";
            button_src.Content = button_dst.Content = "...";
            button_config.Content = "Fichier de configuration";

            EN = false;
        }

        private void EN_Click(object sender, RoutedEventArgs e)
        {
            txt_Create.Text = "Create backup :";
            txt_Extensions.Text = "Extrensions to encrypt :" +
                "\n Separate them using ';' (.docx;.txt)";
            txt_Use.Text = "Use a backup :";
            Button_Create.Content = "Create";
            Button_Start.Content = "Start";
            Check_Differential.Content = "Differential (default backup is a complete backup)";
            src_Path.Text = "Source path";
            dest_Path.Text = "Destination path";
            backup_Name.Text = "Backup name";
            button_src.Content = button_dst.Content = "...";
            button_config.Content = "Configuration file";

            EN = true;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Select_Src(object sender, RoutedEventArgs e)
        {
            using (var dlg = new CommonOpenFileDialog())
            {
                dlg.IsFolderPicker = true;
                dlg.ShowDialog();
                if (dlg.EnsurePathExists)
                    input_src.Text = dlg.FileName;

            }
        }
        private void Button_Select_Dst(object sender, RoutedEventArgs e)
        {
            using (var dlg = new CommonOpenFileDialog())
            {
                dlg.IsFolderPicker = true;
                dlg.ShowDialog();
                if (dlg.EnsurePathExists)
                    input_dst.Text = dlg.FileName;

            }
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            foreach (string str in listbox_backup.SelectedItems)
            {
                vm.FindBackupByName(str);
            }

            if (EN)
                outputCreate.Text = "Backup successfully done !";
            else
                outputCreate.Text = "Sauvegarde réussie !";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", configFile);
        }
    }
}
