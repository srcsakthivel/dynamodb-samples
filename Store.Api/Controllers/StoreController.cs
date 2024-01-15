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

        [HttpGet]
        public IEnumerable<StoreModel> Get()
        {
            return _dynamoDbContext.ScanAsync<StoreModel>(default).GetRemainingAsync().Result;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(StoreModel storeModel)
        {
            try
            {
                _dynamoDbContext.SaveAsync<StoreModel>(storeModel);

                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Created
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: ex.Message);
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> PutItem(StoreModel storeModel)
        {
            try
            {
                var loadItem = await _dynamoDbContext.LoadAsync<StoreModel>(storeModel.Id);

                var updateItem = new StoreModel
                {
                    Id = loadItem.Id,
                    AlbumName = storeModel.AlbumName ?? loadItem.AlbumName,
                    Artist = storeModel.Artist ?? loadItem.Artist,
                    ReleaseDate = storeModel.ReleaseDate ?? loadItem.ReleaseDate
                };

                await _dynamoDbContext.SaveAsync<StoreModel>(updateItem);

                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Accepted
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: ex.Message);
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }

        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteItem(string Id)
        {
            try
            {
                await _dynamoDbContext.DeleteAsync<StoreModel>(Id);
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Accepted
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: ex.Message);
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpGet("{Id}")]
        public async Task<StoreModel> GetItem(string Id)
        {
            return _dynamoDbContext.LoadAsync<StoreModel>(Id).Result;
        }
    }
}
