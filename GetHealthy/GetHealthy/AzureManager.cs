using GetHealthy.DataModels;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHealthy
{
    class AzureManager
    {
        //private variables
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<Weightdb> enterWeightTable;
        private IMobileServiceTable<Historydb> historyTable;
        private IMobileServiceTable<FoodDiarydb> foodDiaryTable;

        private AzureManager()
        {
            //initializing variables
            this.client = new MobileServiceClient("http://gethealthy.azurewebsites.net");
            this.enterWeightTable = this.client.GetTable<Weightdb>();
            this.historyTable = this.client.GetTable<Historydb>();
            this.foodDiaryTable = this.client.GetTable<FoodDiarydb>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        //allowing AzureManager to be a singleton - can have only 1 instance
        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }
        //Weight Information ------------------------------------------------
        public async Task<List<Weightdb>> GetWeightInformation()
        {
            return await this.enterWeightTable.ToListAsync();
        }

        public async Task PostWeightInformation(Weightdb enterWeight)
        {
            await this.enterWeightTable.InsertAsync(enterWeight);
        }

        public async Task UpdateWeightInformation(Weightdb enterWeight)
        {
            await this.enterWeightTable.UpdateAsync(enterWeight);
        }
        //End of Weight Information -----------------------------------------

        //History Information -----------------------------------------------
        public async Task<List<Historydb>> GetHistoryInformation()
        {
            return await this.historyTable.ToListAsync();
        }

        public async Task PostHistoryInformation(Historydb history)
        {
            await this.historyTable.InsertAsync(history);
        }

        public async Task UpdateHistoryInformation(Historydb history)
        {
            await this.historyTable.UpdateAsync(history);
        }
        //End of History Information ----------------------------------------

        //Food Diary Information --------------------------------------------
        public async Task<List<FoodDiarydb>> GetFoodDiaryInformation()
        {
            return await this.foodDiaryTable.ToListAsync();
        }

        public async Task PostFoodDiaryInformation(FoodDiarydb food)
        {
            await this.foodDiaryTable.InsertAsync(food);
        }

        public async Task UpdateFoodDiaryInformation(FoodDiarydb food)
        {
            await this.foodDiaryTable.UpdateAsync(food);
        }
        //End of Food Diary Information -------------------------------------
    }
}
