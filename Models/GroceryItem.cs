using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RoboChefServer
{

    public class GroceryItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Username { get; set; }

        public string? Name { get; set; }

        public string? Quantity { get; set; }
    }
}