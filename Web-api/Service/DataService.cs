﻿using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using shreddit.Data;
using shared.Model;

namespace shreddit.Service
{
    public class DataService
    {
        private DataContext db { get; }

        public DataService(DataContext db)
        {
            this.db = db;
        }
        /// <summary>
        /// Seeder noget nyt data i databasen hvis det er nødvendigt.
        /// </summary>
        public void SeedData()
        {

            Post post = db.Posts.FirstOrDefault()!;
            if (post == null)
            {
                User testUser1 = new User { Name = "TestUser1" };
                User testUser2 = new User { Name = "TestUser2" };
                db.Add(testUser1);
                db.Add(testUser2);
                
                db.SaveChanges();
                post = new Post { Title = "TestPost1", Text = "Please", User = testUser1 };
                db.Posts.Add(post);
                db.Posts.Add(new Post { Title = "TestPost2", Text = "Please", User = testUser2 });
                db.SaveChanges();

                db.Comments.Add(new Comment { Text = "TestPostComment1", User = testUser1, PostId = 1 });
                db.Comments.Add(new Comment { Text = "TestPostComment2", User = testUser2, PostId = 1 });
                

            }

            db.SaveChanges();
        }
        
        
        public List<Post> GetPosts()
        {
            return db.Posts.Include(p => p.User).ToList();
        }

        public Post GetPost(int id)
        {
            return db.Posts.Include(p => p.Comments).Include(p => p.User).First(p => p.PostId == id);
        }

        public List<Comment> GetComments()
        {
            return db.Comments.ToList();
        }

        public string CreateComment(string text, User user, int postId)
        {
            db.Comments.Add(new Comment { Text = text, User = user, PostId = postId });
            db.SaveChanges();
            return "Comment added";
        }

        public string CreatePost(string text, User user, string title)
        {
            db.Posts.Add(new Post { Text = text, User = user, Title = title });
            db.SaveChanges();
            return "Post added";
        }

        public string VotePost(bool b, int id)
        {
            db.Posts.First(p => p.PostId == id).UpdateVote(b);
            db.SaveChanges();
            if (b) { return "Post upvoted"; }
            else if (!b) { return "Post downvoted"; }
            else return "No work";
        }

        public string VoteComment(bool b, int id)
        {
            db.Comments.First(p => p.CommentId == id).UpdateVote(b);
            db.SaveChanges();
            if (b) { return "Comment upvoted"; }
            else if (!b) { return "Comment downvoted"; }
            else return "No work";
        }

    }
}

