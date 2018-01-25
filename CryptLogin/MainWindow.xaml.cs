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
        private string _varijabla = string.Empty;

        public MainWindow()
        {
          
            InitializeComponent();
        }


        private void BtnPrijava_Click(object sender, RoutedEventArgs e)
        {
            VratiKorisnika();
            VratiPasword();
        }

        private string VratiKorisnika()
        {
            var ime =KriptirajTekst(TxtIme.Text.Trim());
            return ime;
        }

        private void VratiPasword()
        {
            
        }
        private static string KriptirajTekst(string tekst)
        {
            var proces = new EncDecrypt(tekst);
            tekst = proces.Encrypt();
            return tekst;
        }
    }
}
