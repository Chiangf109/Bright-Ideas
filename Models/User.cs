using System.Collections.Generic;

namespace CSharp.Models{
    public class User: BaseEntity{
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Idea> Ideas { get; set; }
        public List<Like> Likes { get; set; }

        public User(){
            Ideas = new List<Idea>();
            Likes = new List<Like>();
        }
    }
}