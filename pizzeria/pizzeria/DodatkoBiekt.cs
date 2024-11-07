using System;
using System.Collections.Generic;
using System.Text;

namespace pizzeria
{
    public class DodatkoBiekt
    {
        public string nazwa { get; set; }
        public double cena { get; set; }
        public int ilosc { get; set; }
        public bool stan { get; set; }
        public DodatkoBiekt(string nazwa, double cena, int ilosc=0, bool stan=false)
        {
            this.nazwa = nazwa;
            this.cena = Math.Round(cena, 2);
            this.ilosc = ilosc;
            this.stan = stan;
        }
    }
}
