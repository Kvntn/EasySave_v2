using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using EasySave_liv2.View;
using EasySave_liv2.ViewModel;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace EasySave_liv2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Attributes
        private View_Model vm = new View_Model();
        private LangEnum lang = LangEnum.EN;
        public List<string> BackupNames = new List<string>();
  


        public MainWindow()
        {
            InitializeComponent();
            listbox_backup.ItemsSource = vm.listBackup;

        }


// ----------------------Useful methods for a more user-friendly interface---------------------------------
        //ListView methods

        private void ListBox_BackupList(object sender, SelectionChangedEventArgs e)
        {
            if (listbox_backup.SelectedItem != null)
                outputSave.Text = listbox_backup.SelectedItem.ToString();
        }

        private void listView1_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            System.Windows.MessageBox.Show(e.Item.Text.ToString());
        }


        //Button related methods

        private void Button_Creates(object sender, RoutedEventArgs e)
        {
            bool diff = Check_Differential.IsChecked ?? false;
            vm.CreateBackupUI(input_src.Text, input_dst.Text, input_name.Text, diff);

            vm.listBackup.Add(input_name.Text);
            //listbox_backup.Items.Insert(names.Count() ,input_name.Text);
            listbox_backup.DataContext = vm.listBackup;
            

            if (lang == LangEnum.EN)
                outputCreate.Text = "Backup successfully created !";
            else if (lang == LangEnum.FR)
                outputCreate.Text = "Sauvegarde créée !";
            else if (lang == LangEnum.RU)
                outputCreate.Text = "Резервное копирование сохранено !";
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {

            if (vm.OnSaveProgramPrevention())
            {
                foreach (string str in listbox_backup.SelectedItems)
                    vm.FindBackupByName(str);

                if (lang == LangEnum.EN)
                    outputSave.Text = "Backup successfully done !";
                else if (lang == LangEnum.FR)
                    outputSave.Text = "Sauvegarde réussie !";
                else if (lang == LangEnum.RU)
                    outputCreate.Text = "Cделана резервная копия!";
            }
                
            else
            {
                if (lang == LangEnum.EN)
                    outputSave.Text = "A program prevents from saving !";
                else if (lang == LangEnum.FR)
                    outputSave.Text = "Un programme empêche la sauvegarde !";
                else if (lang == LangEnum.RU)
                    outputCreate.Text = "Программа препятствует резервному копированию!";
            }
                

            
        }

        //Source path browsing
        private void Button_Select_Src(object sender, RoutedEventArgs e)
        {
            using (var dlg = new CommonOpenFileDialog())
            {
                dlg.IsFolderPicker = true;
                CommonFileDialogResult rs = dlg.ShowDialog();
                if (rs == CommonFileDialogResult.Ok)
                    input_src.Text = dlg.FileName;

            }
        }

        //Destination path browsing
        private void Button_Select_Dst(object sender, RoutedEventArgs e)
        {
            using (var dlg = new CommonOpenFileDialog())
            {
                dlg.IsFolderPicker = true;
                CommonFileDialogResult rs = dlg.ShowDialog();
                if (rs == CommonFileDialogResult.Ok)
                    input_dst.Text = dlg.FileName;

            }
        }

        //Open config file
        private void Button_Config(object sender, RoutedEventArgs e)
        {
            new ConfigWindow.ConfigWindow(vm).Show();
        }

        
// ----------------------LANGUAGE SWITCH METHODS (used in droplist)---------------------------------
        

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
            txt_Config.Text = "Ajoutez les programmes pouvant causer des conflits avec les sauvegardes (sans \".exe\")";

            lang = LangEnum.FR;
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
            txt_Config.Text = "Add program that may prevent from saving successfully (without \".exe\" extension)";

            lang = LangEnum.EN;
        }

        private void RU_Click(object sender, RoutedEventArgs e)
        {
            txt_Create.Text = "Создать резервную копию:";
            txt_Extensions.Text = "Расширения для шифрования:" +
                "\n Разделите их, используя ';' (.docx; .txt) ";
            txt_Use.Text = "Использовать резервную копию:";
            Button_Create.Content = "Создать";
            Button_Start.Content = "Старт";
            Check_Differential.Content = "Дифференциальная (по умолчанию резервная копия - полная)";
            src_Path.Text = "Исходный путь";
            dest_Path.Text = "Путь назначения";
            backup_Name.Text = "Имя копии";
            button_src.Content = button_dst.Content = "...";
            button_config.Content = "Файл конфигурации";
            txt_Config.Text = "Добавить программы, которые могут вызвать конфликты с резервными копиями (без \".exe\")";

            lang = LangEnum.RU;
        }

// --------------------------NOT IMPLEMENTED METHODS---------------------------------

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_EncryptExtensions(object sender, TextChangedEventArgs e)
        {

        }

        private void Text_SourcePath(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_SrcPath(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }
    }
}
