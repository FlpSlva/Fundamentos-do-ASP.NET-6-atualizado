using System.Collections.Generic;

namespace Blog.Domain.Entities
{
    public class Role
    {
        private IList<User> _users;

        public Role(string name, string slug)
        {
            
            Name = name;
            Slug = slug;
            _users = new List<User>();
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }

        public IReadOnlyCollection<User> Users { get { return _users.ToArray(); } }
    }
}