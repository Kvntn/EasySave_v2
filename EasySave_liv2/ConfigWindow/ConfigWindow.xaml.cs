using EasySave_liv2.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace EasySave_liv2.ConfigWindow
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private List<string> Prog = new List<string>();
        private List<string> Ext = new List<string>();

        private View_Model vm;

        public ConfigWindow(View_Model vm)
        {
            InitializeComponent(); // loading window
            this.vm = vm;            
            vm.LoadConfig(); // actualize config content

            Prog = vm.Programs; //importing lists sources
            Ext = vm.Extensions;

            lb_P.ItemsSource = Prog; //Lists sources
            lb_E.ItemsSource = Ext;
        }

        private void Button_Add_P(object sender, RoutedEventArgs e)
        {
            if (Prog == null)
                Prog = new List<string>();

            Prog.Add(txt_prog.Text);

            txt_prog.Text = String.Empty;
            vm.SaveConfig(Prog, Ext);
        }

        private void Button_Remove_P(object sender, RoutedEventArgs e)
        {
            if (Prog != null)
                if (Prog.Count != 0)
                    foreach (string str in Prog)
                        if (str == txt_prog.Text)
                        {
                            Prog.Remove(str);
                            txt_prog.Text = String.Empty;
                            vm.SaveConfig(Prog, Ext);
                            return;
                        }


        }

        private void Button_Add_E(object sender, RoutedEventArgs e)
        {
            if (Ext == null)
                Ext = new List<string>();

            Ext.Add(txt_ext.Text);
            txt_ext.Text = String.Empty;

            vm.SaveConfig(Prog, Ext);
        }

        private void Button_Remove_E(object sender, RoutedEventArgs e)
        {
            if (Ext != null)
                if (Ext.Count != 0)
                    foreach (string str in Ext)
                        if (str == txt_ext.Text)
                        {
                            Ext.Remove(str);
                            txt_ext.Text = String.Empty;
                            vm.SaveConfig(Prog, Ext);
                            return;
                        }

        }

        private void TextBox_TextChanged_P(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_E(object sender, TextChangedEventArgs e)
        {

        }

        private void lb_P_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txt_prog.Text = lb_P.SelectedItem.ToString();
        }

        private void lb_E_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txt_ext.Text = lb_E.SelectedItem.ToString();
        }
    }
}
