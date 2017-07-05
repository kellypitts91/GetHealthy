using GetHealthyApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GetHealthyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodDiary : ContentPage
    {
        public FoodDiary()
        {
            InitializeComponent();
            GetFoodDiaryRequest();
        }

        //static List<string> foodList = new List<string>();
        //static List<DateTime> dateList = new List<DateTime>();

        private void BtnHomeClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void BtnConverterClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CalorieConverter());
        }

        private void BtnWeightClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnterWeight());
        }

        async void BtnSubmitClicked(object sender, EventArgs e)
        {
            await PostFoodDiaryRequest();
            GetFoodDiaryRequest();
            entryFood.Text = "";
            entryFood.Placeholder = "Enter food item here";
        }

        private string ConvertDateToString(DateTime date)
        {
            string d = date.Day.ToString();
            string m = date.Month.ToString();
            string y = date.Year.ToString();

            //add leading zeros
            d = AddLeadingZero(d);
            m = AddLeadingZero(m);
            return (d + "/" + m + "/" + y);
        }

        //Add leading zero to number to format date and time string
        private static string AddLeadingZero(string num)
        {
            if (int.Parse(num) < 10)
            {
                num = "0" + num;
            }
            return num;
        }

        private void DateChanged(object sender, DateChangedEventArgs e)
        {
            GetFoodDiaryRequest();
        }

        async void GetFoodDiaryRequest()
        {
            List<FoodDiarydb> foodDiaryInformation = await AzureManager.AzureManagerInstance.GetFoodDiaryInformation();

            bool match = false;
            string day = ConvertDateToString(dateView.Date);
            lblError.Text = "Date: " + day + "\n\n";
            lblDisplay1.Text = "";
            lblDisplay2.Text = "";
            int count = 0;
            foreach (var item in foodDiaryInformation)
            {
                if (dateView.Date == item.DateOfEntry.Date)
                {
                    //Spliting text into 2 columns
                    if(count < 7)
                    {
                        lblDisplay1.Text += item.FoodItem + "\n";
                    } else
                    {
                        lblDisplay2.Text += item.FoodItem + "\n";
                    }
                    match = true;
                    count++;
                }
            }
            if (!match)
            {
                lblError.Text = "There are no entries for " + day + ". Please select another date.";
            }
        }

        private async Task PostFoodDiaryRequest()
        {
            //Error checking
            if(entryFood.Text == null || dateOfEntry.Date == null)
            {
                return;
            }

            FoodDiarydb model = new FoodDiarydb()
            {
                FoodItem = entryFood.Text,
                DateOfEntry = dateOfEntry.Date
            };
            await AzureManager.AzureManagerInstance.PostFoodDiaryInformation(model);
        }
    }
}
