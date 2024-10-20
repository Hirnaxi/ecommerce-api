﻿using filpkart_api.Modals;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace filpkart_api.Service
{
    public class ProductService
    {
        private readonly IMongoCollection<ProductBase> _productCollection;
        private readonly IMongoCollection<SignIn> _signInCollection;
        private readonly IMongoCollection<Order> _orderCollection;
        private readonly IMongoCollection<Cart> _cartCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Rating> _ratingCollection;

        public ProductService(IOptions<ProductSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _productCollection = database.GetCollection<ProductBase>(settings.Value.CollectionName);
            _signInCollection = database.GetCollection<SignIn>(settings.Value.SignInCollection);
            _orderCollection = database.GetCollection<Order>(settings.Value.OrderCollection);
            _cartCollection = database.GetCollection<Cart>(settings.Value.CartCollection);
            _categoryCollection = database.GetCollection<Category>(settings.Value.CategoryCollection);
            _ratingCollection = database.GetCollection<Rating>(settings.Value.RatingCollection);
        }

        // Product Methods
        public async Task<List<ProductBase>> GetProductsAsync(string search = "")
        {
            BsonRegularExpression regex = new(search, "i");
            var res = _productCollection.Aggregate().Match(new BsonDocument
            {
                { "Brand", regex}
            }).Project<ProductBase>(new BsonDocument
            {
                { "Name", 1 },
                { "Brand", 1 },
                { "Price", 1 },
                { "Image1", 1 },
                { "Image2", 1 },
                { "Image3", 1 },
                { "Image4", 1 },
                { "Color", 1 },
                { "Model", 1 },
                { "Weight", 1 },
                { "Description", 1 },
                { "Description2", 1 },
                { "Warranty", 1 },
                { "OldPrice", 1 },
                { "Discount", 1 },
                { "Stock", 1 },
                { "Category", 1 }
            }).ToList();
           return res;
        }

        ///category

        public async Task<List<Category>> GetCategoriesAsync()=>
            await _categoryCollection.Find(_ => true).ToListAsync();

        public async Task CreateCategoryAsync(Category category) =>
           await _categoryCollection.InsertOneAsync(category);


        public async Task<Category> GetCategoryByIdAsync(string id) =>
           await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();


        public async Task<ProductBase> GetProductByIdAsync(string id) =>
            await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateProductAsync(ProductBase product) =>
            await _productCollection.InsertOneAsync(product);

        public async Task UpdateProductAsync(string id, ProductBase updatedProduct) =>
            await _productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task DeleteProductAsync(string id) =>
            await _productCollection.DeleteOneAsync(x => x.Id == id);



        // SignIn Methods
        public async Task<List<SignIn>> GetSignInAsync() =>
            await _signInCollection.Find(_ => true).ToListAsync();

        public async Task CreateSignInAsync(SignIn signIn) =>
            await _signInCollection.InsertOneAsync(signIn);

        public async Task LogoutAccountAsync(string id, SignIn signOut) =>
            await _signInCollection.ReplaceOneAsync(x => x.Id == id, signOut);

        public async Task DeleteAccountAsync(string id) =>
            await _signInCollection.DeleteOneAsync(x => x.Id == id);



        // Order Methods
  /*      public async Task CreateOrderAsync(Order order)
        {
            if (order.Id == null || order.Id == ObjectId.Empty.ToString())
            {
                order.Id = ObjectId.GenerateNewId().ToString();   
            }

            await _orderCollection.InsertOneAsync(order);
        }*/



   /*     public async Task<List<Order>> GetOrdersAsync() =>
            await _orderCollection.Find(_ => true).ToListAsync();*/

        public async Task<List<Order>> GetOrderByIdAsync(string userId) =>
            await _orderCollection.Find(x => x.UserId == userId).ToListAsync();

        public async Task DeleteOrderAsync(string id) =>
            await _orderCollection.DeleteOneAsync(x => x.Id == id);

        //  public async Task<List<Order>> GetOrdersByUserNameAsync(string userName)
        //{
        //  return await _orderCollection.Find(order => order.UserName == userName).ToListAsync();
        //}


        //Cart Methods

        public async Task CreateCartAsync(Cart cart)=>
            await _cartCollection.InsertOneAsync(cart);

        public async Task<List<Cart>> GetCartsAsync()=>
            await _cartCollection.Find(_ => true).ToListAsync();

        public async Task<List<Cart>> GetCartByIdAsync(string userId)=>
            await _cartCollection.Find(x =>x.UserId == userId).ToListAsync();

        public async Task DeleteCartAsync(string id) =>
            await _cartCollection.DeleteOneAsync(x => x.Id == id);


        public async Task UpdateCartAsync(string id, Cart updateCart) =>
           await _cartCollection.ReplaceOneAsync(x => x.Id == id, updateCart);

    
        public async Task ClearCartAsync(string userId)
        {
            var filter = Builders<Cart>.Filter.Eq(x => x.UserId,userId);
            await _cartCollection.DeleteManyAsync(filter);
        }


        //Address Management
        public async Task CreateOrderAsync(Order order)
        {
            if (order.Id == null || order.Id == ObjectId.Empty.ToString())
            {
                order.Id = ObjectId.GenerateNewId().ToString();
            }

            await _orderCollection.InsertOneAsync(order);

            var user = await _signInCollection.Find(x => x.Id == order.UserId).FirstOrDefaultAsync();

            if (user != null)
            {
                if (!user.AddressList.Any(a => a.StreetAddress == order.Address.StreetAddress &&
                                                a.City == order.Address.City &&
                                                a.State == order.Address.State &&
                                                a.PostalCode == order.Address.PostalCode &&
                                                a.Country == order.Address.Country &&
                                                a.Contact == order.Address.Contact &&
                                                a.Name == order.Address.Name))
                {
                    user.AddressList.Add(order.Address);
                    await _signInCollection.ReplaceOneAsync(x => x.Id == user.Id, user);
                }
            }
        }

        // Rating Collection API//
        public async Task<List<Rating>> GetRatingsAsync() =>
            await _ratingCollection.Find(_ => true).ToListAsync();

        public async Task CreateRatingAsync(Rating rating) =>
       await _ratingCollection.InsertOneAsync(rating);


     

        public async Task<Rating> GetRatingByProductId (string productId )=>
            await _ratingCollection.Find(X => X.ProductId == productId).FirstOrDefaultAsync();

        
    }

    }

