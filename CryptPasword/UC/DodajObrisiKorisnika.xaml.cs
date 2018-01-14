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
            //PopuniMrezu();
            KorisiniciDataGrid.ItemsSource = PopuniMrezu();
        }

        private static IEnumerable<Korisnici> PopuniMrezu()
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            var imena = from p in rss["Korisnici"]
                        select (string)p["Ime"];
            var passwordi = from p in rss["Korisnici"]
                            select (string)p["Password"];

            var imenaArray = imena.Select(ime => new EncDecrypt(ime)).Select(desifrator => desifrator.Encrypt()).ToList();
            var paswordArray = passwordi.Select(password => new EncDecrypt(password)).Select(sifra => sifra.Encrypt()).ToList();

            foreach (var ime in imenaArray)
            {
                 foreach (var pass in paswordArray)
                 {
                    yield return (new Korisnici{Ime = ime, Password = pass});
                     paswordArray.Remove(pass);
                     break;
                 }
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
            dodajKorisnika.DodajKorisnikaUJson();
           
        }

        private string VratiPassword()
        {
            if (!string.IsNullOrWhiteSpace(TxtPassword.Password))return TxtPassword.Password;
                MessageBox.Show("Niste ništa upisali");
                return string.Empty;
        }

        private string VratiIme()
        {
            if (!string.IsNullOrWhiteSpace(TxtIme.Text)) return TxtIme.Text;
            MessageBox.Show("Niste ništa upisali");
            return string.Empty;

        }

        /*Dodati u json
         * pročitaj fajl
         * pročitaj textbox
         * dodaj objekt
         * kriptiraj sve 
         * spremi u json
         */
    }
}
