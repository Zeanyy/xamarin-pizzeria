using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace pizzeria.strony
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static List<SklepoBiekt> elements = new List<SklepoBiekt>()
        {
            new SklepoBiekt("MARGHERITA.jpg", "MARGARITA", "sos pomidorowy, ser, przyprawy", 25.00),
            new SklepoBiekt("PEPPERONI.jpg", "PEPPERONI", "kiełbasa pepperoni, sos pomidorowy, ser, przyprawy", 26.00),
            new SklepoBiekt("HAWAJSKA.jpg", "HAWAJSKA", "szynka, ananas, sos pomidorowy, ser, przyprawy", 29.00),
            new SklepoBiekt("SALAMI.jpg", "SALAMI", "kiełbasa pepperoni, sos pomidorowy, ser, przyprawy", 26.00),
            new SklepoBiekt("PRIMAVERA.jpg", "PRIMAVERA", "szynka, rukola, pomidory, sos pomidorowy, ser, przyprawy", 30.00)
        };

        public MainPage()
        {
            InitializeComponent();
            listViewMainPage.ItemsSource = elements;
        }

        private async void listViewMainPage_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SklepoBiekt a = (SklepoBiekt)listViewMainPage.SelectedItem;

            var ilosc = await DisplayPromptAsync($"{a.nazwa}", "podaj ilosc:", "Dodaj", "Anuluj", "", maxLength: 2, keyboard: Keyboard.Numeric, initialValue: "1");

            if (Convert.ToInt32(ilosc) <= 0 || ilosc == "")
                return;

            for (int i = 0; i < Koszyk.koszykElements.Count; i++)
            {
                if (Koszyk.koszykElements[i].nazwa == a.nazwa)
                {
                    Koszyk.koszykElements[i].ilosc += Convert.ToInt32(ilosc);
                    return;
                }
            }
            Koszyk.koszykElements.Add(new KoszykoBiekt("", a.nazwa, "", a.cena, Convert.ToInt32(ilosc)));
        }

        private void przeniesDoKoszyka(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Koszyk());
        }
    }
}