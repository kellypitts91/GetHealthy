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
            About();
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

        private void About()
        {
            lblAbout.Text = "You will find everything you need to help you kick start your weight loss journey, right from your mobile device.\n"+
                "Keep track of your diet and your weight loss to date, see how much weight you need to lose or gain to reach your goal and convert Kilojoules to calories.";
        }
    }
}
