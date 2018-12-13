using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AlexaComp.Core;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.Core;

namespace AlexaComp.Core.Controllers {
    class MDBController : AlexaCompCore{

        private static string URI = MDBConfig.APIURI;

        public static void makeInstance() {
            MainAsync();
        }

        static async Task MainAsync() {
            Clog("in MainAsync");
            var client = new MongoClient(URI);
            
            Clog("Client Defined");
            Clog(client.ListDatabasesAsync().ToString());
            IMongoDatabase db = client.GetDatabase("UsageStats");
            Clog("Databases Defined");

            var collection = db.GetCollection<BsonDocument>("UsageStats");
            Clog("DB Defined");
            db.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
            Clog("Pinging...");

            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument())) {
                while (await cursor.MoveNextAsync()) {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch) {
                        Console.WriteLine(document);
                        Console.WriteLine();
                    }
                }
            }
        }
    }

    public class UsageStats {
        public object Users { get; set; }
        public object API { get; set; }
        public object Requests { get; set; }
        public object Client { get; set; }
    }
}
