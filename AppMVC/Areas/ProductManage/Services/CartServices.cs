namespace AppMVC.Areas.ProductManage.Services
{
    public class CartServices
    {

        IHttpContextAccessor _context;
        HttpContext _httpContext;

        public CartServices(HttpContextAccessor context)
        {
            _context = context;
            _httpContext = _context.HttpContext;
        }



    }
}
