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
        
        [HttpGet("GetFriendsByUserId/{UserId:guid}")]
        public async Task<IActionResult> GetFriendsByUserId(Guid UserId)
        {
            return this.Ok(await this.context.Friend.Where(x => x.UserId == UserId).FirstOrDefaultAsync());
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

        [HttpDelete("{FriendId:guid}")]
        public async Task<IActionResult> Delete(Guid FriendId)
        {
            var Friend = await this.context.Friend.Where(x => x.FriendId == FriendId).FirstAsync();
            this.context.Friend.Remove(Friend);
            return this.Ok(await this.context.SaveChangesAsync());
        }
    }
}
