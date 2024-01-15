using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Model;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;
        private IDynamoDBContext _dynamoDbContext;

        public StoreController(IDynamoDBContext dynamoDBContext, ILogger<StoreController> logger)
        {
            _dynamoDbContext = dynamoDBContext;
            _logger = logger;
        }

        [HttpGet(Name = "GetStoreItems")]
        public IEnumerable<StoreModel> Get()
        {
            return _dynamoDbContext.ScanAsync<StoreModel>(default).GetRemainingAsync().Result;
        }

        [HttpGet("{Id}")]
        public StoreModel GetItem(string Id)
        {
            return _dynamoDbContext.LoadAsync<StoreModel>(Id).Result;
        }
    }
}
