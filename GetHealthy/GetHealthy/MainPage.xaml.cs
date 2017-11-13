using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using GetHealthy.Model;

namespace GetHealthy
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            About();
        }

        //Navigation Menu
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

        //camera can only be navigated to from the home screen -- provides less clutter on other pages
        private void BtnCameraClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Camera());
        }

        //little blerb explaining what the app is about
        private void About()
        {
            lblAbout.Text = "You will find everything you need to help you kick start your weight loss journey, right from your mobile device.\n" +
                "Keep track of your diet and your weight loss to date, see how much weight you need to lose or gain to reach your goal and convert Kilojoules to calories.";
        }

        
    }
}
