using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Store.DocumentModel.Put;

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
            var table = Table.LoadTable(amazonDynamoDBClient, "Store");

            var putDoc = new Document
        {
            { "Id", "4" },
            { "Artist", "Taylor Swift" },
            { "Name", "Red" },
            { "ReleaseDate", "08-25-2017" }
        };

            // Make the call to the Document API
            await table.PutItemAsync(putDoc);

            return "Success";
        }
        catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); return "Failed"; }
        catch (AmazonServiceException e) { Console.WriteLine(e.Message); return "Failed"; }
        catch (Exception e) { Console.WriteLine(e.Message); return "Failed"; }
    }
}
