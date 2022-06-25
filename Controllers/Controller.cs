using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TGBot.Model;
using tryWeb.Clients;
using tryWeb.Model;

namespace tryWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetCourseDateController : ControllerBase
    {
        private readonly ILogger<GetCourseDateController> _logger;
        private readonly Client1 _Client;
        


        public GetCourseDateController(ILogger<GetCourseDateController> logger, Client1 client)
        {
            _logger = logger;
            _Client = client;
           


        }

        [HttpGet("{valueCode}/{Date}")]
        public async Task<List<CourseResponse>> GetCourse([FromRoute] string valueCode, string Date)
        {
            var course = await _Client.Getcourse(valueCode, Date);

            return course;
        }






    }
    [ApiController]
    [Route("[controller]")]
    public class GetCourseController : ControllerBase
    {


        private readonly ILogger<GetCourseController> _logger2;
        private readonly Client2 _Client2;
        private readonly IDynamoDBClient _dynamoDbClient;

        public GetCourseController(ILogger<GetCourseController> logger2, Client2 client2, IDynamoDBClient dynamoDBClient)
        {
            _logger2 = logger2;
            _Client2 = client2;
            _dynamoDbClient = dynamoDBClient;
        }
        [HttpGet("{TradingPlaceName}")]
        public async Task<ModelCoinForBOT> GetPrice([FromRoute] string TradingPlaceName)
        {
            var course = await _Client2.Getprice(TradingPlaceName);
            var result = new ModelCoinForBOT
            {
                Course = course.tickers.FirstOrDefault().last.ToString()
            };
            return result;
        }


        [HttpGet("MarketsFromDBByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFavMarkets([FromRoute] string ID)
        {
            var result = await _dynamoDbClient.GetMarketsByID(ID);

            if (result == null)
                return null;

            var favMarkets = new DBResponse
            {
                MarketName = result.MarketName
            };

            var Data = JsonConvert.DeserializeObject<List<string>>(result.MarketName);
            return Ok(Data);
        }


        [HttpPost("AddFavs")]
        public async Task<IActionResult> AddFavs([FromBody] ModelForDB modelForDB)
        {

            var result = await _dynamoDbClient.PostDataToDB(modelForDB);

            if (result == false)
            {
                return BadRequest("Post error");
            }
            return Ok("Sucsessful adding to db");
        }


        [HttpDelete("DeleteFavs/{ID}/{MarketNameForDelete}")]
        public async Task<IActionResult> Delete([FromRoute] string ID, string MarketNameForDelete)
        {
            var result = await _dynamoDbClient.DeleteDataDB(ID, MarketNameForDelete);

            if (result == false)
            {
                return BadRequest("delete error");
            }
            return Ok("Sucsessful delete for db");


        }

        [HttpDelete("DeleteFavs/{ID}/all")]
        public async Task<IActionResult> Delete([FromRoute] string ID)
        {
            var result = await _dynamoDbClient.DeleteAll(ID);

            if (result == false)
            {
                return BadRequest("delete \"all\" error"); ;
            }
            return Ok("Sucsessful \"all\" delete for db");


        }





    }
}



