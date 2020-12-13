using EasySave.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace EasySave.ConfigWindow
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private View_Model vm;
        private ObservableCollection<string> _prog = new ObservableCollection<string>();
        private ObservableCollection<string> _ext = new ObservableCollection<string>();
        private ObservableCollection<string> _pext = new ObservableCollection<string>();

        public ConfigWindow(View_Model vm)
        {
            this.vm = vm;
            InitializeComponent(); // loading window

            foreach (string str in vm.Programs)
            {
                _prog.Add(str);
                lb_P.Items.Add(str); //listbox of programs that prevent from saving
            }
            foreach (string str in vm.Extensions)
            {
                _ext.Add(str);
                lb_E.Items.Add(str); // listbox of extensions that need encryption
            }
            foreach (string str in vm.PExtensions)
            {
                _pext.Add(str);
                lb_PE.Items.Add(str); // listbox of prioritary extensions 
            }



            //DataContext assigned to the window
            lb_P.DataContext = this; 
            lb_E.DataContext = this; 
            lb_PE.DataContext = this;

        }
    
//----------------------PROGRAMS THAT PREVENT FROM SAVING------------------

        private void Button_Add_P(object sender, RoutedEventArgs e)
        {
            bool isCancelled = false;
            
            if (vm.Programs != null)
                if (vm.Programs.Count != 0)
                    foreach (string str in vm.Programs)
                        if (str == txt_prog.Text)
                            isCancelled = true;

            if(!isCancelled)
            {
                lb_P.Items.Add(txt_prog.Text);
                vm.Programs.Add(txt_prog.Text);
                txt_prog.Text = String.Empty;
                vm.SaveConfig(vm.Programs, vm.Extensions, vm.PExtensions);
            }
                
        }

        private void Button_Remove_P(object sender, RoutedEventArgs e)
        {
            if (vm.Programs != null && txt_prog.Text != "")
                if (vm.Programs.Count != 0)
                    foreach (string str in vm.Programs)
                        if (str == txt_prog.Text)
                        {
                            vm.Programs.Remove(str);
                            lb_P.Items.Remove(str);
                            txt_prog.Text = String.Empty;
                            vm.SaveConfig(vm.Programs, vm.Extensions, vm.PExtensions);
                            return;
                        }
        }

//---------------------EXTENSIONS TO ENCRYPT-------------------------------

        private void Button_Add_E(object sender, RoutedEventArgs e)
        {
            if (vm.Extensions == null)
                vm.Extensions = new List<string>();

            bool isCancelled = false;

            if (vm.Extensions != null)
                if (vm.Extensions.Count != 0)
                    foreach (string str in vm.Extensions)
                        if (str == txt_ext.Text)
                            isCancelled = true;

            if (!isCancelled)
            {
                lb_E.Items.Add(txt_ext.Text);
                vm.Extensions.Add(txt_ext.Text);
                txt_ext.Text = String.Empty;
                vm.SaveConfig(vm.Programs, vm.Extensions, vm.PExtensions);
            }
        }

        private void Button_Remove_E(object sender, RoutedEventArgs e)
        {
            if (vm.Extensions != null && txt_ext.Text != "")
                if (vm.Extensions.Count != 0)
                    foreach (string str in vm.Extensions)
                        if (str == txt_ext.Text)
                        {
                            lb_E.Items.Remove(txt_ext.Text);
                            vm.Extensions.Remove(txt_ext.Text);
                            txt_ext.Text = String.Empty;
                            vm.SaveConfig(vm.Programs, vm.Extensions, vm.PExtensions);
                            return;
                        }
        }

//---------------------SAVE PRIOTITY EXTENSIONS------------------------

        private void Button_Add_PE(object sender, RoutedEventArgs e)
        {
            if (vm.PExtensions == null)
                vm.PExtensions = new List<string>();

            bool isCancelled = false;

            if (vm.PExtensions != null)
                if (vm.PExtensions.Count != 0)
                    foreach (string str in vm.PExtensions)
                        if (str == txt_pext.Text)
                            isCancelled = true;

            if (!isCancelled)
            {
                lb_PE.Items.Add(txt_pext.Text);
                vm.PExtensions.Add(txt_pext.Text);
                txt_pext.Text = String.Empty;
                vm.SaveConfig(vm.Programs, vm.Extensions, vm.PExtensions);
            }
        }


        private void Button_Remove_PE(object sender, RoutedEventArgs e)
        {
            if (vm.PExtensions != null && txt_pext.Text != "")
                if (vm.PExtensions.Count != 0)
                    foreach (string str in vm.PExtensions)
                        if (str == txt_pext.Text)
                        {
                            lb_PE.Items.Remove(txt_pext.Text);
                            txt_pext.Text = String.Empty;
                            vm.PExtensions.Remove(txt_pext.Text);
                            vm.SaveConfig(vm.Programs, vm.Extensions, vm.PExtensions);
                            return;
                        }
        }


        //listbox related functions

        private void lb_P_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lb_P.SelectedItem != null)
                txt_prog.Text = lb_P.SelectedItem.ToString();
        }

        private void lb_E_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_E.SelectedItem != null)
                txt_ext.Text = lb_E.SelectedItem.ToString();
        }
    
        private void TextBox_TextChanged_P(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_E(object sender, TextChangedEventArgs e)
        {

        }

        public void lb_PE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_PE.SelectedItem != null)
                txt_pext.Text = lb_PE.SelectedItem.ToString();
        }
    }
}
