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

        private void btnHomeClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void btnConverterClicked(object sender, EventArgs e)
        {
            lblCalorieResult.Text = "";
        }

        private void btnFoodDiaryClicked(object sender, EventArgs e)
        {

        }

        private void btnExerciseClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnterExercise());
        }

        private void btnWeightClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnterWeight());
        }

        private void btnCalculateClicked(object sender, EventArgs e)
        {
            double kj;
            bool temp = double.TryParse(entryField.Text, out kj);
            if (temp)
            {
                lblCalorieResult.Text = entryField.Text + "Kj = " + (Math.Round(calculateColorie(kj) * 100)/100).ToString() + " KiloCalories";
            }
            else
            {
                lblCalorieResult.Text = "please enter a number";
            }
        }

        private double calculateColorie(double kj)
        {
            return kj * 0.239006;
        }
    }
}
