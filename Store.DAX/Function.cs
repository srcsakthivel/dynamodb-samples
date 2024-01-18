using Amazon.DAX;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Store.DAX;

public class Function
{
    public async Task<string> FunctionHandler(string input, ILambdaContext context)
    {
        var endpointUri = "dax://my-cluster.123abc.dax-clusters.us-east-1.amazonaws.com";

        var clientConfig = new DaxClientConfig(endpointUri)
        {
            AwsCredentials = FallbackCredentialsFactory.GetCredentials()
        };
        var client = new ClusterDaxClient(clientConfig);

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
        var getItemResponse = await client.GetItemAsync(getItemRequest);

        return getItemResponse.HttpStatusCode == System.Net.HttpStatusCode.OK && getItemResponse.Item.Count > 0 ? getItemResponse.Item["Name"].S : "Empty";
    }
}
