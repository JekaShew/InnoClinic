using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfficesAPI.Domain.Data.Models;

[Serializable, BsonIgnoreExtraElements]
public class Office
{
    [BsonId, BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("city"), BsonRepresentation(BsonType.String)]
    public string City { get; set; }
    [BsonElement("street"), BsonRepresentation(BsonType.String)]
    public string Street { get; set; }
    [BsonElement("house_number"), BsonRepresentation(BsonType.String)]
    public string HouseNumber { get; set; }
    [BsonElement("office_number"), BsonRepresentation(BsonType.String)]
    public string? OfficeNumber { get; set; }
    [BsonElement("registry_phone_number"), BsonRepresentation(BsonType.String)]
    public string RegistryPhoneNumber { get; set; }
    [BsonElement("status"), BsonRepresentation(BsonType.Boolean)]
    public bool IsActive { get; set; }
    [BsonElement("office_photos")]
    public ICollection<Photo>? Photos { get; set; }
}
