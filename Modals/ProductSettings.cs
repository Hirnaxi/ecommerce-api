using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace filpkart_api.Modals
{
    public class ProductSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
        public string SignInCollection { get; set; } = null!;
        public string OrderCollection    { get; set;} = null!;
        public string CartCollection { get; set; } = null!;
        public string CategoryCollection { get; set; } = null!;
        public string RatingCollection { get; set; } = null!;

    }

    public class Category
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductBase
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Brand { get; set; }
        public decimal? Price { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? Color { get; set; }
        public string? Model {  get; set; }
        public string? Weight { get; set; }
        public string? Description { get; set; }
        public string? Description2 { get; set; }
        public string? Warranty { get; set; }
        public decimal? OldPrice { get; set; }
        public string? Discount { get; set; }
        public string? Stock {  get; set; }
        public string? Category { get; set; }

       

    }

    public class SignIn
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string? mobile { get; set; }

        [Required]
        public string? Password { get; set; }
        public bool? IfSignIn { get; set; } = true;
        public string? Email { get; set; }
        public string? confirmPassword {  get; set; }
        public List<Address> AddressList { get; set; } = new List<Address>();

    }

    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId), BsonElement("_id")]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId), BsonElement("productId")]
        public List<string> ProductId { get; set; }
        public string? Name { get; set; }
        public int Quantity {  get; set; }
        public string? Contact { get; set; }
        public Address Address { get; set; }
        public string PaymentMethod { get; set; }

        [BsonRepresentation(BsonType.ObjectId), BsonElement("userId")]
        public string UserId { get; set; }
    }

    public class Address
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }


    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId), BsonElement("_id")]
        public string? Id { get; set; }
        
        public string? ProductId { get; set; }
        public int Quantity { get; set; }
        [BsonRepresentation(BsonType.ObjectId), BsonElement("userId")]
        public string? UserId { get; set; }


    }


    public class Rating
    {
        [BsonRepresentation(BsonType.ObjectId),BsonElement("_id")]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId),BsonElement("productId")]
        public string? ProductId { get; set; }
        [BsonRepresentation(BsonType.ObjectId), BsonElement("userId")]
        public string? UserId { get;set; }
        public int RatingNumber { get; set; }

        public string? Review {  get; set; }
    }





}
