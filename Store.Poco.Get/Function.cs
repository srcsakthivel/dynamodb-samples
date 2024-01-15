using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Store.Poco.Get;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<StorePoco> FunctionHandler(string input, ILambdaContext context)
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

            // Retrieve an object from DynamoDB using the hash key and sort key
            var sportItemPoco = await dynamoDBContext.LoadAsync<StorePoco>(input);
            return sportItemPoco != null ? sportItemPoco : new StorePoco();
        }
        catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); return new StorePoco(); }
        catch (AmazonServiceException e) { Console.WriteLine(e.Message); return new StorePoco(); }
        catch (Exception e) { Console.WriteLine(e.Message); return new StorePoco(); }
    }
}
