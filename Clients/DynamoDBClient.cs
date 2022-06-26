using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TGBot.Model;
using tryWeb.Extensions;
using tryWeb.Model;
using Newtonsoft.Json;

namespace tryWeb.Clients
{
    public class DynamoDBClient : IDynamoDBClient
    {
        public string _tableName;
        private readonly IAmazonDynamoDB _dynamoDB;

        public DynamoDBClient(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
            _tableName = Constants.TableName;
        }
        async Task<bool> IDynamoDBClient.DeleteDataDB(string ID, string MarketnameForDelete)
        {
            var item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                        { "User.ID", new AttributeValue { S = ID } }
                }
            };
            var response = await _dynamoDB.GetItemAsync(item);
            var reuslt = response.Item.ToClass<DBResponse>();
            var DESER = JsonConvert.DeserializeObject<List<string>>(reuslt.MarketName.ToLower());
            if (DESER.Remove($"{MarketnameForDelete}"))
            {
                DESER.Remove($"{MarketnameForDelete}");
            }
            else
            {
                Console.WriteLine("Error deleting data in DB");
                return false;
            }
            var InDB = JsonConvert.SerializeObject(DESER);

            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"User.ID", new AttributeValue {S = ID } },
                    {"MarketName", new AttributeValue {S = InDB } }
                }
            };
            try
            {
                var response1 = await _dynamoDB.PutItemAsync(request);
                return response1.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception)
            {
                Console.WriteLine("Error putting data in DB");
                return false;

            }
        }

        async Task<DBResponse> IDynamoDBClient.GetMarketsByID(string ID)
        {
            var item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                        { "User.ID", new AttributeValue { S = ID } }
                }
            };

            var response = await _dynamoDB.GetItemAsync(item);

            if (response.Item == null || !response.IsItemSet)
                return null;

            var reuslt = response.Item.ToClass<DBResponse>();

            return reuslt;
        }

        public async Task<bool> PostDataToDB(ModelForDB data)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"User.ID", new AttributeValue {S = data.UserID } },
                    {"MarketName", new AttributeValue {S = data.MarketName.ToLower() } }
                }
            };
            try
            {
                var response = await _dynamoDB.PutItemAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception)
            {
                Console.WriteLine("Error getting data from DB");
                return false;

            }
        }

        public async Task<bool> DeleteAll(string ID)
        {
            var item = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                        { "User.ID", new AttributeValue { S = ID } }
                }
            };
            var response = await _dynamoDB.GetItemAsync(item);
            var reuslt = response.Item.ToClass<DBResponse>();
            var DESER = JsonConvert.DeserializeObject<List<string>>(reuslt.MarketName);
            DESER.Clear();
            var InDB = JsonConvert.SerializeObject(DESER);

            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"User.ID", new AttributeValue {S = ID } },
                    {"MarketName", new AttributeValue {S = InDB } }
                }
            };
            try
            {
                var response1 = await _dynamoDB.PutItemAsync(request);
                return response1.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception)
            {
                Console.WriteLine("Error putting data in DB");
                return false;

            }
        }
    }
}
