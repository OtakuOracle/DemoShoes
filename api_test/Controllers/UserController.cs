using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using DemoTest.Models;

namespace api_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TrenirovkaContext _context;

        public UserController(TrenirovkaContext context)
        {
            _context = context;
        }


        [HttpGet(Name = "UserGet")]

        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = _context.Users.ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("id", Name = "GetUserById")]

        public async Task<IActionResult> GetUserById(int id) 
        {
            try
            {
                var user = _context.Users.FindAsync(id);
                return Ok(user);    
            }
            catch (Exception ex) 
            {
                return StatusCode(400, "");
            }
        }

        [HttpDelete("id", Name = "DeleteUser")]

        public async Task <IActionResult> Delete(int id) 
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if(user == null) 
                { 
                    return NotFound("Пользователь не найден");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex) 
            {
                return StatusCode(400, "");
            }
        }


        [HttpPost(Name = "CreateUser")]

        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                if(user == null)
                {
                    return BadRequest("Пустой пользователь");
                }

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(user);
            }

            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("id", Name = "UpdateUser")]

        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                if(user == null)
                {
                    return BadRequest("User is null");
                }

                if (id != user.UserId)
                {
                    return BadRequest("User ID mismatch");
                }

                var existingUser = await _context.Users.FindAsync(id);


                if (existingUser == null) 
                {
                    return NotFound($"User with id {id} not found");
                }

                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;


                _context .Entry(existingUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(existingUser);

            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }

    
}
