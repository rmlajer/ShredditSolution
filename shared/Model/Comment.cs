namespace shared.Model
{
    public class Comment
    {
        
        public Comment(string text = "", int userId = 0, int postId = 0)
        {
            this.Text = text;
            this.UserId = userId;
            this.PostId = postId;
            this.Time = DateTime.Now;
            //this.User = new User { Name = "", UserId = 0 };
        }

        public Comment() { }

        public int CommentId { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int Upvotes { get; set; }

        public int Downvotes { get; set; }
        public DateTime Time { get; set; }

        public void UpvoteComment()
        {
            Upvotes++;
        }
        public void DownvoteComment()
        {
            Downvotes++;
        }

    }
}
