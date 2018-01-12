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
        private readonly Korisinici _korisnik = new Korisinici();

        public DodajObrisiKorisnika()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //PopuniMrezu();
            KorisiniciDataGrid.ItemsSource = PopuniMrezu();
        }

        private  List<Korisinici> PopuniMrezu()
        {
            var lista = new List<Korisinici>();
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            var imena = from p in rss["Korisnici"]
                select(string)p["Ime"];
            var passwordi = from p in rss["Korisnici"]
                select (string) p["Password"];

            foreach(var ime in imena)
            {
                var desifrator = new EncDecrypt(ime);
                var vraceno = desifrator.Encrypt();
                _korisnik.Ime = vraceno;
      
            }
            foreach (var password in passwordi)
            {
                var sifra = new EncDecrypt(password);
                var vracenasifra = sifra.Encrypt();
                _korisnik.Password = vracenasifra;
                 lista.Add(_korisnik);   
            }
            return lista;
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
