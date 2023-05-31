using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserProfileAPI.Models;
using UserProfileAPI.Services;
using UserProfileAPI.Services.Interfaces;

namespace UserProfileAPI.Controllers
{
    [ApiController]
    [Route("api/userprofileapi/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await this.usersRepository.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception or perform other error handling actions
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(string userId)
        {
            try
            {
                var user = await this.usersRepository.GetUserById(userId);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception or perform other error handling actions
                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                if (user.UserId is null)
                    return BadRequest();

                var dbUser = await this.usersRepository.GetUserById(user.UserId);
                if (dbUser != null)
                {
                    dbUser.LastLogin = DateTime.Now;
                    await this.usersRepository.UpdateUser(dbUser);
                    return Ok(dbUser);
                }

                user.AddedAt = DateTime.Now;
                var createdUser = await this.usersRepository.CreateUser(user);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                // Log the exception or perform other error handling actions
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }

        [HttpPut("")]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                var updatedUser = await this.usersRepository.UpdateUser(user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                // Log the exception or perform other error handling actions
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                var deletedRows = await this.usersRepository.DeleteUser(userId);
                if (deletedRows == 0)
                    return NotFound();

                // Perform additional cleanup or actions
                PublisherService publisherService = new PublisherService();
                publisherService.DeleteUser(userId);

                return Ok(deletedRows);
            }
            catch (Exception ex)
            {
                // Log the exception or perform other error handling actions
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }
    }
}
