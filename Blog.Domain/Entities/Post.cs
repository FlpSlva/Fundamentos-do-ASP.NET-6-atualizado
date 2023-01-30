using System;
using System.Collections.Generic;

namespace Blog.Domain.Entities
{
    public class Post
    {
        private IList<Tag> _tags;
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public string Body { get; private set; }
        public string Slug { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }
        public Category Category { get; private set; }
        public User Author { get; private set; }

        public IReadOnlyCollection<Tag> Tags { get { return _tags.ToArray(); } }
        
    }
}