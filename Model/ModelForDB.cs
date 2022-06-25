using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBot.Model
{
    public class ModelForDB
    {
        public string UserID { get; set; }
        public string MarketName { get; set; }
    }

}
