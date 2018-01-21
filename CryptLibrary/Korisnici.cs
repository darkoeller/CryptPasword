using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptLibrary
{
    [Serializable]
    public class Korisnici : IEquatable<Korisnici>
    {
        [JsonProperty("Ime")]
        public string Ime { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Uloga")]
        public JToken Uloga { get; set; }

        public bool Equals(Korisnici other)
        {
            return other != null &&
                   Ime == other.Ime &&
                   Password == other.Password;
            //&& Admin == other.Admin;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Ime != null ? Ime.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Password != null ? Password.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Uloga.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Korisnici);
        }

        public static bool operator ==(Korisnici korisnici1, Korisnici korisnici2)
        {
            return EqualityComparer<Korisnici>.Default.Equals(korisnici1, korisnici2);
        }

        public static bool operator !=(Korisnici korisnici1, Korisnici korisnici2)
        {
            return !(korisnici1 == korisnici2);
        }
    }
}