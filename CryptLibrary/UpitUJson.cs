﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return ime.Contains(_ime) && password.Contains(_password);
        }
    }
}