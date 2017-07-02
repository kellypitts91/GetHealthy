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
        }

        static List<string> datesEnteredList = new List<string>();
        static List<List<string>> foodList = new List<List<string>>();

        private void BtnHomeClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void BtnConverterClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CalorieConverter());
        }

        private void BtnFoodDiaryClicked(object sender, EventArgs e)
        {

        }

        private void BtnWeightClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnterWeight());
        }

        private void BtnSubmitClicked(object sender, EventArgs e)
        {
            int newDay = dateOfEntry.Date.Day;
            int newMonth = dateOfEntry.Date.Month;
            int newYear = dateOfEntry.Date.Year;

            string newDate = newDay.ToString() + "/" + newMonth.ToString() + "/" + newYear.ToString();

            if (datesEnteredList.Contains(newDate))
            {
                int pos = datesEnteredList.IndexOf(newDate);
                foodList[pos].Add(entryFood.Text);
            }
            else
            {
                datesEnteredList.Add(newDate);
                foodList.Add(new List<string> { entryFood.Text });
            }

            int newDay1 = dateView.Date.Day;
            int newMonth1 = dateView.Date.Month;
            int newYear1 = dateView.Date.Year;

            string newDate1 = newDay1.ToString() + "/" + newMonth1.ToString() + "/" + newYear1.ToString();

            Display(newDate1);
        }

        private void Display(string day)
        {
            //int count = 0;
            //foreach (List<string> item in foodList)
            //{
            //    lblDisplay.Text += datesEnteredList[count] + "\n";
            //    foreach (string item2 in item)
            //    {
            //        lblDisplay.Text += item2 + ", ";
            //    }
            //    lblDisplay.Text += "\n";
            //    count++;
            //}

            //working on this, getting some minor errors
            int count = -1;
            if (datesEnteredList.Contains(day))
            {
                count = datesEnteredList.IndexOf(day);
            }
            if (count != -1)
            {
                lblDisplay.Text = datesEnteredList[count] + "\n";
                foreach (List<string> item in foodList)
                {
                    lblDisplay.Text += item[count] + "\n";
                }
            }
            else
            {
                lblDisplay.Text = "There are no entries for " + day + ". Please select another date.";
            }
        }

        private void DateChanged(object sender, DateChangedEventArgs e)
        {
            int newDay = dateView.Date.Day;
            int newMonth = dateView.Date.Month;
            int newYear = dateView.Date.Year;

            string newDate = newDay.ToString() + "/" + newMonth.ToString() + "/" + newYear.ToString();

            //Display(newDate);
        }
    }
}
