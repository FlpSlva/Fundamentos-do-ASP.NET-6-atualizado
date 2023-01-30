using System.Collections.Generic;

namespace Blog.Domain.Entities
{
    public class Tag
    {
        private IList<Post> _posts;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public IReadOnlyCollection<Post> Posts { get { return _posts.ToArray(); } }
    }
}