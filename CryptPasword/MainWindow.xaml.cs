﻿using System.Windows;
using CryptLibrary;

namespace CryptPasword
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Korisnici _korisnik;

        public MainWindow()
        {
            InitializeComponent();
            _korisnik = new Korisnici();
        }

        private string VratiPassword()
        {
            if (string.IsNullOrWhiteSpace(TxtPassword.Password)) MessageBox.Show("Niste ništa upisali");
            return TxtPassword.Password;
        }

        private string VratiIme()
        {
            if (!string.IsNullOrWhiteSpace(TxtKorisnik.Text)) return TxtKorisnik.Text;
            MessageBox.Show("Niste ništa upisali");
            return string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            KriptirajPassword();
            ProvjeriUJson(_korisnik);
            DodajUJson();
            LblIzlaz.Content = _korisnik.Password;
            //DodajKorisnikaUJson();
        }

        private bool ProvjeriUJson(Korisnici korisnik)
        {
            var upit = new UpitUJson(_korisnik.Ime);
            var dobro = upit.VratiUpit();
            if (dobro)
                MessageBox.Show("Korisnik se nalazi u bazi");

            return false;
        }


        private void KriptirajPassword()
        {
            var proces = new EncDecrypt(VratiPassword());
            _korisnik.Password = proces.Encrypt();
            _korisnik.Ime = VratiIme();
        }

        private void DodajUJson()
        {
            var dodajKorisnika = new CitajPisiJson(_korisnik);
            dodajKorisnika.DodajKorisnikaUJson();
        }


        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            var pass = new EncDecrypt(LblIzlaz.Content.ToString());
            LblIzlaz.Content = pass.Decrypt();
        }
    }
}