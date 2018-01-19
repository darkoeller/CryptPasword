using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using CryptLibrary;
using Newtonsoft.Json.Linq;

namespace CryptPasword.UC
{
    /// <summary>
    ///     Interaction logic for DodajObrisiKorisnika.xaml
    /// </summary>
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
        }
        private string VratiPassword => TxtPassword.Text.Trim();

        private string VratiIme => TxtIme.Text.Trim();

        private bool JelAdmin => IsAdmin.IsChecked == true;

        private void PopuniMrezu()
        {
            var rss = VratiObjekte(out IEnumerable<string> imena,
                out var passwordi);
            var admin = from a in rss["Korisnici"]
                select (bool) a["Admin"];

            var imenaArray = imena.Select(ime => new EncDecrypt(ime))
                .Select(desifrator => desifrator.Decrypt())
                .ToList();
            var paswordArray = passwordi.Select(password => new EncDecrypt(password))
                .Select(sifra => sifra.Decrypt())
                .ToList();
            var adminArray = admin.Select(ad => ad).ToList();
            var listaKorisnika = new List<Korisnici>();
            foreach (var ime in imenaArray)
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
            if (KorisiniciDataGrid.SelectedIndex == -1)
            {
                MessageBox.Show("Niste nikog odabrali za brisanje");
                return;
            }
            _korisnik = (Korisnici)KorisiniciDataGrid.SelectedItem ;
            var ime = KriptirajTekst(_korisnik.Ime);
            var osobe = IzvuciListuKorisnika();
            try
            {
            var zabrisati = osobe.SingleOrDefault(x => x.Ime == ime);
            osobe.Remove(zabrisati);
            CitajPisiJson.ObrisiKorisnika(osobe);
            PopuniMrezu();
            KorisiniciDataGrid.Items.Refresh();
            KorisiniciDataGrid.SelectedIndex = -1;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Niste nikog označili za brisanje!");
                //return;
            }
          
        }

        private static IList<Korisnici> IzvuciListuKorisnika()
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            var jarray = (JArray) rss["Korisnici"];
            var osobe = jarray.ToObject<IList<Korisnici>>();
            return osobe;
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
            _korisnik.Admin = JelAdmin;
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
                Admin = JelAdmin
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