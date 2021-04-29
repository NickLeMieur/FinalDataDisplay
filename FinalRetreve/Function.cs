using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace FinalRetreve
{
    public class Books
    {
        public int rank;
        public string title;
        public string author;
    }
    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private string tableName = "FinalBook";
        public async Task<Books> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string rank = "";
            Dictionary<string, string> dict = (Dictionary<string, string>)input.QueryStringParameters;
            dict.TryGetValue("rank", out rank);
            GetItemResponse res = await client.GetItemAsync(tableName, new Dictionary<string, AttributeValue>
                {
                    {"rank", new AttributeValue { N = rank } }
                }
            );
            Document myDoc = Document.FromAttributeMap(res.Item);
            Books myBook = JsonConvert.DeserializeObject<Books>(myDoc.ToJson());
            return myBook;
        }
    }
}
