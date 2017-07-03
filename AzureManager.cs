using GetHealthyApp.DataModels;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHealthyApp
{
    class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<DataModels.EnterWeight> enterWeightTable;
        private IMobileServiceTable<Historydb> historyTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://gethealthy.azurewebsites.net");
            this.enterWeightTable = this.client.GetTable<DataModels.EnterWeight>();
            this.historyTable = this.client.GetTable<Historydb>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

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

        public async Task<List<DataModels.EnterWeight>> GetWeightInformation()
        {
            return await this.enterWeightTable.ToListAsync();
        }

        public async Task PostWeightInformation(DataModels.EnterWeight enterWeight)
        {
            await this.enterWeightTable.InsertAsync(enterWeight);
        }

        public async Task UpdateWeightInformation(DataModels.EnterWeight enterWeight)
        {
            await this.enterWeightTable.UpdateAsync(enterWeight);
        }

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
    }
}
