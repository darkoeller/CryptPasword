using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CryptLibrary
{
    [Serializable]
  public class Korisnici : IEquatable<Korisnici>
    {
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Ime != null ? Ime.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Password != null ? Password.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Admin.GetHashCode();
                return hashCode;
            }
        }

        [JsonProperty("Ime")]
        public string Ime { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
        [JsonProperty("Admin")]
        public bool Admin { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Korisnici);
        }

        public bool Equals(Korisnici other)
        {
            return other != null &&
                   Ime == other.Ime &&
                   Password == other.Password &&
                   Admin == other.Admin;
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