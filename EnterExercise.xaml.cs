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
    public partial class EnterExercise : ContentPage
    {
        public EnterExercise()
        {
            InitializeComponent();
        }

        private void btnHomeClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
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
