using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace digital_docs_wpf
{
    public partial class MailWindow : Window
    {
        public MailWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var newForm = new AssignEmployeeWindow();
            newForm.Show();
            Close();
        }

        private void listView_Click(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                listView.Items.Clear();
                listView.Items.Add(new Mail { Content = "Przykladowy tekst maila " + item });
            }
        }
    }
}
