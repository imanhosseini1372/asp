using WebShop.WebSite;

var builder = WebApplication.CreateBuilder(args);




var app = builder.ConfigService().ConfigPipeLine();



app.Run();
