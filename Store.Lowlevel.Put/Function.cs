using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Store.Lowlevel.Put;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<string> FunctionHandler(string input, ILambdaContext context)
    {
        try
        {
            // Configure the DynamoDB Client
            var amazonDynamoDBConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1
            };
            AmazonDynamoDBClient amazonDynamoDBClient = new AmazonDynamoDBClient(amazonDynamoDBConfig);

            // Prepare the data object
            var item = new Dictionary<string, AttributeValue> {
            {  "Id", new AttributeValue { S = "2" } },
            {  "Artist", new AttributeValue { S = "Pink Floyd" } },
            {  "Name", new AttributeValue { S = "The Wall" } },
            {  "ReleaseDate", new AttributeValue { S = "09-30-1979" } }
        };

            // Prepare the request object
            var putItemRequest = new PutItemRequest
            {
                TableName = "Store",
                Item = item,

            };

            // Call AWS DynamoDB API 
            var putItemResponse = await amazonDynamoDBClient.PutItemAsync(putItemRequest);

            return putItemResponse.HttpStatusCode == System.Net.HttpStatusCode.OK ? "Success" : "Failed";
        }
        catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); return "Failed"; }
        catch (AmazonServiceException e) { Console.WriteLine(e.Message); return "Failed"; }
        catch (Exception e) { Console.WriteLine(e.Message); return "Failed"; }
    }
}
