using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUnity",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10L * 1024 * 1024 * 1024; // 10 GB in bytes, because if I typed out the number it refused to load. Do not ask questions for I do not have answers
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "games")),
    RequestPath = "/games",
    ServeUnknownFileTypes = true, //hail marry at this point tbh
    DefaultContentType = "application/octet-stream",

    OnPrepareResponse = ctx =>
    {
        var path = ctx.File.Name;

        // If serving a .br file, set the Content-Encoding header
        if (path.EndsWith(".br"))
        {
            ctx.Context.Response.Headers["Content-Encoding"] = "br";

            // Set the correct Content-Type based on the file extension
            ctx.Context.Response.Headers["Content-Type"] = path switch
            {
                var p when p.EndsWith(".js.br") => "application/javascript",
                var p when p.EndsWith(".data.br") => "application/octet-stream",
                var p when p.EndsWith(".wasm.br") => "application/wasm",
                _ => "application/octet-stream"
            };
        }
    }
});




app.UseCors("AllowUnity");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
