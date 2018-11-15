using System.Windows;

namespace digital_docs_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var newForm = new User1_NewTasks();
            newForm.Show();
            XmlTransformer xmlTranformer = new XmlTransformer();
            Close();
        }
    }
}