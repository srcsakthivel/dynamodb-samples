using Amazon.Auth.AccessControlPolicy;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Model;

namespace Store.API.Controllers
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

        [HttpPost]
        public async Task<HttpResponseMessage> Post(StoreModel storeModel)
        {
            if (storeModel == null)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }
            try
            {
                await _dynamoDbContext.SaveAsync<StoreModel>(storeModel);
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

        [HttpGet("{Id}")]
        public async Task<StoreModel> GetItem(string Id)
        {
            try
            {
                var loadItem = await _dynamoDbContext.LoadAsync<StoreModel>(Id);
                return loadItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: ex.Message);
                return null;
            }            
            
        }

        [HttpPut]
        public async Task<HttpResponseMessage> PutItem(StoreModel storeModel)
        {
            // Update Item 
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

        [HttpDelete("{Id}")]
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

        
    }
}
