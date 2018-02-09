using System;
using Newtonsoft.Json;

namespace CryptLibrary
{
    public class EnumStringCodeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var uloge = (Uloge)value;

            switch (uloge)
            {
                case Uloge.Admin:
                    writer.WriteValue("Admin");
                    break;

                case Uloge.SuperKorisnik:
                    writer.WriteValue("SuperKorisnik");
                    break;
                case Uloge.Korisnik:
                    writer.WriteValue("Korisnik");
                    break;
                case Uloge.Razina1:
                    writer.WriteValue("SuperKorisnik");
                    break;
                
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;

            return Enum.Parse(typeof(Uloge), enumString, true);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
        
    }
}