﻿namespace Books.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
