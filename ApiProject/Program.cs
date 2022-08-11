using ApiProject;
using Serilog;

LogUtils.Startup();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

var services = builder.Services;
services.AddCors(opt => {
    opt.AddDefaultPolicy(b=> b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
services.AddControllers().AddNewtonsoftJson(opt => {
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
services.AddSwaggerGen(c=> {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Blazor LastBlock Backend", Version ="v1" });
    c.EnableAnnotations();
});
services.AddHttpClient();


var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
}

app.UseSwagger();
app.UseCors();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(ep => {
    ep.MapControllers();
});
app.Run();
