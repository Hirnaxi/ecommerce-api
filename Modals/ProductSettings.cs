﻿using MongoDB.Bson.Serialization.Attributes;
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
        public string? OldPrice { get; set; }
        public string? Discount { get; set; }
        public string? Category { get; set; }

       

    }

    public class SignIn
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string? mobile { get; set; }

        [Required]
        public string? Password { get; set; }
        public bool? IfSignIn { get; set; } = true;

    }

    public class Order
    {
        [BsonId]
        public string? Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }

        public string? UserName { get; set; }

       

    }

  
  


    public class Cart
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

    }

     
    
    

}
