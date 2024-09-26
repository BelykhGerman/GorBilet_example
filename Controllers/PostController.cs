using GorBilet_example.Data;
using GorBilet_example.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace GorBilet_example.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IData<Post> _posts;

        public PostController(IData<Post> posts)
        {
            _posts = posts;
        }

        [HttpPost("posts")]
        public async Task<IActionResult> CreatePost(Post? post)
        {
            try
            {
                var validation = ModelValidator(ModelState);
                if (!string.IsNullOrEmpty(validation))
                {
                    return BadRequest(validation);
                }
                if (post == null)
                {
                    return BadRequest($"{typeof(Post).Name} object is null");
                }

                var newPost = await _posts.Create(post);
                if (newPost == null)
                {
                    throw new InvalidOperationException("New post is null");
                }
                return Ok(newPost);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("posts/all")]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var allPosts = await _posts.GetAll();
                if (allPosts == null)
                {
                    throw new InvalidOperationException("Db returned null instead.");
                }
                return Ok(allPosts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("posts/{id}")]
        public async Task<IActionResult> GetPost(string id)
        {
            try
            {
                var post = await _posts.GetById(id);
                if (post == null)
                {
                    return NotFound($"Post with id:{id} doesn't exist");
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("posts/{id}")]
        public async Task<IActionResult> UpdatePost(string id, Post? post)
        {
            try
            {
                var validation = ModelValidator(ModelState);
                if (!string.IsNullOrEmpty(validation))
                {
                    return BadRequest(validation);
                }
                if (post == null)
                {
                    return BadRequest($"{typeof(Post).Name} object is null");
                }
                post.Id = id;
                if (await _posts.Update(post) == null)
                {
                    return NotFound($"Post with id:{post.Id} doesn't exist");
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("posts/{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            try
            {
                if (await _posts.Delete(id) == null)
                {
                    return NotFound($"Post with id:{id} doesn't exist");
                }
                return Ok($"Post with id:{id} successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        private string? ModelValidator(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Check fields: ");
                sb.AppendJoin(',', modelState.Keys);
                return $"Object is invalid. {sb.ToString()}.";
            }
            return null;
        }
    }
}
