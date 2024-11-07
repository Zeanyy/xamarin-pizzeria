using System;
using System.Collections.Generic;
using System.Text;

namespace pizzeria
{
    public class SklepoBiekt
    {
        public string zdjecie { get; set; }
        public string nazwa { get; set; }
        public string opis { get; set; }
        public double cena { get; set; }

        public SklepoBiekt(string zdjecie, string nazwa, string opis, double cena)
        {
            this.zdjecie = zdjecie;
            this.nazwa = nazwa;
            this.opis = opis;
            this.cena = Math.Round(cena, 2);
        }
    }
}
