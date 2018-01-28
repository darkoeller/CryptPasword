using System;
using System.Collections.Generic;
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
using CryptLogin.UC;

namespace CryptLogin
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


        private void BtnPrijava_Click(object sender, RoutedEventArgs e)
        {
            
            var ime =  VratiIme();
            var pass = VratiPasword();
        }



        private string VratiIme()
        {
            if (!string.IsNullOrWhiteSpace(TxtIme.Text))
            {
                string ime = TxtIme.Text.Trim();
                KriptirajTekst(ime);
                return ime;
            }
            MessageBox.Show("Upišite korisničko ime!");
            return string.Empty;
            
        }

        private string VratiPasword()
        {
            var pass = KriptirajTekst(TxtPass.Password.Trim());
           var teks= TextBox.NameProperty.ToString();
            return pass;

        }
        private static string KriptirajTekst(string tekst)
        {
            var proces = new EncDecrypt(tekst);
            tekst = proces.Encrypt();
            return tekst;
        }
        //provjeri da li ime i pasword postoje u jsonu
        //ako postoje dozvoli korisniku pristup
        //ako ne postoje ne puštaj ga vrati na početak
    }
}
