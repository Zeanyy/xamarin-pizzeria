using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Text.RegularExpressions;

namespace pizzeria.strony
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Formularz : ContentPage
    {
        public Formularz()
        {
            InitializeComponent();
        }

        private void sprawdzLitery(object sender, TextChangedEventArgs e)
        {
            var wzor = Regex.IsMatch(e.NewTextValue, "^[a-zA-Z]+$");

            if (e.NewTextValue.Length > 0)
                ((Entry)sender).Text = wzor ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
        }
        private void sprawdzCyfry(object sender, TextChangedEventArgs e)
        {
            var wzor = Regex.IsMatch(e.NewTextValue, "^[0-9]+$");

            if (e.NewTextValue.Length > 0)
                ((Entry)sender).Text = wzor ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
        }
        private void sprawdzEmail(object sender, TextChangedEventArgs e)
        {
            var wzor = Regex.IsMatch(e.NewTextValue, "^[A-Za-z0-9._%+-@]+$");

            if (e.NewTextValue.Length > 0)
                ((Entry)sender).Text = wzor ? e.NewTextValue : e.NewTextValue.Remove(e.NewTextValue.Length - 1);
        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            switch(((RadioButton)sender).Value.ToString())
            {
                case "naMiejscu":
                    naWynosForm.IsVisible = false;
                    break;
                case "dowoz":
                    naWynosForm.IsVisible = true;
                    break;
            }
        }
        private void sprawdzDane(object sender, EventArgs e)
        {
            bool jakisProblem = false;

            foreach(var element in mainForm.Children)
                element.BackgroundColor = Color.Transparent;
            foreach(var element in naWynosForm.Children)
                element.BackgroundColor = Color.Transparent;
            submit.BackgroundColor = Color.Gray;
            labelRegulamin.TextColor = Color.White;

            if (imie.Text == null || !Regex.IsMatch(imie.Text, "^[a-zA-Z]+$"))
            {
                imie.BackgroundColor = Color.Red;
                jakisProblem = true;
            }
            if (nazwisko.Text == null || !Regex.IsMatch(nazwisko.Text, "^[a-zA-Z]+$"))
            {
                nazwisko.BackgroundColor = Color.Red;
                jakisProblem = true;
            }
            if (numerTelefonu.Text == null || !Regex.IsMatch(numerTelefonu.Text, "^[0-9]{9}$"))
            {
                numerTelefonu.BackgroundColor = Color.Red;
                jakisProblem = true;
            }
            if (((RadioButton)radioButtons.Children[0]).IsChecked)
            {
                if (jakisProblem)
                    return;
                DisplayAlert("", $"Zamówienie dla {imie.Text} {nazwisko.Text} o wartości {String.Format("{0:0.00}", Koszyk.wartoscKoszyka)}zł zostało przyjęte.", "OK");
                return;
            }
            if (miejscowosc.SelectedIndex == -1)
            {
                miejscowosc.BackgroundColor = Color.Red;
                jakisProblem = true;
            }
            if (nrLokalu.Text == null || nrLokalu.Text == "")
            {
                nrLokalu.BackgroundColor = Color.Red;
                jakisProblem = true;
            }
            if (email.Text == null || !Regex.IsMatch(email.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))
            {
                email.BackgroundColor = Color.Red;
                jakisProblem = true;
            }
            if (switchRegulamin.IsToggled == false)
            {
                labelRegulamin.TextColor = Color.Red;
                jakisProblem = true;
            }
            if(((RadioButton)radioButtons.Children[1]).IsChecked && !jakisProblem)
            {
                DisplayAlert("", $"Zamówienie dla {imie.Text} {nazwisko.Text} o wartości {String.Format("{0:0.00}", Koszyk.wartoscKoszyka)}zł zostało przyjęte.", "OK");
                return;
            }
        }
    }
}