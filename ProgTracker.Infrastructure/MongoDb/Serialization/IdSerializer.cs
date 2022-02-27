using ProgTracker.Domain.Model;
using ProgTracker.Domain.Model.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ProgTracker.Infrastructure.MongoDb.Serialization;

// https://github.com/mongodb/mongo-csharp-driver/blob/14e046f23640ff9257c4edf53065b9a6768254d4/src/MongoDB.Bson/Serialization/Serializers/ObjectIdSerializer.cs
// https://github.com/mongodb/mongo-csharp-driver/blob/010e7ee46b085cdd3762894ece9e2d258b66ab0d/src/MongoDB.Bson/Serialization/PrimitiveSerializationProvider.cs

public class IdSerializer : StructSerializerBase<Id>, IRepresentationConfigurable<IdSerializer>
{
    private readonly BsonType _representation;

    public BsonType Representation
    {
        get { return _representation; }
    }

    public IdSerializer() : this(BsonType.ObjectId)
    {
    }

    public IdSerializer(BsonType representation)
    {
        switch (representation)
        {
            case BsonType.ObjectId:
                break;

            default:
                var message = string.Format("{0} is not a valid representation for a IdSerializer.", representation);
                throw new ArgumentException(message);
        }

        _representation = representation;
    }

    public override Id Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonReader = context.Reader;

        var bsonType = bsonReader.GetCurrentBsonType();
        switch (bsonType)
        {
            case BsonType.ObjectId:
                return new Id(bsonReader.ReadObjectId().ToString());
        }

        throw CreateCannotDeserializeFromBsonTypeException(bsonType);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Id value)
    {
        var bsonWriter = context.Writer;

        switch (_representation)
        {
            case BsonType.ObjectId:
                if (value.Value != null)
                {
                    bsonWriter.WriteObjectId(new ObjectId(value.Value));
                }
                else if (value.Value == "undefined")
                {
                }
                else
                {
                    bsonWriter.WriteNull();
                    // bsonWriter.WriteObjectId(default);
                }

                break;

            default:
                var message = string.Format("'{0}' is not a valid Id representation.", _representation);
                throw new BsonSerializationException(message);
        }
    }

    public IdSerializer WithRepresentation(BsonType representation)
    {
        if (representation == _representation)
        {
            return this;
        }
        else
        {
            return new IdSerializer(representation);
        }
    }

    IBsonSerializer IRepresentationConfigurable.WithRepresentation(BsonType representation)
    {
        return WithRepresentation(representation);
    }
}