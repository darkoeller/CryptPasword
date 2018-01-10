using System;
using Newtonsoft.Json;

namespace CryptLibrary
{
    [Serializable]
  public class Korisinici
    {
        [JsonProperty("Ime")]
        public string Ime { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
    }

}