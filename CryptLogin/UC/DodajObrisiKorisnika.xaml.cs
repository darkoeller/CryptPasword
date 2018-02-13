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
            var imenaArray = DekriptirajImenaArray(imena, passwordi, uloge, out var paswordArray, out var ulogeArray);
            var listaKorisnika = VratObrisanuiListuKorisnika(imenaArray, paswordArray, ulogeArray);
            KorisiniciDataGrid.ItemsSource = listaKorisnika;
        }

        private static IEnumerable<string> DekriptirajImenaArray(IEnumerable<string> imena, IEnumerable<string> passwordi, IEnumerable<string> uloge,
            out List<string> paswordArray, out List<string> ulogeArray)
        {
            var imenaArray = imena.Select(ime => new EncDecrypt(ime))
                .Select(desifrator => desifrator.Decrypt())
                .ToList();
            paswordArray = passwordi.Select(password => new EncDecrypt(password))
                .Select(sifra => sifra.Decrypt())
                .ToList();
            ulogeArray = uloge.Select(ad => new EncDecrypt(ad.ToString())).Select(ad => ad.Decrypt()).ToList();
            return imenaArray;
        }

        private static IEnumerable<Korisnici> VratObrisanuiListuKorisnika(IEnumerable<string> imenaArray, ICollection<string> paswordArray, ICollection<string> ulogeArray)
        {
            var listaKorisnika = new List<Korisnici>();
            foreach (var ime in imenaArray)
            foreach (var pass in paswordArray)
            {
                foreach (var ul in ulogeArray)
                {
                    listaKorisnika.Add(new Korisnici {Ime = ime, Password = pass, Uloga = ul});
                    paswordArray.Remove(pass);
                    ulogeArray.Remove(ul);
                    break;
                }
                break;
            }
            return listaKorisnika;
        }

        private static void VratiObjekte(out IEnumerable<string> imena, out IEnumerable<string> passwordi, out IEnumerable<string> uloge)
        {
            var rss = VratiJObject();
            imena = from p in rss["Korisnici"]
                select (string) p["Ime"];
            passwordi = from p in rss["Korisnici"]
                select (string) p["Password"];
            uloge = from a in rss["Korisnici"]
                select (string) a["Uloga"];
        }

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (KorisiniciDataGrid.SelectedIndex == -1 && KorisiniciDataGrid.Items.Count == 0)
            {
                MessageBox.Show("Niste nikog odabrali za brisanje\n ili je mreža prazna!");
                return;
            }
            BrisiIzMreze();
            PopuniMrezu();
            KorisiniciDataGrid.Items.Refresh();
            KorisiniciDataGrid.SelectedIndex = -1;
        }

        private void BrisiIzMreze()
        {
            _korisnik = (Korisnici) KorisiniciDataGrid.SelectedItem;
            var ime = KriptirajTekst(_korisnik.Ime);
            var osobe = IzvuciListuKorisnika();
            var zabrisati = osobe.FirstOrDefault(x => x.Ime == ime);
            osobe.Remove(zabrisati);
            CitajPisiJson.ObrisiKorisnika(osobe);
        }

        private static IList<Korisnici> IzvuciListuKorisnika()
        {
           var rss = VratiJObject();
            var jarray = (JArray) rss["Korisnici"];
           IList<Korisnici> listaResults = jarray.Select(p => new Korisnici {Ime=(string) p["Ime"], Password=(string) p["Password"], Uloga=(string) p["Uloga"]}).ToList();
           return listaResults;
        }

        private static JObject VratiJObject()
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            return rss;
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