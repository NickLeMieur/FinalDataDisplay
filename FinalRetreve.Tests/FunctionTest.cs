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
using Xunit;
using Amazon.Lambda.TestUtilities;

using FinalRetreve;
using System.Net.Http;
using System.Dynamic;

namespace FinalRetreve.Tests
{
    public class FunctionTest
    {
        public static readonly HttpClient client = new HttpClient();
        //Checks if the database information is filled correctly by using the http from the get to check if they are similar in length
        [Fact]
        public async void TestDatabase()
        {
            var function = new Function();
            var context = new TestLambdaContext();
            HttpResponseMessage httpResponse = await client.GetAsync("https://api.nytimes.com/svc/books/v3/lists/hardcover-fiction.json?api-key=buM7NOsyelhN3OAGrt3oGViXQjIyajlR");
            httpResponse.EnsureSuccessStatusCode();
            string responseBody = await httpResponse.Content.ReadAsStringAsync();

            Document myDoc = Document.FromJson(responseBody);
            dynamic bookObject = JsonConvert.DeserializeObject<ExpandoObject>(myDoc.ToJson());

            Assert.Equal(15, bookObject.results.books[14].rank);
        }
    }
}
