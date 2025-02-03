using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEFmysql.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostRepository _postRepository;

        public PostController(PostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet("thread/{threadId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllPostsInThread(int threadId)
        {
            try
            {
                var posts = await _postRepository.GetAllPostsAsync(threadId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving posts in thread: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(id);

                if (post == null)
                {
                    return NotFound($"Post with ID {id} not found");
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving post: {ex.Message}");
            }
        }

        [HttpPost("CreatePost")]
        public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
        {
            try
            {
                var createdPost = await _postRepository.CreatePostAsync(post);
                return CreatedAtAction(nameof(GetPostById), new { id = createdPost.post_id }, createdPost);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating post: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Post>> UpdatePost(int id, [FromBody] Post post)
        {
            try
            {
                var existingPost = await _postRepository.GetPostByIdAsync(id);

                if (existingPost == null)
                {
                    return NotFound($"Post with ID {id} not found");
                }
                existingPost.post_content = post.post_content;

                await _postRepository.UpdatePostAsync(post);

                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating post: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                var postToDelete = await _postRepository.GetPostByIdAsync(id);

                if (postToDelete == null)
                {
                    return NotFound($"Post with ID {id} not found");
                }

                await _postRepository.DeletePostAsync(postToDelete);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting post: {ex.Message}");
            }
        }
    }
}
