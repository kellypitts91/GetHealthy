using GetHealthy.DataModels;
using Microsoft.WindowsAzure.MobileServices;
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
    public partial class EnterWeight : ContentPage
    {
        public EnterWeight()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            GetWeightRequest();
            GetHistoryRequest();
        }

        //Local variables
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;
        static List<double> weightHistoryList = new List<double>();
        static List<string> weightHistoryTimeList = new List<string>();
        static double difference = 0;

        //Navigation Menu
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
            Navigation.PushAsync(new FoodDiary());
        }

        private void BtnWeightClicked(object sender, EventArgs e)
        {
            Visibility();

            entryField.Placeholder = "Enter weight here";
            entryField.IsVisible = true;
            lblCurrentWeight.IsVisible = true;
            lblDifference.IsVisible = true;
            lblResult.IsVisible = true;
            btnAddWeight.IsVisible = true;

            btnTarget.BackgroundColor = Color.FromRgb(102, 181, 255);
            btnHistory.BackgroundColor = Color.FromRgb(102, 181, 255);

            //get information from database
            GetWeightRequest();
        }

        private void BtnTargetClicked(object sender, EventArgs e)
        {
            Visibility();
            entryTargetWeight.IsVisible = true;
            dateTargetWeight.IsVisible = true;
            lblTargetDifference.IsVisible = true;
            btnAddTargetWeight.IsVisible = true;

            btnTarget.BackgroundColor = Color.FromRgb(0, 132, 255);
            btnHistory.BackgroundColor = Color.FromRgb(102, 181, 255);
        }

        private void BtnHistoryClicked(object sender, EventArgs e)
        {
            Visibility();
            lblHistory.IsVisible = true;
            lblHistory.Text = "";
            DisplayHistory();
            btnHistory.BackgroundColor = Color.FromRgb(0, 132, 255);
            btnTarget.BackgroundColor = Color.FromRgb(102, 181, 255);
        }

        private void Visibility()
        {
            entryField.IsVisible = false;
            lblCurrentWeight.IsVisible = false;
            lblDifference.IsVisible = false;
            entryTargetWeight.IsVisible = false;
            dateTargetWeight.IsVisible = false;
            lblTargetDifference.IsVisible = false;
            btnAddTargetWeight.IsVisible = false;
            btnAddWeight.IsVisible = false;
            lblHistory.IsVisible = false;
            lblResult.IsVisible = false;
        }

        async void BtnAddWeightClicked(object sender, EventArgs e)
        {
            await PostHistoryRequest();
            GetHistoryRequest();
            await UpdateWeightRequest();
            GetWeightRequest();
        }

        async void BtnAddTargetWeightClicked(object sender, EventArgs e)
        {
            EnterTargetWeight();
            GetWeightRequest();
            await UpdateWeightRequest();
            GetWeightRequest();
        }

        //Get data from enterWeight database
        async void GetWeightRequest()
        {
            //getting information from database
            List<Weightdb> weightInformation = await AzureManager.AzureManagerInstance.GetWeightInformation();
            int count = weightInformation.Count;
            if (count == 0)
            {
                return; // first time user
            }
            Weightdb last = weightInformation[count - 1];

            //populating invisible labels
            lblCWeight.Text = last.CurrentWeight.ToString();
            lblTWeight.Text = last.TargetWeight.ToString();
            lblTDate.Text = last.TargetDate.ToString();

            DisplayCurrentWeight();
        }

        private async Task UpdateWeightRequest()
        {
            //trying to convert string to float
            bool resultCurrent = float.TryParse(entryField.Text, out float cWeight);
            bool resultTarget = float.TryParse(entryTargetWeight.Text, out float tWeight);
            Weightdb model;

            //first time user - this is to stop errors occuring
            //if weight label is null - no data in database
            if (IsNull(lblCWeight.Text))
            {
                lblCWeight.Text = "0";
            }

            //if target weight label is null - no data in database
            if (IsNull(lblTWeight.Text))
            {
                lblTWeight.Text = "0";
            }

            if (!resultCurrent) //checking if valid input/or empty
            {
                //updating values in database
                model = new Weightdb()
                {
                    ID = "1",
                    CurrentWeight = float.Parse(lblCWeight.Text), //user has not entered a value so grabing value from database
                    TargetWeight = tWeight,
                    TargetDate = dateTargetWeight.Date
                };
            }
            else if (!resultTarget) //checking if valid input/or empty
            {
                //updating values in database
                model = new Weightdb()
                {
                    ID = "1",
                    CurrentWeight = cWeight,
                    TargetWeight = float.Parse(lblTWeight.Text), //user has not entered a value so grabing value from database
                    TargetDate = dateTargetWeight.Date
                };
            }
            else
            {
                //updating values in database
                model = new Weightdb()
                {
                    ID = "1",
                    CurrentWeight = cWeight,
                    TargetWeight = tWeight,
                    TargetDate = dateTargetWeight.Date
                };
            }

            if (weightHistoryList.Count == 0)
            {
                //first time user
                await AzureManager.AzureManagerInstance.PostWeightInformation(model);
                return;
            }

            try
            {
                //update existing row
                await AzureManager.AzureManagerInstance.UpdateWeightInformation(model);
            }
            catch (ArgumentException)
            {
                lblResult.Text = "ID does not exist in Database";
            }
        }

        private void DisplayHistory()
        {
            GetHistoryRequest();
        }

        private async Task PostHistoryRequest()
        {
            bool result = float.TryParse(entryField.Text, out float c);
            if (result)
            {
                lblResult.Text = "";
                Historydb model = new Historydb()
                {
                    CurrentWeight = c,
                    CurrentDate = DateTime.Now
                };
                //inserting new row into history table
                await AzureManager.AzureManagerInstance.PostHistoryInformation(model);
            }
            else
            {
                //display message if invalid value entered
                lblResult.Text = "Error: Invalid number, please enter a valid number";
            }
        }

        //get data from history database
        async void GetHistoryRequest()
        {
            List<Historydb> historyInformation = await AzureManager.AzureManagerInstance.GetHistoryInformation();
            lblHistory.Text = "";
            //clearing the lists otherwize the list will have duplicates
            weightHistoryList.Clear();
            weightHistoryTimeList.Clear();
            FormatDateString(historyInformation);
            DisplayCurrentWeight();
            EnterTargetWeight();
        }

        private void FormatDateString(List<Historydb> historyInformation)
        {
            //going through the list and displaying the history of weight inputs
            //in descending order
            for (int i = historyInformation.Count - 1; i >= 0; i--)
            {
                lblHistory.Text += "Weight: " + historyInformation[i].CurrentWeight.ToString() + " Kg \t";
                weightHistoryList.Add(historyInformation[i].CurrentWeight);

                //formating date/time string
                string d = historyInformation[i].CurrentDate.Day.ToString();
                string m = historyInformation[i].CurrentDate.Month.ToString();
                string y = historyInformation[i].CurrentDate.Year.ToString();
                string hour = historyInformation[i].CurrentDate.Hour.ToString();
                string min = historyInformation[i].CurrentDate.Minute.ToString();

                //add leading zeros
                d = AddLeadingZero(d);
                m = AddLeadingZero(m);
                hour = AddLeadingZero(hour);
                min = AddLeadingZero(min);
                string day = d + "/" + m + "/" + y + " " + hour + ":" + min;

                lblHistory.Text += "Date: " + day + "\n\n";

                weightHistoryTimeList.Add(day); // dd/mm/yyyy hh:mm
            }
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

        private void DisplayCurrentWeight()
        {
            lblCurrentWeight.Text = "";
            int count = weightHistoryList.Count;

            //list is empty
            if (count == 0)
            {
                return;
            }

            //first time user
            if (IsNull(lblCWeight.Text))
            {
                return;
            }

            lblCurrentWeight.Text = "Weight: " + lblCWeight.Text + " Kg\nDate: " + weightHistoryTimeList[count - 1];

            lblDifference.Text = "Difference:\n";
            bool result = double.TryParse(lblCWeight.Text, out double weight);
            if (result)
            {
                difference = CalculateDifference(weight);
            }
            lblDifference.Text += Math.Abs(difference) + " Kg";
        }

        private double CalculateDifference(double weight)
        {
            //getting the last item in the list
            int count = weightHistoryList.Count();
            //return from method if less than 2 entries (can't calculate the difference if only 1 entry)
            if (count < 2)
            {
                return 0;
            }
            //checking if new weight entered is less or more than the last entry
            if (weight <= weightHistoryList[1])
            {
                //lost weight
                lblDifference.Text += "-";
                lblDifference.TextColor = Color.Green;
            }
            else
            {
                //gained weight
                lblDifference.Text += "+";
                lblDifference.TextColor = Color.Red;
            }
            //return the difference
            return weightHistoryList[1] - weight;
        }

        //check if string is null (used for labels/text fields)
        private bool IsNull(string text)
        {
            if (text == null)
            {
                return true;
            }
            return false;
        }

        //formating string to display target weight information
        private void EnterTargetWeight()
        {
            double weight = CalculateTargetWeight();
            double daysRemaining = GetDaysRemaining();
            if ((weight > -1.0) && (daysRemaining > 6))
            {
                DisplayTargetWeight("lose", "weeks");
            }
            else if ((weight < -1.0) && (daysRemaining > 6))
            {
                DisplayTargetWeight("gain", "weeks");
            }
            else if ((weight > -1.0) && (daysRemaining <= 6))
            {
                DisplayTargetWeight("lose", "days");
            }
            else if ((weight < -1.0) && (daysRemaining <= 6))
            {
                DisplayTargetWeight("gain", "days");
            }
        }

        private void DisplayTargetWeight(string loseGain, string dayWeek)
        {
            //first time user
            if (IsNull(lblTWeight.Text))
            {
                return;
            }
            double weeksRemaining;
            double daysRemaining = GetDaysRemaining();
            if (daysRemaining == -999.0) //high value to check for error
            {
                return;
            }

            if (dayWeek == "weeks")
            {
                weeksRemaining = Math.Round((daysRemaining / 7) + 1);
            }
            else
            {
                weeksRemaining = daysRemaining;
            }

            if (weeksRemaining <= 0)
            {
                lblTargetDifference.Text = "Please select a date greater than today";
                return;
            }

            //display target goal statistics
            lblTargetDifference.Text = "To reach your target weight of " +
                lblTWeight.Text + " Kg\nYou must " + loseGain + " " +
                Math.Abs((Math.Round(CalculateTargetWeight() * 100) / 100)).ToString() + " Kg in " +
                Math.Round(weeksRemaining).ToString() + " " + dayWeek + "\n";

            //if less than 1 week to go
            if (daysRemaining > 6)
            {
                lblTargetDifference.Text += "You must " + loseGain + " " +
                    Math.Abs((Math.Round((CalculateTargetWeight() / weeksRemaining) * 100) / 100)) + " Kg per week";
            }
        }

        private double CalculateTargetWeight()
        {
            int count = weightHistoryList.Count();
            //making sure list is not empty 
            if (count == 0)
            {
                lblTargetDifference.Text = "Please enter a current weight first.";
                return -1.0;
            }
            //get the most recent entry (list is in reverse order of when entered)
            double weight = weightHistoryList[0];

            bool result = double.TryParse(lblTWeight.Text, out double targetWeight);
            if (result)
            {
                //calculate difference
                return weight - targetWeight;
            }
            return -999.0; //error occured
        }

        //calculate difference between 2 dates to get the days between dates
        private double GetDaysRemaining()
        {
            DateTime currentDate = DateTime.Now;
            bool result = DateTime.TryParse(lblTDate.Text, out DateTime targetDate);
            //check valid date before calculating (could be empty)
            if (result)
            {
                return (targetDate - currentDate).TotalDays;
            }
            else
            {
                return -1.0;
            }
        }
    }
}
