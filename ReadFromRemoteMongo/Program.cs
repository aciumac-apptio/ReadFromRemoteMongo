using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ReadFromRemoteMongo
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Press Enter");
            Console.WriteLine();
        }

        static async Task MainAsync(string[] args)
        {
            await Task.Delay(500);
            var connectionString = "mongodb://192.168.5.102:27017";
            var client = new MongoClient(connectionString);

            var db = client.GetDatabase("test");
            var col = db.GetCollection<Meteorite>("meteorites");
            
            using (var cursor = await col.Find(new BsonDocument()).ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var doc in cursor.Current)
                    {                       
                        var json = JsonConvert.SerializeObject(doc);
                        Console.WriteLine(json);               
                    }
                }
            }
        }

        [BsonIgnoreExtraElements]
        private class Meteorite
        {
            public ObjectId Id { get; set; }
            public string fall { get; set; }
            public GeoLocation geolocation { get; set; }
            public double mass { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string nametype { get; set; }
            public string recclass { get; set; }
            public double reclat { get; set; }
            public double reclong { get; set; }
            public DateTime year { get; set; }
            
        }

        private class GeoLocation
        {
            public string type { get; set; }
            public double[] coordinates { get; set; }
        }
    }

}
