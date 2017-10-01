using System;

namespace CSharp.Models{
    public abstract class BaseEntity{
        public BaseEntity(){
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}