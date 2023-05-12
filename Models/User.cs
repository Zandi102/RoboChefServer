using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RoboChefServer {

	public class User
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}