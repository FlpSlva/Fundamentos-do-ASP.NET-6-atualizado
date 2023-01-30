using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Blog.Domain.Entities
{
    public class User
    {
        private IList<Post> _posts;
        private IList<Role> _roles;
        


        public User(string name, string email, string image, string slug, string bio)
        {
           
            Name = name;
            Email = email;
            Image = image;
            Slug = slug;
            Bio = bio;
            _posts = new List<Post>();
            _roles = new List<Role>();
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        [JsonIgnore]
        public string PasswordHash { get; private set; }
        public string Image { get; private set; }
        public string Slug { get; private set; }
        public string Bio { get; private set; }
        public IReadOnlyCollection<Post> Posts { get { return _posts.ToArray(); } }
        public IReadOnlyCollection<Role> Roles { get { return _roles.ToArray(); } }
        public void SetPasswordHash(string password) => PasswordHash = password;
        public void AddPosts(Post post) => _posts.Add(post);
        public void AddRole(Role role) => _roles.Add(role);
        public void UpdatedImage(string url) => Image = url;

    }
}