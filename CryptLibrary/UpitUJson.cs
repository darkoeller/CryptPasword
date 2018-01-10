using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CryptLibrary
{
    public class UpitUJson
    {
        private readonly string _ime;

        public UpitUJson(string ime)
        {
            _ime = ime;
        }
        public bool VratiUpit()
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            //var item = (JArray)rss["Korisnici"];
            var upit = from p in rss["Korisnici"]
                select(string)p["Ime"];
            if (upit.Contains(_ime)) return true;
            return false;
        }
    }
}