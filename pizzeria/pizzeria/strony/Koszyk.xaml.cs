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
    public partial class Koszyk : ContentPage
    {
        public static List<KoszykoBiekt> koszykElements = new List<KoszykoBiekt>();
        public static List<DodatkoBiekt> dodatkoElements = new List<DodatkoBiekt>()
        {
            new DodatkoBiekt("ketchup", 2.00),
            new DodatkoBiekt("czosntkowy", 2.00),
            new DodatkoBiekt("majonez", 2.00),
            new DodatkoBiekt("musztarda", 2.20),
            new DodatkoBiekt("podwójne ciasto", 3.00),
            new DodatkoBiekt("podwójny ser", 4.00)
        };
        public static List<DodatkoBiekt> selectedDodatkoElements = new List<DodatkoBiekt>();
        public static List<DodatkoBiekt> tmp = new List<DodatkoBiekt>();

        public static double wartoscKoszyka = 0;

        public DodatkoBiekt compareElements(DodatkoBiekt e, List<DodatkoBiekt> sdb)
        {
            foreach (DodatkoBiekt element in sdb)
                if (element.nazwa == e.nazwa)
                    return element; 
            return e;
        }
        public Koszyk()
        {
            tmp.RemoveAll(r => r.nazwa != null);

            InitializeComponent();

            listViewKoszyk.ItemsSource = koszykElements;

            foreach(DodatkoBiekt element in dodatkoElements)
            {
                if (compareElements(element, selectedDodatkoElements) == element)
                {
                    tmp.Add(element);
                    continue;
                }
                tmp.Add(compareElements(element, selectedDodatkoElements));
            }
            listViewDodatki.ItemsSource = tmp;

            resetListView<DodatkoBiekt>(listViewDodatki, dodatkoElements);        }
        private async void usunElement(object sender, EventArgs e)
        {
            bool potwierdz = await DisplayAlert("", "Czy chcesz usunąć element?", "Tak", "Nie");
            if (!potwierdz)
                return;
            var childrens = ((Grid)((Button)sender).Parent).Children;
            var nazwaElementu = ((Label)childrens[0]).Text;
            for (int i = 0; i < koszykElements.Count; i++)
            {
                if (koszykElements[i].nazwa != nazwaElementu)
                    continue;
                koszykElements.RemoveAt(i);

                resetListView<KoszykoBiekt>(listViewKoszyk, koszykElements);
            }
        }
        private async void usunKoszyk(object sender, EventArgs e)
        {
            bool potwierdz = await DisplayAlert("", "Czy chcesz wyczyścić koszyk?", "Tak", "Nie");
            if (!potwierdz)
                return;
            koszykElements.RemoveAll(r => r.nazwa != null);
            selectedDodatkoElements.RemoveAll(r => r.nazwa != null);
            foreach (var element in dodatkoElements)
                element.stan = false;
            resetListView<DodatkoBiekt>(listViewDodatki, dodatkoElements);
            resetListView<KoszykoBiekt>(listViewKoszyk, koszykElements);
        }

        private void wybranieDodatku(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            Grid grid = (Grid)checkbox.Parent;

            var nazwa = (Label)grid.Children[1];
            var cena = (Label)grid.Children[2];
            var ilosc = (Label)grid.Children[4];
            var stepper = (Stepper)grid.Children[5];

            if (checkbox.IsChecked == true)
            {
                ilosc.IsVisible = true;
                ilosc.Text = "1";
                stepper.IsVisible = true;
                stepper.Value = 1;
                selectedDodatkoElements.Add(new DodatkoBiekt(nazwa.Text, Convert.ToDouble(cena.Text), Convert.ToInt32(ilosc.Text), true));
                obliczWartoscKoszyka();
                return;
            }
            ilosc.IsVisible = false;
            ilosc.Text = "0";
            stepper.IsVisible = false;
            stepper.Value = 0;

            selectedDodatkoElements.RemoveAll(r => r.nazwa == nazwa.Text || r.nazwa == null);

            obliczWartoscKoszyka();
        }

        private void zmienIloscKoszyk(object sender, ValueChangedEventArgs e)
        {
            var childrens = ((Grid)((Stepper)sender).Parent).Children;
            var nazwaElementu = ((Label)childrens[0]).Text;
            int index = -1;
            for (int i = 0; i < koszykElements.Count; i++)
            {
                if (koszykElements[i].nazwa != nazwaElementu)
                    continue;
                index = i;
            }
            if (index == -1)
                return;
            if (e.NewValue <= 0)
            {
                koszykElements.RemoveAt(index);
                resetListView<KoszykoBiekt>(listViewKoszyk, koszykElements);
                return;
            }
            ((Label)childrens[3]).Text = e.NewValue.ToString();
            koszykElements[index].ilosc = Convert.ToInt32(e.NewValue);

            obliczWartoscKoszyka();
        }
        private void zmienIloscDodatki(object sender, ValueChangedEventArgs e)
        {
            var childrens = ((Grid)((Stepper)sender).Parent).Children;
            var checkbox = (CheckBox)childrens[0];
            if (e.NewValue <= 0)
            {
                checkbox.IsChecked = false;
                return;
            }

            var labelNazwa = (Label)childrens[1];
            var labelIlosc = (Label)childrens[4];
            for (int i = 0; i < selectedDodatkoElements.Count; i++)
            {
                if (selectedDodatkoElements[i].nazwa != labelNazwa.Text)
                    continue;
                selectedDodatkoElements[i].ilosc = Convert.ToInt32(e.NewValue);
                labelIlosc.Text = e.NewValue.ToString();
                obliczWartoscKoszyka();
            }
        }

        public void przeniesDoFormularz(object sender, EventArgs e)
        {
            if (wartoscKoszyka == 0 || koszykElements.Count == 0)
            {
                DisplayAlert("UWAGA", "Koszyk jest pusty", "oK");
                return;
            }
            Navigation.PushAsync(new Formularz());
        }

        public void resetListView<myType>(ListView lv, List<myType> e)
        {
            lv.ItemsSource = null;
            lv.ItemsSource = e;
            obliczWartoscKoszyka();
        }

        public void obliczWartoscKoszyka()
        {
            wartoscKoszyka = 0;
            foreach (var element in koszykElements)
                wartoscKoszyka += Math.Round(Convert.ToDouble(element.ilosc) * Convert.ToDouble(element.cena), 2);
            foreach (var element in selectedDodatkoElements)
                wartoscKoszyka += Math.Round(Convert.ToDouble(element.ilosc) * Convert.ToDouble(element.cena), 2);
            labelWartosKoszyka.Text = $"Wartość zamówienia: {String.Format("{0:0.00}", wartoscKoszyka)} zł";
        }
    }
}