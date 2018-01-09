using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CryptLibrary;
using Newtonsoft.Json;

namespace CryptPasword
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string VratiPassword()
        {
            return TxtPassword.Password;
        }

        private string VratiIme()
        {
            return TxtKorisnik.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var proces = new EncDecrypt(VratiPassword());
            var izlaz = proces.Encrypt();
            LblIzlaz.Content= izlaz;
            UpisiJson();

        }

        private void UpisiJson()
        {
            var korisnik = new Korisinici
            {
                Ime = VratiIme(),
                Password = VratiPassword()
            };
            using (StreamWriter file = File.AppendText(@"C:\Users\Darko\Documents\CryptPasword\Korisnici.json"))
                {
                   JsonSerializer serializer = new JsonSerializer();
                   serializer.Serialize(file, korisnik);
                }
        }

        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            var pass = new EncDecrypt(LblIzlaz.Content.ToString());
           LblIzlaz.Content = pass.Decrypt();

        }
    }
}
                                           