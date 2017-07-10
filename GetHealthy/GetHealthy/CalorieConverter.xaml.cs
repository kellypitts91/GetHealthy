using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GetHealthy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalorieConverter : ContentPage
    {
        public CalorieConverter()
        {
            InitializeComponent();
        }

        //Navigation Menu
        private void BtnHomeClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void BtnConverterClicked(object sender, EventArgs e)
        {
            lblCalorieResult.Text = "";
        }

        private void BtnFoodDiaryClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FoodDiary());
        }

        private void BtnWeightClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnterWeight());
        }

        //Display the result in a label
        private void BtnCalculateClicked(object sender, EventArgs e)
        {
            //checking if user has entered a correct value (number)
            bool temp = double.TryParse(entryField.Text, out double kj);
            if (temp)
            {
                //display result with 2 decimal places
                lblCalorieResult.Text = entryField.Text + " Kj = " + (Math.Round(CalculateColorie(kj) * 100) / 100).ToString() + " KiloCalories";
            }
            else
            {
                lblCalorieResult.Text = "please enter a number";
            }
        }

        //Calculate the conversion from KJ to calories
        private double CalculateColorie(double kj)
        {
            return kj * 0.239006;
        }
    }
}
