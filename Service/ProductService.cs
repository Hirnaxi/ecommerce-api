using filpkart_api.Modals;
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

        public ProductService(IOptions<ProductSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _productCollection = database.GetCollection<ProductBase>(settings.Value.CollectionName);
            _signInCollection = database.GetCollection<SignIn>(settings.Value.SignInCollection);
            _orderCollection = database.GetCollection<Order>(settings.Value.OrderCollection);
            _cartCollection = database.GetCollection<Cart>(settings.Value.CartCollection);
        }

        // Product Methods
        public async Task<List<ProductBase>> GetProductsAsync() =>
            await _productCollection.Find(_ => true).ToListAsync();

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
        public async Task CreateOrderAsync(Order order)
        {
            if (order.Id == null || order.Id == ObjectId.Empty.ToString())
            {
                order.Id = ObjectId.GenerateNewId().ToString();   
            }

            await _orderCollection.InsertOneAsync(order);
        }



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

        
        }

    }

