using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGBot.Model;
using tryWeb.Model;

namespace tryWeb.Clients
{
    public interface IDynamoDBClient
    {
        public Task<DBResponse> GetMarketsByID(string UserID);
        public Task<bool> PostDataToDB(ModelForDB data);
        public Task<bool> DeleteAll(string UserID);
        public Task<bool> DeleteDataDB(string UserID, string MarletNameForDelete);
    }
}
