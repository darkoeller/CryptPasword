namespace CryptLibrary
{
    public class VratiKorisnika
    {
        private readonly string _ime;

        public VratiKorisnika(string ime)
        {
            _ime = ime;
        }

        public Korisnici Korisnik()
        {
            return new Korisnici();
        }

    }
}