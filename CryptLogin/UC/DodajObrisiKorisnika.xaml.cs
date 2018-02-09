using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using CryptLibrary;
using Newtonsoft.Json.Linq;

namespace CryptLogin.UC
{
    public partial class DodajObrisiKorisnika
    {
        private Korisnici _korisnik = new Korisnici();

        public DodajObrisiKorisnika()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PopuniMrezu();
            TxtIme.Focus();
            CmbUloga.ItemsSource = Enum.GetNames(typeof(Uloge));
            CmbUloga.SelectedIndex = 0;
        }
        private string VratiPassword => TxtPassword.Text.Trim();

        private string VratiIme => TxtIme.Text.Trim();


        private void PopuniMrezu()
        {
             VratiObjekte(out var imena, out var passwordi, out var uloge);
            var imenaArray = imena.Select(ime => new EncDecrypt(ime))
                .Select(desifrator => desifrator.Decrypt())
                .ToList();
            var paswordArray = passwordi.Select(password => new EncDecrypt(password))
                .Select(sifra => sifra.Decrypt())
                .ToList();
            var ulogeArray = uloge.Select(ad => new EncDecrypt(ad.ToString())).Select(ad =>ad.Decrypt()).ToList();
            var listaKorisnika = new List<Korisnici>();
            foreach (var ime in imenaArray)
                foreach (var pass in paswordArray)
                {
                    foreach (var ul in ulogeArray)
                    {
                        listaKorisnika.Add(new Korisnici { Ime = ime, Password = pass,  Uloga = ul });
                        paswordArray.Remove(pass);
                        ulogeArray.Remove(ul);
                        break;
                    }
                    break;
                }
            KorisiniciDataGrid.ItemsSource = listaKorisnika;
        }

        private static void VratiObjekte(out IEnumerable<string> imena, out IEnumerable<string> passwordi, out IEnumerable<string> uloge)
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            imena = from p in rss["Korisnici"]
                select (string) p["Ime"];
            passwordi = from p in rss["Korisnici"]
                select (string) p["Password"];
            uloge = from a in rss["Korisnici"]
                select (string) a["Uloga"];
        }

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (KorisiniciDataGrid.SelectedIndex == -1)
            {
                MessageBox.Show("Niste nikog odabrali za brisanje!");
                return;
            }
            _korisnik = (Korisnici)KorisiniciDataGrid.SelectedItem ;
            var ime = KriptirajTekst(_korisnik.Ime);
            var osobe = IzvuciListuKorisnika();
            var zabrisati = osobe.FirstOrDefault(x => x.Ime == ime);
            osobe.Remove(zabrisati);
            CitajPisiJson.ObrisiKorisnika(osobe);
            PopuniMrezu();
            KorisiniciDataGrid.Items.Refresh();
            KorisiniciDataGrid.SelectedIndex = -1;
        }

        private static IList<Korisnici> IzvuciListuKorisnika()
        {
           var jsonObject = File.ReadAllText(@"Korisnici.json");
           var rss = JObject.Parse(jsonObject);
           var jarray = (JArray) rss["Korisnici"];
           IList<Korisnici> listaResults = jarray.Select(p => new Korisnici {Ime=(string) p["Ime"], Password=(string) p["Password"], Uloga=(string) p["Uloga"] }).ToList();
           return listaResults;
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtIme.Text.Trim()) || string.IsNullOrWhiteSpace(TxtPassword.Text.Trim()))
            {
                 MessageBox.Show("Upišite nešto u text boxove!");
                 TxtIme.Focus();
                 return;
            }
            if (!DaliPostoji()) return;
            KriptirajKorisnika();
            ProcesuirajKorisnika();
            OcistiKontrole();
        }

        private void KriptirajKorisnika()
        {
            _korisnik.Ime = KriptirajTekst(VratiIme);
            _korisnik.Password = KriptirajTekst(VratiPassword);
            _korisnik.Uloga = KriptirajTekst(VratiUlogu());
        }

        private string VratiUlogu()
        {
          return CmbUloga.SelectedItem.ToString();
        }

        private void OcistiKontrole()
        {
            TxtIme.Text = string.Empty;
            TxtPassword.Text = string.Empty;
            TxtIme.Focus();
        }

        private bool DaliPostoji()
        {
            var postoji = NoviKorisnik();
            if (!KorisiniciDataGrid.ItemsSource.Cast<object>().Contains(postoji)) return true;
            MessageBox.Show("Korisnik je već zadan.\nOdaberite drugo ime i, ili pasword!");
            OcistiKontrole();
            return false;
        }

        private Korisnici NoviKorisnik()
        {
            var noviKorisnik = new Korisnici
            {
                Ime = VratiIme,
                Password = VratiPassword,
                Uloga= VratiUlogu()
            };
            return noviKorisnik;
        }

        private void ProcesuirajKorisnika()
        {
            DodajUJson();
            PopuniMrezu();
            KorisiniciDataGrid.Items.Refresh();
        }

        private static string KriptirajTekst(string tekst)
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
    }
}