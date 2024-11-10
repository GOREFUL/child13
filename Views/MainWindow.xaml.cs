using child13.Service;
using child13.Models;
using DotNetEnv;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using str = child13.Resources.Localization.Strings;
using System.Text;
using child13.Views;

namespace child13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        private string _connection;
        private string _message;
        public int x1, x2, y1, y2;
        public static readonly HttpClient client = new HttpClient();
        public CultureInfo _culture;
        public Coordinate first;
        public Coordinate second;
        public Coordinate third;
        private Coordinate coordinate = new Coordinate();
        #endregion

        #region Window
        public MainWindow()
        {
            Env.LoadContents(TestTxt);
            _connection = Environment.GetEnvironmentVariable("child13_API_KEY");
            InitializeComponent();
            ShowLanguageSelection();
        }
        public static string TestTxt
        {
            get
            {
                var info = Assembly.GetExecutingAssembly().GetName();
                var name = info.Name;
                var stream = Assembly
                    .GetExecutingAssembly()
                    .GetManifestResourceStream($"{name}..env");
                var streamReader = new StreamReader(stream, Encoding.UTF8);
                return streamReader.ReadToEnd();
            }
        }
        private void Tompost_mode_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            _message = "";
        }
        #region Font
        private void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_txtTranslate != null && _txtOriginal != null && _cbFont.SelectedItem is ComboBoxItem selectedItem)
            {
                var fontName = selectedItem.Content.ToString();
                _txtTranslate.FontFamily = new FontFamily(fontName);
                _txtOriginal.FontFamily = new FontFamily(fontName);
            }
        }
        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_txtTranslate != null && _txtOriginal != null && _cbFontSize.SelectedItem is ComboBoxItem selectedItem)
            {
                if (double.TryParse(selectedItem.Content.ToString(), out double fontSize))
                {
                    _txtTranslate.FontSize = fontSize;
                    _txtOriginal.FontSize = fontSize;
                }
            }
        }
        #endregion
        #region Bold
        private void BoldCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _txtTranslate.FontWeight = FontWeights.Bold;
            _txtOriginal.FontWeight = FontWeights.Bold;
        }
        private void BoldCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _txtTranslate.FontWeight = FontWeights.Normal;
            _txtOriginal.FontWeight = FontWeights.Normal;
        }
        #endregion
        #region Italic
        private void ItalicCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _txtTranslate.FontStyle = FontStyles.Italic;
            _txtOriginal.FontStyle = FontStyles.Italic;
        }
        private void ItalicCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _txtTranslate.FontStyle = FontStyles.Normal;
            _txtOriginal.FontStyle = FontStyles.Normal;
        }
        #endregion
        private void BackgroundColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)_cbBack.SelectedItem;
            if (selectedItem != null)
            {
                SolidColorBrush backgroundColor = (SolidColorBrush)selectedItem.Background;
                if (backgroundColor.Color == Colors.Black)
                {
                    this.Background = backgroundColor;
                    _txtTranslate.Foreground = Brushes.White;
                    _txtOriginal.Foreground = Brushes.White;
                }
                else if (backgroundColor.Color == Colors.White)
                {
                    this.Background = backgroundColor;
                    _txtTranslate.Foreground = Brushes.Black;
                    _txtOriginal.Foreground = Brushes.Black;
                }
            }
        }
        #endregion

        #region API_Check
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            API_Check aPI_Check = new API_Check();
            bool? dialogResult = aPI_Check.ShowDialog();

            if (dialogResult == true || dialogResult == false)
            {
                _message = IdentityCode._message;
            }
        }
        #endregion

        #region Localization
        private void ShowLanguageSelection()
        {
            TranslateWindow languageWindow = new TranslateWindow();
            if (languageWindow.ShowDialog() == true)
            {
                if (languageWindow.SelectedLanguage != null)
                {
                    ChangeLanguage(languageWindow.SelectedLanguage);
                }
                else
                {
                    MessageBox.Show("Language not selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
        }

        private void ChangeLanguage(string culture)
        {
            _culture = new CultureInfo(culture);
            switch (_culture.Name)
            {
                case "ua":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ua");
                    break;
                case "ru":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
                    break;
                case "pl":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl");
                    break;
                default:
                    break;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            _btnCoordinate.Content = str._btnCoordinate;
            _btnTompost.Content = str._btnTompost;
            _btnFirstClear.Content = str._btnFirstClear;
            _btnSecondClear.Content = str._btnSecondClear;
            _btnThirdClear.Content = str._btnThirdClear;
            _btnClearAll.Content = str._btnClearAll;
            _btnFirst.Content = str._btnFirst;
            _btnSecond.Content = str._btnSecond;
            _btnThird.Content = str._btnThird;
            _txtSettings.Text = str._txtSettings;
            _lblFont.Content = str._lblFont;
            _lblSize.Content = str._lblSize;
            _lblBack.Content = str._lblBack;
            _cbItemWhite.Content = str._cbItemWhite;
            _cbItemBlack.Content = str._cbItemBlack;
        }
        #endregion

        #region Any methods
        private void text_to_textbox(string filePath)
        {
            string text = File.ReadAllText(filePath);
            _txtOriginal.Text = text;
        }
        private string getSourceName()
        {
            this.Hide();
            string name = ActiveWindowService._GetActiveWindow();
            this.Show();
            return name;
        }
        #endregion

        #region Coordinate
        private void OpenCoordinateWindowButton_Click(object sender, RoutedEventArgs e)
        {
            CoordinateRecorder coordinateWindow = new CoordinateRecorder(coordinate);
            coordinateWindow.ShowDialog();
            if (coordinateWindow.DialogResult != true)
            {
                x1 = coordinate._GetX1();
                y1 = coordinate._GetY1();
                x2 = coordinate._GetX2();
                y2 = coordinate._GetY2();

                if (first == null)
                    first = coordinate;
                else if (second == null)
                    second = coordinate;
                else if (third == null)
                    third = coordinate;
                else
                    MessageBox.Show("Всі зони вказані");
            }

        }
        #endregion

        #region MainFunctionality
        private async void _translateText()
        {
            string _text = TranslationService.TranslateText();
            await TextCorrectionService.ProcessTextAsync(_txtTranslate);
            string language = _culture.Name;
            string sourceName = getSourceName();
            string originalText = File.ReadAllText(@"Resources/text/output.txt");
            string _translatedText = await TranslationService.GetTranslationAsync(_connection, _message,
                sourceName, originalText, language);
            await TextCorrectionService.ProcessOutputTextAsync(_txtTranslate, _translatedText);
        }
        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            if (first == null)
            {
                MessageBox.Show(str._ErrorMessage_First);
            }
            else
            {
                ImageService.PreprocessImage("Resources/img/image.png", first._GetX1(), first._GetY1(),
                    first._GetX2(), first._GetY2());
                _translateText();
                text_to_textbox(@"Resources/text/output.txt");
            }
        }
        private void btnSecond_Click(object sender, RoutedEventArgs e)
        {
            if (first == null)
            {
                MessageBox.Show(str._ErrorMessage_First);
            }
            else
            {
                if (second == null)
                {
                    MessageBox.Show(str._ErrorMessage_Second);
                }
                else
                {
                    ImageService.PreprocessImage("Resources/img/image.png", second._GetX1(), second._GetY1(),
                        second._GetX2(), second._GetY2());
                    _translateText();
                    text_to_textbox(@"Resources/text/output.txt");
                }
            }
        }
        private void btnThird_Click(object sender, RoutedEventArgs e)
        {
            if (first == null)
            {
                MessageBox.Show(str._ErrorMessage_First);
            }
            else
            {
                if (second == null)
                {
                    MessageBox.Show(str._ErrorMessage_Second);
                }
                else
                {
                    if (third == null)
                    {
                        MessageBox.Show(str._ErrorMessage_Third);
                    }
                    else
                    {
                        ImageService.PreprocessImage("Resources/img/img.png", third._GetX1(), third._GetY1(),
                            third._GetX2(), third._GetY2());
                        _translateText();
                        text_to_textbox(@"Resources/text/output.txt");
                    }
                }
            }
        }
        #endregion

        #region Clear Bind Button
        private void btnFirstClear_Click(object sender, RoutedEventArgs e)
        {
            first = null;
        }

        private void btnSecondClear_Click(object sender, RoutedEventArgs e)
        {
            second = null;
        }

        private void btnThirdClear_Click(object sender, RoutedEventArgs e)
        {
            third = null;
        }
        private void _btnClear_Click(object sender, RoutedEventArgs e)
        {
            first = null;
            second = null;
            third = null;
        }
        #endregion
    }
}