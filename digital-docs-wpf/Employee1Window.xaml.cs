using System.Windows;

namespace digital_docs_wpf
{
    public partial class Employee1Window : Window
    {
        public Employee1Window()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var newForm = new AssignEmployeeWindow();
            newForm.Show();
            Close();
        }
    }
}
