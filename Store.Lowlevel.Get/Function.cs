using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Store.Lowlevel.Get;

public class Function
{
    public async Task<string> FunctionHandler(string input, ILambdaContext context)
    {
        // Configure the DynamoDB Client
        var amazonDynamoDBConfig = new AmazonDynamoDBConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.USEast1
        };
        AmazonDynamoDBClient amazonDynamoDBClient = new AmazonDynamoDBClient(amazonDynamoDBConfig);

        // Prepare the request object
        var getItemRequest = new GetItemRequest
        {
            TableName = "Store",
            Key = new Dictionary<string, AttributeValue>()
            {
                {
                    "Id", new AttributeValue { S = input }
                }
            },
            ConsistentRead = true
        };

        // Call AWS DynamoDB API
        var getItemResponse = await amazonDynamoDBClient.GetItemAsync(getItemRequest);

        return getItemResponse.HttpStatusCode == System.Net.HttpStatusCode.OK  && getItemResponse.Item.Count > 0 ? getItemResponse.Item["Name"].S : "Empty";
    }
}
