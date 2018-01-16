﻿using System;
using Newtonsoft.Json;

namespace CryptLibrary
{
    [Serializable]
  public class Korisnici
    {
        [JsonProperty("Ime")]
        public string Ime { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
        [JsonProperty("Admin")]
        public bool Admin { get; set; }
    }

}