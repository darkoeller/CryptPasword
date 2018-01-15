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
            UpisiJsonFile(rss);
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