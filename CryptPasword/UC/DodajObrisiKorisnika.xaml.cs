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
            //KorisiniciDataGrid.ItemsSource = PopuniMrezu();
        }

        private void PopuniMrezu()
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");            
            var rss = JObject.Parse(jsonObject);
            var imena = from p in rss["Korisnici"]
                        select (string)p["Ime"];
            var passwordi = from p in rss["Korisnici"]
                            select (string)p["Password"];

            var imenaArray = imena.Select(ime => new EncDecrypt(ime)).Select(desifrator => desifrator.Decrypt()).ToList();
            var paswordArray = passwordi.Select(password => new EncDecrypt(password)).Select(sifra => sifra.Decrypt()).ToList();

            var listaKorisnika = new List<Korisnici>();
            foreach (var ime in imenaArray)
            {
                 foreach (var pass in paswordArray)
                 {
                     listaKorisnika.Add(new Korisnici {Ime = ime, Password = pass});
                     paswordArray.Remove(pass);
                     break;
                 }
            }
            KorisiniciDataGrid.ItemsSource = listaKorisnika;
        }


        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            _korisnik.Ime = KriptirajTekst(VratiIme());
            _korisnik.Password = KriptirajTekst(VratiPassword());
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
