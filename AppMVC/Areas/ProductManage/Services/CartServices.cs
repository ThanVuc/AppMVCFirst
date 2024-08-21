using AppMVC.Areas.ProductManage.Models;
//using AppMVC.Models.Product;
using AppMVC.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SixLabors.ImageSharp.Formats;
using System.Data.Entity;

namespace AppMVC.Areas.ProductManage.Services
{
    public class CartServices
    {

        private readonly IHttpContextAccessor _context;
        private readonly HttpContext _httpContext;
        //private readonly AppDBContext _dbContext;
        //private readonly UserManager<AppUser> _userManager;
        //private int? cartKey { get; set; } = null;
        public const string CartKey = "CartKey";

        public string GetCarkey()
        {
            return CartKey;
        }

        public CartServices(IHttpContextAccessor context)
        {
            _context = context;
            _httpContext = _context.HttpContext;

            //if (_httpContext.User.Identity.IsAuthenticated)
            //{
            //    var currentUser = _userManager.GetUserAsync(_httpContext.User).Result;

            //    var cart = _dbContext.Carts
            //        .Include(c => c.Customer)
            //        .FirstOrDefault(c => c.Customer == currentUser);

            //    cartKey = cart.CartId;

            //    if (cart == null)
            //    {
            //        var cartCreate = new AppMVC.Models.Product.Cart() { Customer = currentUser };
            //        _dbContext.Carts.Add(cartCreate);
            //        _dbContext.SaveChanges();
            //    }
            //}
        }

        public IList<CartItem> GetCartItems()
        {
            var session = _httpContext.Session;
            var json = session.GetString(CartKey);

            if (!string.IsNullOrEmpty(json))
            {
                var listItems = JsonConvert.DeserializeObject<List<CartItem>>(session.GetString(CartKey));

                return listItems;
            } 
            //else
            //{
            //    var listItems = _dbContext.CartItems
            //        .Include(c => c.Product)
            //        .Include(c => c.Cart)
            //        .Where(item => item.CartId == cartKey).ToList();

            //    if (listItems != null)
            //    {
            //        string jsonCart = JsonConvert.SerializeObject(listItems, Formatting.Indented,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //            }
            //        );

            //        session.SetString(CartKey, jsonCart);

            //        return listItems;
            //    }
            //}
            return new List<CartItem>();
        }

        public void ClearAll()
        {
            try
            {
                var session = _httpContext.Session;
                session.Remove(CartKey);

                //var removeItem = _dbContext.CartItems
                //    .Where(item => item.CartId == cartKey);

                //_dbContext.CartItems.RemoveRange(removeItem);
                //_dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                throw;
            }

        }

        public void SaveCart(IList<CartItem> ls)
        {

            

            var session = _httpContext.Session;
            string jsonCart = JsonConvert.SerializeObject(ls, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
            );

            session.SetString(CartKey, jsonCart);

            // Database handle
            //if (cartKey == null)
            //{
            //    throw new Exception("Not Found Cart In This Case!");
            //}

            //var listOldItem = _dbContext.CartItems
            //    .Where(item => item.CartId == cartKey)
            //    .ToList();

            //IList<CartItem> listItemAdd;
            //listItemAdd = ls.Where(item => !listOldItem.Contains(item)).ToList();

            //IList<CartItem> listItemRemove;
            //listItemRemove = listOldItem.Where(item => ls.Contains(item)).ToList();

            //_dbContext.AddRange(listItemAdd);
            //_dbContext.CartItems.RemoveRange(listItemRemove);


            //_dbContext.SaveChanges();
        }

    }
}
