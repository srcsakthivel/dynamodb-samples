using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Poco.Put
{
    [DynamoDBTable("Store")]
    public class StorePoco
    {
        public string Id { get; set; }

        [DynamoDBProperty("Name")]
        public string AlbumName { get; set; }
        public string Artist { get; set; }
        public string ReleaseDate { get; set; }
    }
}
