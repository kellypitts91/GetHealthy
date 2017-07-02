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
        }

        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;
        async void GetWeightInformationClicked(object sender, EventArgs e)
        {
            List<DataModels.EnterWeight> weightInformation = await AzureManager.AzureManagerInstance.GetWeightInformation();
            enterWeightList.ItemsSource = weightInformation;
        }
        
        async void PostWeightInformationClicked(object sender, EventArgs e)
        {
            DataModels.EnterWeight model = new DataModels.EnterWeight()
            {
                currentWeight = float.Parse(entryField.Text),
                targetWeight = float.Parse(entryTargetWeight.Text)
            };
            await AzureManager.AzureManagerInstance.PostWeightInformation(model);
        }

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

        private void BtnAddWeightClicked(object sender, EventArgs e)
        {
            DisplayCurrentWeight();
        }

        private void DisplayCurrentWeight()
        {
            double weight = FillList();

            lblCurrentWeight.Text = "";
            int count = weightHistoryList.Count;
            if (count != 0 || weight != -1.0)   //if weight is -1.0, this could mean incorrect value entered 
                                                //or re opening tab (want to display the previous values)
            {
                lblCurrentWeight.Text += "Weight: " + weightHistoryList[count - 1] + " Kg\nDate: " + weightHistoryTimeList[count - 1] + "\n\n";
            }

            if ((weightHistoryList.Count > 1) && (weight != -1.0))
            {
                lblDifference.Text = "Difference:\n";
                difference = CalculateDifference(weight);
                lblDifference.Text += Math.Abs(difference) + " Kg";
            }
            else if ((weightHistoryList.Count > 1) || (weight == -1.0)) //ensure to display values that were previously entered
            {
                lblDifference.Text = "Difference:\n";
                lblDifference.Text += Math.Abs(difference) + " Kg";
            }
        }

        private double FillList()
        {
            //ensures only 10 previous entries are displayed in history
            if (weightHistoryList.Count == 10)
            {
                //remove first item from list
                weightHistoryList.RemoveAt(0);
                weightHistoryTimeList.RemoveAt(0);
            }

            bool temp = double.TryParse(entryField.Text, out double weight);

            //checking that the user entered a correct value
            if (temp)
            {
                weightHistoryList.Add(weight);
                weightHistoryTimeList.Add(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                return weight;
            }
            else
            {
                //return -1.0 if user did not enter a correct value
                return -1.0;
            }
        }

        private double CalculateDifference(double weight)
        {
            //getting the last item in the list
            int count = weightHistoryList.Count();

            //checking if new weight entered is less or more than the last entry
            if (weight <= weightHistoryList[count - 2])
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
            return weightHistoryList[count - 2] - weight;
        }

        private void BtnAddTargetWeightClicked(object sender, EventArgs e)
        {
            double weeksRemaining = Math.Round((GetDaysRemaining() / 7) + 1);
            double kgPerWeek;
            if (CalculateTargetWeight() != -1.0)
            {
                //if less than 1 week to go
                if (GetDaysRemaining() > 7)
                {
                    kgPerWeek = Math.Round((CalculateTargetWeight() / weeksRemaining) * 100) / 100;
                    //display target goal statistics
                    lblTargetDifference.Text = "To reach your target weight of " +
                        entryTargetWeight.Text + " Kg\nYou must lose " +
                        (Math.Round(CalculateTargetWeight() * 100) / 100).ToString() + " Kg in " +
                        Math.Round(weeksRemaining).ToString() + " weeks\nYou must lose " +
                        kgPerWeek + " Kg per week";
                }
                else
                {
                    kgPerWeek = Math.Round((CalculateTargetWeight()) * 100) / 100;
                    //display target goal statistics
                    lblTargetDifference.Text = "To reach your target weight of " +
                        entryTargetWeight.Text + " Kg\nYou must lose " +
                        Math.Round(CalculateTargetWeight()).ToString() + " Kg in " +
                        Math.Round(GetDaysRemaining()).ToString() + " days";
                }
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
            double weight = weightHistoryList[count - 1];

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
            //going through the list and displaying the history of weight inputs
            //in descending order
            for (int i = weightHistoryList.Count; i > 0; i--)
            {
                lblHistory.Text += "Weight: " + weightHistoryList[i - 1] + " Kg \t Date: " + weightHistoryTimeList[i - 1] + "\n\n";
            }
        }
    }
}
