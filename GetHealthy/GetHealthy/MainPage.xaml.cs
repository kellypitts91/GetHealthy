using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GetHealthy
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnHomeClicked(object sender, EventArgs e)
        {

        }

        private void btnConverterClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CalorieConverter());
        }

        private void btnFoodDiaryClicked(object sender, EventArgs e)
        {

        }

        private void btnExerciseClicked(object sender, EventArgs e)
        {

        }

        private void btnWeightClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnterWeight());
        }
    }
}
