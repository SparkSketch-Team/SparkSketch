using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FriendController : ApiController
{
    private readonly IFriendRepository _friendRepository;

    public FriendController(IFriendRepository friendRepository, ILogger<FriendController> logger) : base(logger)
    {
        _friendRepository = friendRepository;
    }

    // POST: api/Friend/Add
    [HttpPost("Add")]
    public async Task<IActionResult> AddFriend([FromBody] Guid followedUserId)
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var isAdded = await _friendRepository.AddFriend(Guid.Parse(currentUserId), followedUserId);

        if (!isAdded)
        {
            return FailMessage("Failed to add friend.");
        }

        return SuccessMessage("Friend added successfully.");
    }

    // DELETE: api/Friend/Remove
    [HttpDelete("Remove")]
    public async Task<IActionResult> RemoveFriend([FromBody] Guid followedUserId)
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var isRemoved = await _friendRepository.RemoveFriend(Guid.Parse(currentUserId), followedUserId);

        if (!isRemoved)
        {
            return FailMessage("Failed to remove friend.");
        }

        return SuccessMessage("Friend removed successfully.");
    }
}
