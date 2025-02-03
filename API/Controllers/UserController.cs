using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Blazor;
namespace ApiEFmysql.Controllers
{
    [Route("api/users")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetListUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving users: {ex.Message}");
            }
        }


        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _userRepository.GetUsersByIdAsync(id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving user: {ex.Message}");
            }
        }

        [HttpPost("Registration")]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserRegistrationRequest registrationRequest)
        {
            try
            {
                var user = registrationRequest.User;
                var password = registrationRequest.Password;

                var createdUser = await _userRepository.CreateUserAsync(user, password);

                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.user_id }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }

        [HttpPut("UpdateUserPassword/{id}")]
        public async Task<ActionResult<User>> UpdateUserPassword(int id, [FromBody] UserRegistrationRequest registrationRequest)
        {
            try
            {
                var user = registrationRequest.User;
                var password = registrationRequest.Password;
                var existingUser = await _userRepository.GetUsersByIdAsync(id);

                if (existingUser == null)
                {
                    return NotFound($"User with ID {id} not found");
                }
                existingUser.user_picture = user.user_picture;
                existingUser.user_phone = user.user_phone;
                existingUser.user_fullname = user.user_fullname;
                existingUser.user_isadmin = user.user_isadmin;
                await _userRepository.UpdateUserPasswordAsync(existingUser, password);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating user: {ex.Message}");
            }
        }
        [HttpPut("UpdateUser/{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                var existingUser = await _userRepository.GetUsersByIdAsync(id);

                if (existingUser == null)
                {
                    return NotFound($"User with ID {id} not found");
                }
                existingUser.user_picture = user.user_picture;
                existingUser.user_phone = user.user_phone;
                existingUser.user_fullname = user.user_fullname;
                existingUser.user_isadmin = user.user_isadmin;
                await _userRepository.UpdateUserAsync(existingUser);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating user: {ex.Message}");
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var userToDelete = await _userRepository.GetUsersByIdAsync(id);

                if (userToDelete == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                await _userRepository.DeleteUserAsync(userToDelete);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting user: {ex.Message}");
            }
        }
        [HttpGet("Picture/{id}")]
        public async Task<ActionResult> GetUserPicture(int id)
        {
            try
            {
                var user = await _userRepository.GetUsersByIdAsync(id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                // Assuming user_picture is a byte array
                return File(user.user_picture, "image/jpeg"); // Adjust the content type accordingly
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving user picture: {ex.Message}");
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login([FromBody] UserLoginRequest loginRequest)
        {
            try
            {
                var phone = loginRequest.Phone;
                var password = loginRequest.Password;

                var user = await _userRepository.AuthenticateAsync(phone, password);

                if (user != null)
                {
                    return user;
                }
                else
                {
                    return Unauthorized("Invalid phone or password");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error during authentication: {ex.Message}");
            }
        }
    }
}
