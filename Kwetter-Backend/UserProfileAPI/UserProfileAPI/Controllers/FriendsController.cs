using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Models;

namespace UserProfileAPI.Controllers
{
    [ApiController]
    [Route("api/userprofileapi/[controller]")]
    public class FriendController : Controller
    {
        private readonly ApplicationContext context;

        public FriendController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.context.Friend.ToListAsync());
        }

        [HttpGet("GetById/{FriendId:guid}")]
        public async Task<IActionResult> GetById(Guid FriendId)
        {
            return this.Ok(await this.context.Friend.Where(x => x.FriendId == FriendId).FirstOrDefaultAsync());
        }
        
        [HttpGet("GetFollowingByUserId/{UserId}")]
        public async Task<IActionResult> GetFollowingByUserId(string UserId)
        {
            try
            {
                if (this.context == null)
                {
                    return this.BadRequest("Error: Database context is null.");
                }

              
                var result = await this.context.Friend.Where(x => x.UserId == UserId).ToListAsync();

                if (result == null || !result.Any())
                {
                    return this.NotFound(new { error = "No data found", message = "The requested resource was not found in the database." });
                }

                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        
        [HttpGet("GetFollowersByUserId/{UserId}")]
        public async Task<IActionResult> GetFollowersByUserId(string UserId)
        {
            try
            {
                if (this.context == null)
                {
                    return this.BadRequest("Error: Database context is null.");
                }


                var result = await this.context.Friend.Where(x => x.FriendsWith == UserId).ToListAsync();

                if (result == null || !result.Any())
                {
                    return this.NotFound(new { error = "No data found", message = "The requested resource was not found in the database." });
                }

                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(Friend Friend)
        {
            await this.context.Friend.AddAsync(Friend);
            await this.context.SaveChangesAsync();
            return this.Ok(Friend);
        }

        [HttpPut("")]
        public async Task<IActionResult> Update(Friend Friend)
        {
            this.context.Friend.Update(Friend);
            await this.context.SaveChangesAsync();
            return this.Ok(Friend);
        }

        [HttpDelete("{UserId}/{FriendId}")]
        public async Task<IActionResult> Delete(string UserId,string FriendId)
        {
            var Friend = await this.context.Friend.Where(x => x.UserId == UserId && x.FriendsWith == FriendId).FirstAsync();
            this.context.Friend.Remove(Friend);
            return this.Ok(await this.context.SaveChangesAsync());
        }
    }
}
