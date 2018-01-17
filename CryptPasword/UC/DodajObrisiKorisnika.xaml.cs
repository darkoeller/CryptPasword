using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using CryptLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptPasword.UC
{
    /// <summary>
    /// Interaction logic for DodajObrisiKorisnika.xaml
    /// </summary>
    public partial class DodajObrisiKorisnika : UserControl
    {
        private  Korisnici _korisnik = new Korisnici();

        public DodajObrisiKorisnika()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PopuniMrezu();
            TxtIme.Focus();
        }

        private void PopuniMrezu()
        {
            var rss = VratiObjekte(out IEnumerable<string> imena,
                out var passwordi);
            var admin = from a in rss["Korisnici"]
                select (bool)a["Admin"];

            var imenaArray = imena.Select(ime => new EncDecrypt(ime))
                .Select(desifrator => desifrator.Decrypt()).ToList();
            var paswordArray = passwordi.Select(password => new EncDecrypt(password))
                .Select(sifra => sifra.Decrypt())
                .ToList();
            var adminArray = admin.Select(ad => ad).ToList();

            var listaKorisnika = new List<Korisnici>();

            foreach (var ime in imenaArray)
            {
                 foreach (var pass in paswordArray)
                 {
                     foreach (var adm in adminArray)
                     {
                         listaKorisnika.Add(new Korisnici {Ime = ime, Password = pass, Admin = adm});
                         paswordArray.Remove(pass);
                         adminArray.Remove(adm);
                         break;
                     }
                     break;
                 }
            }
            KorisiniciDataGrid.ItemsSource = listaKorisnika;
        }

        private static JObject VratiObjekte(out IEnumerable<string> imena, out IEnumerable<string> passwordi)
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            imena = from p in rss["Korisnici"]
                select (string) p["Ime"];
            passwordi = from p in rss["Korisnici"]
                select (string) p["Password"];
            return rss;
        }


        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            _korisnik = (Korisnici) KorisiniciDataGrid.SelectedItem;
            var ime = KriptirajTekst(_korisnik.Ime);
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            var jarray = (JArray) rss["Korisnici"];
            var osobe = jarray.ToObject<IList<Korisnici>>();
            var zabrisati = osobe.SingleOrDefault(x => x.Ime == ime);
            osobe.Remove(zabrisati);
            var upisi = new CitajPisiJson();
            CitajPisiJson.DodajKorisnike(osobe);


        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!DaliPostoji()) return;
            KriptirajKorisnika();
            ProcesuirajKorisnika();
            OcistiKontrole();
        }

        private void KriptirajKorisnika()
        {
            _korisnik.Ime = KriptirajTekst(VratiIme());
            _korisnik.Password = KriptirajTekst(VratiPassword());
            _korisnik.Admin = JelAdmin();
        }

        private void OcistiKontrole()
        {
            TxtIme.Text = string.Empty;
            TxtPassword.Text = string.Empty;
            IsAdmin.IsChecked = false;
            TxtIme.Focus();
        }

        private bool DaliPostoji()
        {
            var postoji = NoviKorisnik();
             
            foreach (var dr in KorisiniciDataGrid.ItemsSource)
            {
                if (!dr.Equals(postoji)) continue;
                MessageBox.Show("Korisnik je već zadan.\nOdaberite drugo ime ili pasword!");
                return false;
            }
            return true;
        }

        private Korisnici NoviKorisnik()
        {
            var noviKorisnik = new Korisnici
            {
                Ime = VratiIme(),
                Password = VratiPassword(),
                Admin = JelAdmin()
            };
            return noviKorisnik;
        }

        private bool JelAdmin()
        {
            return IsAdmin.IsChecked == true;
        }

        private void ProcesuirajKorisnika()
        {
            DodajUJson();
            PopuniMrezu();
            KorisiniciDataGrid.Items.Refresh();
        }

        private string KriptirajTekst(string tekst)
        {
            var proces = new EncDecrypt(tekst);
            tekst = proces.Encrypt();
            return tekst;
        }
        private void DodajUJson()
        {
            var dodajKorisnika = new CitajPisiJson(_korisnik);
            dodajKorisnika.DodajKorisnikaUJson();
        }

        private string VratiPassword()
        {
            if (!string.IsNullOrWhiteSpace(TxtPassword.Text.Trim()))return TxtPassword.Text;
                MessageBox.Show("Niste ništa upisali u box password");
                return string.Empty;
        }

        private string VratiIme()
        {
            if (!string.IsNullOrWhiteSpace(TxtIme.Text.Trim())) return TxtIme.Text;
            MessageBox.Show("Niste ništa upisali u box Ime!");
            return string.Empty;
        }
    }
}
