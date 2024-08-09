using System.Net;

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
                    var code = response.StatusCode;
                    var codeMeans = (HttpStatusCode)code;
                    response.ContentType = "text/html";
                    string html = $@"
                    <body style=""background-color: black;""></body>
                    <h1 style=""
                    text-align: center; font-size: 4vw;
                    position: relative;
                    color: rgb(185, 199, 211);
                    top: 35%;
                    "" >{code} | {codeMeans}</h1>
                    ";

                    await response.WriteAsync(html);
                });
            });
        }



    }
}
