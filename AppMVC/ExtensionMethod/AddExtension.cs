namespace AppMVC.ExtensionMethod
{
    public static class AddExtension
    {
        public static void AddStatusCodePage(this IApplicationBuilder app)
        {
            app.UseStatusCodePages(appError =>
            {
                appError.Run(async context =>
                {
                    var response = context.Response;
                    var code = context.Response.StatusCode;

                    response.Redirect($"/StatusErrors?code={code}");
                });
            });
        }



    }
}
