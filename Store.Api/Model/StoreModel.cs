using Amazon.DynamoDBv2.DataModel;

namespace Store.Api.Model
{
    [DynamoDBTable("Store")]
    public class StoreModel
    {
        public string Id { get; set; }

        [DynamoDBProperty("Name")]
        public string? AlbumName { get; set; }
        public string? Artist { get; set; }
        public string? ReleaseDate { get; set; }
    }
}
