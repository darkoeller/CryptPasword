using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CryptLibrary;
using Newtonsoft.Json.Linq;

namespace CryptPasword.UC
{
    /// <summary>
    /// Interaction logic for DodajObrisiKorisnika.xaml
    /// </summary>
    public partial class DodajObrisiKorisnika : UserControl
    {
        private readonly Korisinici _korisnik = new Korisinici();

        public DodajObrisiKorisnika()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PopuniMrezu(out JArray item);
            KorisiniciDataGrid.ItemsSource = item;
        }

        private static void PopuniMrezu(out JArray item)
        {
            
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            item = (JArray) rss["Korisnici"];
            foreach (var arr in item)
            {
                var desifrator = new EncDecrypt(arr.ToString());
            }
        }


        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            var ime = KriptirajTekst(TxtIme.Text);
            var pasword = KriptirajTekst(TxtPassword.Password);
            DodajUJson(ime, pasword);

        }
        private string KriptirajTekst(string tekst)
        {
            var proces = new EncDecrypt(tekst);
            tekst = proces.Encrypt();
            return tekst;
        }
        private void DodajUJson(string ime, string pasword)
        {
            _korisnik.Ime = ime;
            _korisnik.Password = pasword;
            var dodajKorisnika = new CitajPisiJson(_korisnik);
            dodajKorisnika.ProcessJson();
           
        }

        private string VratiPassword()
        {
            if (!string.IsNullOrWhiteSpace(TxtPassword.Password)) MessageBox.Show("Niste ništa upisali");
            return TxtPassword.Password;
        }

        private string VratiIme()
        {
            if (!string.IsNullOrWhiteSpace(TxtIme.Text)) return TxtIme.Text;
            MessageBox.Show("Niste ništa upisali");
            return string.Empty;

        }
    }
}
