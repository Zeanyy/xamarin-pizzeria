using System;
using System.Collections.Generic;
using System.Text;

namespace pizzeria
{
    public class KoszykoBiekt : SklepoBiekt
    {
        public int ilosc { get; set; }
        public KoszykoBiekt(string zdjecie, string nazwa, string opis, double cena, int ilosc) : base(zdjecie, nazwa, opis, cena)
        {
            this.ilosc = ilosc;
        }
    }
}
