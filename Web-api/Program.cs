using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;

using shreddit.Data;
using shreddit.Service;
using shared.Model;
using System.Data.Common;


var builder = WebApplication.CreateBuilder(args);

// S�tter CORS s� API'en kan bruges fra andre dom�ner
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Tilf�j DbContext factory som service.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));

// Tilf�j DataService s� den kan bruges i endpoints
builder.Services.AddScoped<DataService>();

// Dette kode kan bruges til at fjerne "cykler" i JSON objekterne.
/*
builder.Services.Configure<JsonOptions>(options =>
{
    // Her kan man fjerne fejl der opst�r, n�r man returnerer JSON med objekter,
    // der refererer til hinanden i en cykel.
    // (alts� dobbelrettede associeringer)
    options.SerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
*/

var app = builder.Build();

// Seed data hvis n�dvendigt.
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    dataService.SeedData(); // Fylder data p�, hvis databasen er tom. Ellers ikke.
}

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

// Middlware der k�rer f�r hver request. S�tter ContentType for alle responses til "JSON".
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});


// DataService f�s via "Dependency Injection" (DI)
app.MapGet("/", (DataService service) =>
{
    return new { message = "Hello World!" };
});

app.MapGet("/api/posts", (DataService service) =>
{
    return service.GetPosts();
});

app.MapGet("/api/posts/{id}", (DataService service, int id) =>
{
    return service.GetPost(id);
});

app.MapGet("/api/users/{id}", (DataService service, int id) =>
{
    return service.GetUser(id);
});

app.MapGet("/api/comments", (DataService service) =>
{
    return service.GetComments();
});

app.MapGet("/api/comments/{id}", (DataService service, int id) =>
{
    return service.GetComment(id);
});

app.MapPost("/api/users", (DataService service, NewUserData data) =>
{
    string result = service.CreateUser(data.UserId, data.Name );

    return result;
});

app.MapPost("/api/comments", (DataService service, NewCommentData data) =>
{
    return service.CreateComment(data.Text, data.UserId,  data.PostId);
});

app.MapPost("/api/posts", (DataService service, NewPostData data) =>
{
    string result = service.CreatePost(data.Text, data.User,  data.Title);
    
    return result;
});

app.MapPut("/api/posts/{id}/upvote", (DataService service, int id) =>
{
    string result = service.UpvotePost(id);
    return result;
});

app.MapPut("/api/posts/{id}/downvote", (DataService service, int id) =>
{
    string result = service.DownvotePost(id);
    return result;
});

app.MapPut("/api/comments/{id}/upvote", (DataService service, int id) =>
{
    string result = service.UpvoteComment(id);
    return result;
});

app.MapPut("/api/comments/{id}/downvote", (DataService service, int id) =>
{
    string result = service.DownvoteComment(id);
    return result;
});



app.Run();

record NewCommentData (string Text, int UserId, int PostId);
record NewPostData(string Text, User User, string Title);

record NewUserData(int UserId, string Name);
