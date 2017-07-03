using GetHealthyApp.DataModels;
using Microsoft.WindowsAzure.MobileServices;
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
    public partial class EnterWeight : ContentPage
    {
        public EnterWeight()
        {
            InitializeComponent();
            GetWeightRequest();
            GetHistoryRequest();
        }

        //Local variables
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;
        static List<double> weightHistoryList = new List<double>();
        static List<string> weightHistoryTimeList = new List<string>();
        static double difference = 0;

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

            btnTarget.BackgroundColor = Color.LightBlue;
            btnHistory.BackgroundColor = Color.LightBlue;
            
            //get information from database
            GetWeightRequest();
        }

        async void GetHistoryRequest()
        {
            List<Historydb> historyInformation = await AzureManager.AzureManagerInstance.GetHistoryInformation();
            lblHistory.Text = "";
            weightHistoryList.Clear();
            weightHistoryTimeList.Clear();
            FormatDateString(historyInformation);
            DisplayCurrentWeight();
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

        private static string AddLeadingZero(string num)
        {
            if (int.Parse(num) < 10)
            {
                num = "0" + num;
            }
            return num;
        }

        async void GetWeightRequest()
        {
            //getting information from database
            List<DataModels.EnterWeight> weightInformation = await AzureManager.AzureManagerInstance.GetWeightInformation();
            int count = weightInformation.Count;
            if(count == 0)
            {
                return; // first time user
            }
            DataModels.EnterWeight last = weightInformation[count - 1];

            lblCWeight.Text = last.CurrentWeight.ToString();
            lblTWeight.Text = last.TargetWeight.ToString();

            DisplayCurrentWeight();
        }

        private void BtnTargetClicked(object sender, EventArgs e)
        {
            Visibility();
            entryTargetWeight.IsVisible = true;
            dateTargetWeight.IsVisible = true;
            lblTargetDifference.IsVisible = true;
            btnAddTargetWeight.IsVisible = true;

            btnTarget.BackgroundColor = Color.Cyan;
            btnHistory.BackgroundColor = Color.LightBlue;
        }

        private void BtnHistoryClicked(object sender, EventArgs e)
        {
            Visibility();
            lblHistory.IsVisible = true;
            lblHistory.Text = "";
            DisplayHistory();
            btnHistory.BackgroundColor = Color.Cyan;
            btnTarget.BackgroundColor = Color.LightBlue;
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
            //DisplayCurrentWeight();
            await PostHistoryRequest();
            GetHistoryRequest();
            await UpdateWeightRequest();
            GetWeightRequest();
        }

        private void DisplayCurrentWeight()
        {
            lblCurrentWeight.Text = "";
            int count = weightHistoryList.Count;
            //if (count != 0 || weight != -1.0)   //if weight is -1.0, this could mean incorrect value entered 
            //                                    //or re opening tab (want to display the previous values)
            //{
            //    lblCurrentWeight.Text += "Weight: " + weightHistoryList[count - 1] + " Kg\nDate: " + weightHistoryTimeList[count - 1] + "\n\n";
            //}

            //if ((weightHistoryList.Count > 1) && (weight != -1.0))
            //{
            //    lblDifference.Text = "Difference:\n";
            //    difference = CalculateDifference(weight);
            //    lblDifference.Text += Math.Abs(difference) + " Kg";
            //}
            //else if ((weightHistoryList.Count > 1) || (weight == -1.0)) //ensure to display values that were previously entered
            //{
            //    lblDifference.Text = "Difference:\n";
            //    lblDifference.Text += Math.Abs(difference) + " Kg";
            //}
            if(count == 0)
            {
                return;
            }
            lblCurrentWeight.Text = "Weight: " + lblCWeight.Text + " Kg\nDate: " + weightHistoryTimeList[count - 1];

            lblDifference.Text = "Difference:\n";
            bool temp = double.TryParse(lblCWeight.Text, out double weight);
            if(temp)
            {
                difference = CalculateDifference(weight);
            }
            lblDifference.Text += Math.Abs(difference) + " Kg";
        }

        private double CalculateDifference(double weight)
        {
            //getting the last item in the list
            int count = weightHistoryList.Count();
            if(count < 2)
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

        async void BtnAddTargetWeightClicked(object sender, EventArgs e)
        {
            EnterTargetWeight();
            GetWeightRequest();
            await UpdateWeightRequest();
            GetWeightRequest();
        }

        private async Task PostHistoryRequest()
        {
            bool temp = float.TryParse(entryField.Text, out float c);
            if(temp)
            {
                Historydb model = new Historydb()
                {
                    CurrentWeight = c,
                    CurrentDate = DateTime.Now
                };
                await AzureManager.AzureManagerInstance.PostHistoryInformation(model);
            }
        }

        private async Task UpdateWeightRequest()
        {
            bool temp1 = float.TryParse(entryField.Text, out float cWeight);
            bool temp2 = float.TryParse(entryTargetWeight.Text, out float tWeight);
            DataModels.EnterWeight model;
            if (!temp1) //checking if valid input/or empty
            {
                //updating values in database
                model = new DataModels.EnterWeight()
                {
                    ID = "1",
                    CurrentWeight = float.Parse(lblCWeight.Text),
                    TargetWeight = tWeight,
                    TargetDate = dateTargetWeight.Date
                };
            }
            else if (!temp2) //checking if valid input/or empty
            {
                //updating values in database
                model = new DataModels.EnterWeight()
                {
                    ID = "1",
                    CurrentWeight = cWeight,
                    TargetWeight = float.Parse(lblTWeight.Text),
                    TargetDate = dateTargetWeight.Date
                };
            } else
            {
                model = new DataModels.EnterWeight()
                {
                    ID = "1",
                    CurrentWeight = cWeight,
                    TargetWeight = tWeight,
                    TargetDate = dateTargetWeight.Date
                };
            }

            try
            {
                await AzureManager.AzureManagerInstance.UpdateWeightInformation(model);
            } catch (ArgumentException)
            {
                lblCWeight.Text = "ID does not exist in Database";
            }
        }

        private void EnterTargetWeight()
        {
            double weight = CalculateTargetWeight();
            if ((weight > -1.0) && (GetDaysRemaining() > 7))
            {
                DisplayTargetWeight("lose", "weeks");
            } else if ((weight < -1.0) && (GetDaysRemaining() > 7))
            {
                DisplayTargetWeight("gain", "weeks");
            } else if ((weight > -1.0) && (GetDaysRemaining() < 7))
            {
                DisplayTargetWeight("lose", "days");
            } else if ((weight < -1.0) && (GetDaysRemaining() < 7))
            {
                DisplayTargetWeight("gain", "days");
            }
        }

        private void DisplayTargetWeight(string loseGain, string dayWeek)
        {
            double weeksRemaining;
            if (dayWeek == "weeks")
            {
                weeksRemaining = Math.Round((GetDaysRemaining() / 7) + 1);
            } else
            {
                weeksRemaining = GetDaysRemaining();
            }
             
            //display target goal statistics
            lblTargetDifference.Text = "To reach your target weight of " +
                entryTargetWeight.Text + " Kg\nYou must " + loseGain + " " +
                Math.Abs((Math.Round(CalculateTargetWeight() * 100) / 100)).ToString() + " Kg in " +
                Math.Round(weeksRemaining).ToString() + " " + dayWeek + "\n";
            
            //if less than 1 week to go
            if (GetDaysRemaining() > 7)
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
            //double weight = weightHistoryList[count - 1];
            double weight = weightHistoryList[0];

            bool temp = double.TryParse(entryTargetWeight.Text, out double targetWeight);

            if (temp)
            {
                return weight - targetWeight;
            }
            return -1.0;
        }

        private double GetDaysRemaining()
        {
            DateTime currentDate = DateTime.Now;
            DateTime targetDate = dateTargetWeight.Date;

            return (targetDate - currentDate).TotalDays;
        }

        private void DisplayHistory()
        {
            GetHistoryRequest();
        }
    }
}
