using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfficesAPI.Domain.Data.Models;

[Serializable, BsonIgnoreExtraElements]
public class Photo
{
    [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("url"), BsonRepresentation(BsonType.String)]
    public string Url { get; set; }
    [BsonElement("title"), BsonRepresentation(BsonType.String)]
    public string Title { get; set; }
    [BsonElement("office_id"), BsonRepresentation(BsonType.String)]
    public string OfficeId { get; set; }
}
