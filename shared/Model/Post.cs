using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace shared.Model
{
    public class Post
    {
  
        public Post(User user, string title = "", string text = "")
        {
            this.Title = title;
            this.Text = text;
            this.User = user;
            this.Time = DateTime.Now;
            this.Comments = new List<Comment>();
            this.User = new User { Name = "", UserId = 0 };


        }
        public Post() {

            Title = "";
            Text = "";
            User = null;
            Downvotes = 0;
            Upvotes = 0;
        }

        public int PostId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public int Upvotes { get; set; }

        public int Downvotes { get; set; }
        public DateTime Time { get; set; }
        public List<Comment> Comments { get; set; }
        
        public void UpvotePost()
        {  
            Upvotes++;  
        }
        public void DownvotePost()
        {
            Downvotes++;
        }

    }

}
