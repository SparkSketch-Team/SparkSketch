using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FriendRepository : BaseRepository, IFriendRepository
{
    public FriendRepository(IConfiguration config) : base(config)
    {
    }

    public async Task<bool> AddFriend(Guid userId, Guid friendId)
    {
        var follower = new Follower
        {
            FollowedUserID = friendId,
            FollowingUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        db.Followers.Add(follower);

        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveFriend(Guid userId, Guid friendId)
    {
        var follower = await db.Followers
            .FirstOrDefaultAsync(f => f.FollowingUserID == userId && f.FollowedUserID == friendId);

        if (follower == null)
        {
            return false;
        }

        db.Followers.Remove(follower);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<List<User>> GetFriends(Guid userId)
    {
        return await db.Followers
            .Where(f => f.FollowingUserID == userId)
            .Select(f => f.User)  // Assuming that Follower.User refers to the followed User entity
            .ToListAsync();
    }
}
