using System.Windows;
using CryptLibrary;

namespace CryptLogin
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
      


        private void BtnPrijava_Click(object sender, RoutedEventArgs e)
        {
            var ime =  VratiIme();
            var pass = VratiPasword();
            var upit = new UpitUJson(ime, pass);
            var jelDobro = upit.JelDobarLogin();
            if (jelDobro)
            {
                DozvoljenPristup();
            }
            else
            {
                 MessageBox.Show("Molim ponovite pokušaj logiranja!");
            }
        }

        private void DozvoljenPristup()
        {

            TabUprava.Visibility = Visibility.Visible;
            KontrolaTabova.SelectedIndex = 1;
            TabLogin.Visibility = Visibility.Collapsed;
            UpdateLayout();

        }


        private string VratiIme()
        {
            if (!string.IsNullOrWhiteSpace(TxtIme.Text))
            {
                var ime = TxtIme.Text.Trim();
                ime = KriptirajTekst(ime);
                return ime;
            }
            MessageBox.Show("Upišite korisničko ime!");
            return string.Empty;
            
        }

        private string VratiPasword()
        {
            if (!string.IsNullOrWhiteSpace(TxtPass.Password))
            {
                var pass = TxtPass.Password.Trim();
                pass=KriptirajTekst(pass);
                return pass;
            }
            MessageBox.Show("Upišite svoj password!");
            return string.Empty;
        }
        private static string KriptirajTekst(string tekst)
        {
            var proces = new EncDecrypt(tekst);
            tekst = proces.Encrypt();
            return tekst;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtIme.Focus();
        }
       
    }
}
