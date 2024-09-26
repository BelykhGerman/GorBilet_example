using GorBilet_example.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace GorBilet_example.Data
{
    public class PostData : IData<Post>
    {
        private readonly IDatabase _db;

        public PostData(IConnectionMultiplexer connection)
        {
            _db = connection.GetDatabase();
        }

        public async Task<Post?> Create(Post post)
        {
            post.Id = Guid.NewGuid().ToString();
            var post_json = JsonSerializer.Serialize(post);
            await _db.HashSetAsync("postsdb", [
                new HashEntry(post.Id, post_json)
            ]);

            return post;
        }

        public async Task<IEnumerable<Post?>?> GetAll()
        {
            return Array.ConvertAll(await _db.HashGetAllAsync("postsdb"), posts => JsonSerializer.Deserialize<Post>(posts.Value.ToString()));
        }

        public async Task<Post?> GetById(dynamic id)
        {
            var post = await _db.HashGetAsync("postsdb", id);
            if (post.IsNull)
            {
                return null;
            }
            var found = JsonSerializer.Deserialize<Post>(post);
            return found;
        }

        public async Task<Post?> Update(Post post)
        {
            var storagedPost = await _db.HashGetAsync("postsdb", post.Id);
            if (storagedPost.IsNull)
            {
                return null;
            }
            var post_json = JsonSerializer.Serialize(post);
            await _db.HashSetAsync("postsdb", [
                new HashEntry(post.Id, post_json)
            ]);
            return post;
        }

        public async Task<bool?> Delete(dynamic id)
        {
            if (!await _db.HashExistsAsync("postsdb", id))
            {
                return null;
            }
            return await _db.HashDeleteAsync("postsdb", id);
        }
    }
}
