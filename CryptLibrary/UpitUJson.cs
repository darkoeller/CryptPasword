using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptLibrary
{
    public class UpitUJson
    {
        private readonly string _ime;
        private readonly string _password;

        public UpitUJson(string ime)
        {
            _ime = ime;
        }

        public UpitUJson(string ime, string password)
        {
            _ime = ime;
            _password = password;

        }

        public bool VratiUpit()
        {
            var rss = VratiJObject();
            var upit = from p in rss["Korisnici"]
                select (string) p["Ime"];
            return upit.Contains(_ime);
        }

        private static JObject VratiJObject()
        {
            var jsonObject = File.ReadAllText(@"Korisnici.json");
            var rss = JObject.Parse(jsonObject);
            return rss;
        }

        public bool JelDobarLogin()
        {
            var rss = VratiJObject();
            var ime = from m in rss["Korisnici"]
                select (string) m["Ime"];
            var password = from p in rss["Korisnici"]
                select (string) p["Password"];
            ProcitajJson();
            return ime.Contains(_ime) && password.Contains(_password);
        }

        public void ProcitajJson()
        {
            var ime = _ime;
            var rss = VratiJObject();
            var cor = (JArray)rss["Korisnici"];
            IList<Korisnici> listaResults = cor.Select(p => new Korisnici {
                Ime=(string) p["Ime"], 
                Password=(string) p["Password"], 
                Uloga=(string) p["Uloga"]}).ToList();

            var ul = from l in listaResults
                where ime.Contains(l.Ime)
                select  l.Uloga.ToString();
          
        }
    }
}