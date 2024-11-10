using System.Windows;

namespace child13.Views
{
    /// <summary>
    /// Interaction logic for TranslateWindow.xaml
    /// </summary>
    public partial class TranslateWindow : Window
    {
        public string SelectedLanguage { get; private set; }

        public TranslateWindow()
        {
            InitializeComponent();
            SelectedLanguage = string.Empty;
        }

        private void Button_Ukr_Click(object sender, RoutedEventArgs e)
        {
            SelectedLanguage = "ua";
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Rus_Click(object sender, RoutedEventArgs e)
        {
            SelectedLanguage = "ru";
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Pol_Click(object sender, RoutedEventArgs e)
        {
            SelectedLanguage = "pl";
            this.DialogResult = true;
            this.Close();
        }
    }
}
