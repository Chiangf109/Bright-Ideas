using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace CSharp.Models{
    public class Idea: BaseEntity{
        public int IdeaId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public string IdeaText { get; set; }
        public List<Like> Likes { get; set; }

        public Idea(){
            Likes = new List<Like>();
        }
    }
}