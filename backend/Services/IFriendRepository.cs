public interface IFriendRepository
{
    Task<bool> AddFriend(Guid userId, Guid friendId);
    Task<bool> RemoveFriend(Guid userId, Guid friendId);
    Task<List<User>> GetFriends(Guid userId);
    //Task<List<User>> GetFriendRequests(Guid userId);
    //Task<bool> AcceptFriendRequest(Guid userId, Guid friendId);
    //Task<bool> DeclineFriendRequest(Guid userId, Guid friendId);
    //Task<bool> SendFriendRequest(Guid userId, Guid friendId);
}