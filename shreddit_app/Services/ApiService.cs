using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

using shared.Model;

namespace shreddit_app.Data;

public class ApiService
{
    private readonly HttpClient http;
    private readonly IConfiguration configuration;
    private readonly string baseAPI = "";

    public ApiService(HttpClient http, IConfiguration configuration)
    {
        this.http = http;
        this.configuration = configuration;
        this.baseAPI = configuration["base_api"];
    }

    public async Task<Post[]> GetPosts()
    {
        string url = $"{baseAPI}posts/";
        return await http.GetFromJsonAsync<Post[]>(url);
    }

    public async Task<Post> GetPost(int id)
    {
        string url = $"{baseAPI}posts/{id}/";
        return await http.GetFromJsonAsync<Post>(url);
    }

    public async Task<Comment[]> GetComments()
    {
        string url = $"{baseAPI}comments/";
        return await http.GetFromJsonAsync<Comment[]>(url);
    }

    public async Task<User> GetUser(int id)
    {
        string url = $"{baseAPI}users/{id}/";
        return await http.GetFromJsonAsync<User>(url);
    }

    public async Task<Comment> GetComment(int id)
    {
        string url = $"{baseAPI}comments/{id}/";
        return await http.GetFromJsonAsync<Comment>(url);
    }

    public async Task<User> CreateUser(int userId, string name )
    {
        string url = $"{baseAPI}users";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { userId, name });

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Comment object
        User? newUser = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new user 
        return newUser;
    }

    public async Task<Comment> CreateComment(string text, int userId, int postId)
    {
        string url = $"{baseAPI}comments";
     
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { text, userId, postId });

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Comment object
        Comment? newComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new comment 
        return newComment;
    }

    public async Task<Post> UpvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/upvote";
       
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");
        

        /* Didn't commented code working
        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;
       
        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        */
        Post updatedPost = await GetPost(id);
        return updatedPost;
    }

    public async Task<Post> DownvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/downvote";
     
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        /*
        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        */
        Post updatedPost = await GetPost(id);
        return updatedPost;
    }

    public async Task<Comment> UpvoteComment(int id)
    {
        string url = $"{baseAPI}comments/{id}/upvote";
     
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");
        /*
        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });
        */
        // Return the updated post (vote increased)
        Comment updatedPost = await GetComment(id);
        return updatedPost;
    }

    public async Task<Comment> DownvoteComment(int id)
    {
        string url = $"{baseAPI}comments/{id}/downvote";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        /*
        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });
        */
        // Return the updated post (vote increased)
        Comment updatedComment = await GetComment(id);
        return updatedComment;
    }
}
