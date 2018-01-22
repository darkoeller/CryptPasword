using System.Collections.Generic;
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

        public CitajPisiJson()
        {
            ;
        }

        public void DodajKorisnikaUJson()
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            var item = (JArray) rss["Korisnici"];
            var itemToAdd = new JObject
            {
                ["Ime"] = _korisnik.Ime,
                ["Password"] = _korisnik.Password,
                ["Uloga"] = _korisnik.Uloga
            };
            item.Add(itemToAdd);
            UpisiJsonFile(rss);
        }

        public static void ObrisiKorisnika(IList<Korisnici> osoba)
        {
            var jsonObject = new JObject(new JProperty("Korisnici",
                new JArray(from k in osoba
                    select new JObject(new JProperty("Ime", k.Ime),
                        new JProperty("Password", k.Password), new JProperty("Admin", k.Uloga)))));
            UpisiJsonFile(jsonObject);
        }


        private static void UpisiJsonFile(JObject rss)
        {
            using (var file = File.CreateText(@"Korisnici.json"))
            {
                var serializer = new JsonSerializer {Formatting = Formatting.Indented};
                serializer.Serialize(file, rss);
            }
        }
    }
}