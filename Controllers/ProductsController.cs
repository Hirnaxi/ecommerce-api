using filpkart_api.Service;
using filpkart_api.Modals;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace filpkart_api.Controllers
{
    [ApiController]
    [Route("Products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService service)
        {
            _productService = service;
        }

        // Get all products
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<ProductBase>>> GetProducts(string search = "")
        {
            var products = await _productService.GetProductsAsync(search);
            return Ok(products);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCategory")]
        public async Task<ActionResult<List<Category>>> GetCategory()
        {
            var category = await _productService.GetCategoriesAsync();
            return Ok(category);
        }


        [HttpPost("Create_Category")]
        public async Task<IActionResult> CreateCategory([FromBody] Category newProduct)
        {
            if (newProduct == null)
            {
                return BadRequest("Product data is required.");
            }

            newProduct.Id = ObjectId.GenerateNewId().ToString();
            await _productService.CreateCategoryAsync(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }


        [HttpGet("CategoryBy{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(string id)
        {
            var category = await _productService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }
















        // Get a product by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBase>> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Create a new product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductBase newProduct)
        {
            if (newProduct == null)
            {
                return BadRequest("Product data is required.");
            }

            newProduct.Id = ObjectId.GenerateNewId().ToString();
            await _productService.CreateProductAsync(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        // Update an existing product
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductBase updatedProduct)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            updatedProduct.Id = id;
            await _productService.UpdateProductAsync(id, updatedProduct);
            return NoContent();
        }

        // Delete a product by ID
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        // Get all sign-in records
        [HttpGet("SignIn")]
        public async Task<ActionResult<List<SignIn>>> GetSignIn()
        {
            var signIns = await _productService.GetSignInAsync();
            return Ok(signIns);
        }

        // Create a new sign-in record
        [HttpPost("SignIn")]
        public async Task<IActionResult> CreateSignIn([FromBody] SignIn newSignIn)
        {
            if (newSignIn == null)
            {
                return BadRequest("SignIn data is required.");
            }

            newSignIn.Id = ObjectId.GenerateNewId().ToString();
            await _productService.CreateSignInAsync(newSignIn);
            return CreatedAtAction(nameof(GetSignIn), new { id = newSignIn.Id }, newSignIn);
        }

        //Login
        [HttpPost ("Login")]
        public async Task<IActionResult> CreateLogin([FromBody] SignIn LoginData)
        {
            if(LoginData == null || string.IsNullOrEmpty(LoginData.UserName) || string.IsNullOrEmpty(LoginData.Password))
            {
                return BadRequest("Username and Password are required");
            }

            var user = (await _productService.GetSignInAsync()).FirstOrDefault(u => u.UserName == LoginData.UserName && u.Password == LoginData.Password);

            if(user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            user.IfSignIn = true;
            await _productService.LogoutAccountAsync(user.Id, user);

            return Ok(user);
        }

        //Update User Data
        [HttpPut("Update-User/{id}")]

        public async Task<IActionResult> UpdateUser(string id,[FromBody] SignIn updateUserData)
        {
            if(updateUserData == null || id != updateUserData.Id)
            {
                return BadRequest("Invalid user Data");
            }

            var existingUser = (await _productService.GetSignInAsync()).FirstOrDefault(x => x.Id == id);
            if(existingUser == null)
            {
                return NotFound("User not found");
            }

            existingUser.UserName = updateUserData.UserName;
            existingUser.Password = updateUserData.Password;
           
            await _productService.LogoutAccountAsync(id, existingUser);
            return NoContent();
        }

        // Logout a user
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] SignIn signOutUser)
        {
            if (signOutUser == null || string.IsNullOrEmpty(signOutUser.UserName))
            {
                return BadRequest("User data is required for logout.");
            }

            var user = (await _productService.GetSignInAsync()).FirstOrDefault(u => u.UserName == signOutUser.UserName);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.IfSignIn = false;
            await _productService.LogoutAccountAsync(user.Id, user);

            return Ok("User logged out successfully.");
        }

        // Delete a sign-in record by ID
        [HttpDelete("SignIn/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = (await _productService.GetSignInAsync()).FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            await _productService.DeleteAccountAsync(id);
            return NoContent();
        }



        // Create a new order
        [HttpPost("Order")]
        public async Task<IActionResult> PlaceOrder([FromBody] Order newOrder)
        {
            if (newOrder == null)
            {
                return BadRequest("Order data is required.");
            }

            await _productService.CreateOrderAsync(newOrder);
            return Ok("Order placed successfully.");
        }




        // Get all orders
        /*    [HttpGet("Orders")]
            public async Task<ActionResult<List<Order>>> GetOrders()
            {
                var orders = await _productService.GetOrdersAsync();
                return Ok(orders);
            }
    */
        // Get an order by ID
        [HttpGet("GetOrderByUserId")]
        public async Task<ActionResult<Order>> GetOrderById(string userId)
        {
            var order = await _productService.GetOrderByIdAsync(userId);
          
            return Ok(order);
        }




        // Delete an order by ID
        [HttpDelete("Orders/{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _productService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _productService.DeleteOrderAsync(id);
            return NoContent();
        }

        //Create a new cart
        [HttpPost("Cart")]
        public async Task<IActionResult> PlaceCart([FromBody] Cart newCart)
        {
            if (newCart == null)
            {
                return BadRequest("Cart data is required.");
            }

            await _productService.CreateCartAsync(newCart);
            return Ok("Order placed successfully.");
        }


        // Get all cart
        [HttpGet("Cart")]
        public async Task<ActionResult<List<Cart>>> GetCart()
        {
            var carts = await _productService.GetCartsAsync();
            return Ok(carts);
        }

        // Get an cart by ID

        [HttpGet("CartById")]
        public async Task<ActionResult<Cart>> GetCartById(string userId)
        {
            var cart = await _productService.GetCartByIdAsync(userId);
            return Ok(cart);
        }




        // Delete an order by ID
        [HttpDelete("Cart/{id}")]
        public async Task<IActionResult> DeleteCart(string id)
        {
            var cart = await _productService.GetCartByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            await _productService.DeleteCartAsync(id);
            return NoContent();
        }

            [HttpPut("Update-Cart/{id}")]

            public async Task<IActionResult> UpdateCart(string id, [FromBody] Cart updateCartData)
            {
            
                if (string.IsNullOrWhiteSpace(id) || updateCartData == null)
                {
                    return BadRequest("Invalid cart ID or data");
                }

          
                var existingCart = (await _productService.GetCartsAsync()).FirstOrDefault(x => x.Id == id);
                if (existingCart == null)
                {
                    return NotFound("Cart not found");
                }


                existingCart.Quantity = updateCartData.Quantity;
    
                await _productService.UpdateCartAsync(id, existingCart);

                return NoContent();
            }



        // Get all users
        [HttpGet("Users")]
        public async Task<ActionResult<List<SignIn>>> GetUsers()
        {
            var users = await _productService.GetSignInAsync();
            return Ok(users);
        }



        // Address management

        [HttpGet("User/{userId}/Addresses")]
        public async Task<ActionResult<List<Address>>> GetUserAddresses(string userId)
        {

            var users = await _productService.GetSignInAsync();

            var user = users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user.AddressList);
        }

    }
}
