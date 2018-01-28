using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CryptLibrary
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Uloga
    {
        [EnumMember(Value = "Admin")]
        Admin = 1,
        [EnumMember(Value = "SuperKorisnik")]
        SuperKorisnik,
        [EnumMember(Value = "Korisnik")]
        Korisnik,
        [EnumMember(Value = "Razina1")]
        Razina1
    }
}