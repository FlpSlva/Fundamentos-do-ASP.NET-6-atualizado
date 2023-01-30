using System.Collections.Generic;

namespace Blog.Domain.Entities
{
    public class Category
    {
        private IList<Post> _posts;


        public Category(int id, string name, string slug)
        {
            Id = id;
            Name = name;
            Slug = slug;
            _posts = new List<Post>();
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }

        public IReadOnlyCollection<Post> Posts { get { return _posts.ToArray(); } }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangeSlug(string slug)
        {
            Slug = slug;
        }
    }
}