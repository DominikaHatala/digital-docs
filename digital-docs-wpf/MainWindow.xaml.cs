using System.Windows;

namespace digital_docs_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var newForm = new Employee1Window();
            newForm.Show();
            Close();
        }
    }
}