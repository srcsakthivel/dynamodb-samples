using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Store.DocumentModel.Get;

public class Function
{
    public async Task<string> FunctionHandler(string input, ILambdaContext context)
    {
        // Configure the DynamoDB SDK Client
        var amazonDynamoDBConfig = new AmazonDynamoDBConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.USEast1
        };
        AmazonDynamoDBClient amazonDynamoDBClient = new AmazonDynamoDBClient(amazonDynamoDBConfig);
        var table = Table.LoadTable(amazonDynamoDBClient, "Store");
        var result = await table.GetItemAsync(input);
        var output = result != null ? $" Id: {result["Id"]} \n Artist: {result["Artist"]} \n Name: {result["Name"]} \n ReleaseDate: {result["ReleaseDate"]}" : "Not Found";
        context.Logger.LogInformation(output);
        return result != null ? result["Name"] : "empty";
    }
}
