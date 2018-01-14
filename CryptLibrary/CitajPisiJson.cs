using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptLibrary
{
    public class CitajPisiJson
    {
        private readonly Korisnici _korisnik;

        public CitajPisiJson(Korisnici korisnik)
        {
            _korisnik = korisnik;
        }
        public void DodajKorisnikaUJson()
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            var item = (JArray)rss["Korisnici"];
            var itemToAdd = new JObject
            {
                ["Ime"] = _korisnik.Ime,
                ["Password"] = _korisnik.Password
            };
            item.Add(itemToAdd);
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
                    //yield return (new Korisnici { Ime = ime, Password = pass });
                    paswordArray.Remove(pass);
                    break;
                }
            }


            UpisiJsonFile(rss);
        }

        private static void UpisiJsonFile(JObject rss)
        {
            using (var file = File.CreateText(@"Korisnici.json"))
            {
                var serializer = new JsonSerializer {Formatting = Formatting.Indented};
                //serialize object directly into file stream
                serializer.Serialize(file, rss);
            }
        }
    }
}