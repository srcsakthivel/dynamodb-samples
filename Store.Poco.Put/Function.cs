using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Store.Poco.Put;

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
            // Configure the DynamoDB SDK Client
            var amazonDynamoDBConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1
            };
            AmazonDynamoDBClient amazonDynamoDBClient = new AmazonDynamoDBClient(amazonDynamoDBConfig);

            // Create a DynamoDB Context Object
            var dynamoDBContext = new DynamoDBContext(amazonDynamoDBClient);

            // Create a Sports Item POCO object and save it to the DynamoDB table
            var sportsItemPoco = new StorePoco
            {
                Id = "3",
                AlbumName = "The Division Bell",
                Artist = "Pink Floyd",
                ReleaseDate = "09-30-1979"
            };

            await dynamoDBContext.SaveAsync(sportsItemPoco);

            return "Success";
        }
        catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); return "Failed"; }
        catch (AmazonServiceException e) { Console.WriteLine(e.Message); return "Failed"; }
        catch (Exception e) { Console.WriteLine(e.Message); return "Failed"; }
    }
}
