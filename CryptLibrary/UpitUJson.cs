﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

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
            return ime.Contains(_ime) && password.Contains(_password);
        }

        public string VratiUlogu()
        {
            var rss = VratiJObject();
            var uloga = from u in rss["Korisnici"]
                where _ime.Equals((string) u["Ime"])
                      select (string) u ["Uloga"];
            var act = uloga.ToArray()[0];
            var gamer = PronadjiUlogu(act);
            return gamer;
        }

        private string PronadjiUlogu(string act)
        {
           var decrypt = new EncDecrypt(act);
           var enc = decrypt.Decrypt();
            return enc;
        }
    }
}