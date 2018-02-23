using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace CryptLibrary
{
    public class UpitUJson
    {
        private readonly string _ime;
        private readonly string _password;

        public UpitUJson(string ime, string password)
        {
            _ime = ime;
            _password = password;
        }

        private static JObject VratiJObject()
        {
            using (var reader = File.OpenText(@"Korisnici.json"))
            {
                    var jsonObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                    return jsonObject;
            }
        }

        public bool JelDobarLogin()
        {
            var rss = VratiJObject();
            var ime = from m in rss["Korisnici"]
                select (string) m["Ime"];
            var password = from p in rss["Korisnici"]
                select (string) p["Password"];
            VratiUlogu();
            return ime.AsParallel().Contains(_ime) && password.Contains(_password);
        }

        public string VratiUlogu()
        {
            var rss = VratiJObject();
            var uloga = (from u in rss["Korisnici"]
                where _ime.Equals((string) u["Ime"])
                      select (string) u ["Uloga"]).First();
            return PronadjiUlogu(uloga);
        }

        private static string PronadjiUlogu(string act)
        {
           var decrypt = new EncDecrypt(act);
           var enc = decrypt.Decrypt();
            return enc;
        }
    }
}