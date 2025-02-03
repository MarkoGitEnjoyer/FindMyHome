using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thread = Data.Models.Thread;

namespace ApiEFmysql.Controllers
{
    [Route("api/threads")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        private readonly ThreadRepository _threadRepository;

        public ThreadController(ThreadRepository threadRepository)
        {
            _threadRepository = threadRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thread>>> GetThreads()
        {
            try
            {
                var threads = await _threadRepository.GetThreadsAsync();
                return Ok(threads);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving threads: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Thread>> GetThreadById(int id)
        {
            try
            {
                var thread = await _threadRepository.GetThreadByIdAsync(id);

                if (thread == null)
                {
                    return NotFound($"Thread with ID {id} not found");
                }

                return Ok(thread);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving thread: {ex.Message}");
            }
        }

        [HttpGet("Threads/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Thread>>> GetThreadsByCategory(int categoryId)
        {
            try
            {
                var threadsInCategory = await _threadRepository.GetThreadsByCategoryAsync(categoryId);
                return Ok(threadsInCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving threads by category: {ex.Message}");
            }
        }

        [HttpPost("CreateThread")]
        public async Task<ActionResult<Thread>> CreateThread([FromBody] Thread thread)
        {
            try
            {
                var createdThread = await _threadRepository.CreateThreadAsync(thread);
                return CreatedAtAction(nameof(GetThreadById), new { id = createdThread.thread_id }, createdThread);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating thread: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Thread>> UpdateThread(int id, [FromBody] Thread thread)
        {
            try
            {
                var existingThread = await _threadRepository.GetThreadByIdAsync(id);

                if (existingThread == null)
                {
                    return NotFound($"Thread with ID {id} not found");
                }
                existingThread.thread_title = thread.thread_title;

                await _threadRepository.UpdateThreadAsync(thread);

                return Ok(thread);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating thread: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteThread(int id)
        {
            try
            {
                var threadToDelete = await _threadRepository.GetThreadByIdAsync(id);

                if (threadToDelete == null)
                {
                    return NotFound($"Thread with ID {id} not found");
                }

                await _threadRepository.DeleteThreadAsync(threadToDelete);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting thread: {ex.Message}");
            }
        }
    }
}