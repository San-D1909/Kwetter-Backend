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

        [HttpGet("{UserId:guid}")]
        public async Task<IActionResult> GetById(Guid UserId)
        {
            return this.Ok(await this.context.User.Where(x => x.UserId == UserId).FirstOrDefaultAsync());
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(User User)
        {
            await this.context.User.AddAsync(User);
            await this.context.SaveChangesAsync();
            return this.Ok(User);
        }

        [HttpPut("")]
        public async Task<IActionResult> Update(User User)
        {
            this.context.User.Update(User);
            await this.context.SaveChangesAsync();
            return this.Ok(User);
        }

        [HttpDelete("{UserId:guid}")]
        public async Task<IActionResult> Delete(Guid UserId)
        {
            var User = await this.context.User.Where(x => x.UserId == UserId).FirstAsync();
            this.context.User.Remove(User);
            return this.Ok(await this.context.SaveChangesAsync());
        }
    }
}
