using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GetHealthyApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void BtnConverterClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CalorieConverter());
        }

        private void BtnFoodDiaryClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FoodDiary());
        }

        private void BtnWeightClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnterWeight());
        }
    }
}
