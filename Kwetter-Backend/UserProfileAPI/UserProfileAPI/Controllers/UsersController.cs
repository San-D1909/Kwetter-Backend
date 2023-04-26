using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Models;

namespace UserProfileAPI.Controllers
{
    [ApiController]
    [Route("api/userprofileapi/[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationContext context;

        public UsersController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.context.User.ToListAsync());
        }

        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetById(string UserId)
        {
            return this.Ok(await this.context.User.Where(x => x.UserId == UserId).FirstOrDefaultAsync());
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(User user)
        {
            if(user.UserId is null)
            {
                return BadRequest();
            }
            var dbUser = this.context.User.Where(x=>x.UserId == user.UserId).FirstOrDefault();
            if (dbUser != null)
            {
                dbUser.LastLogin = DateTime.Now;
                await this.context.SaveChangesAsync();
                return this.Ok(dbUser);
            }
            user.AddedAt = DateTime.Now;
            await this.context.User.AddAsync(user);
            await this.context.SaveChangesAsync();
            return this.Ok(user);
        }

        [HttpPut("")]
        public async Task<IActionResult> Update(User User)
        {
            this.context.User.Update(User);
            await this.context.SaveChangesAsync();
            return this.Ok(User);
        }

        [HttpDelete("{UserId}")]
        public async Task<IActionResult> Delete(string UserId)
        {
            var User = await this.context.User.Where(x => x.UserId == UserId).FirstAsync();
            this.context.User.Remove(User);
            return this.Ok(await this.context.SaveChangesAsync());
        }
    }
}
