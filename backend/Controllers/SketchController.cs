
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SketchController : ApiController {
    private readonly ISketchRepository _sketchRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IUserRepository _userRepository;

    public SketchController(ISketchRepository sketchRepository, ICommentRepository commentRepository, ILikeRepository likeRepository, IUserRepository userRepository, ILogger<SketchController> logger) : base(logger) {
        _sketchRepository = sketchRepository;
        _commentRepository = commentRepository;
        _likeRepository = likeRepository;
        _userRepository = userRepository;
    }

    [HttpPost("addLike/{postId}")]
    public async Task<IActionResult> CreateLike(int postId)
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == SparkSketchClaims.UserId)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return FailMessage("Unauthorized, your user Id is " + currentUserId + ", check ur db to confirm this is correct");
        }

        var like = new Like
        {
            PostID = postId,
            UserID = Guid.Parse(currentUserId)
        };

        var createdLike = await _likeRepository.CreateLikeAsync(like);
        return Ok(createdLike);
    }

    [HttpDelete("removeLike/{likeId}")]
    public async Task<IActionResult> RemoveLike(int likeId)
    {
        var result = await _likeRepository.RemoveLikeAsync(likeId);
        if (!result)
        {
            return NotFound();
        }
        return SuccessMessage();
    }

    [HttpGet("getLikes/{postId}")]
    public async Task<IActionResult> GetLikes(int postId)
    {
        var likes = await _likeRepository.GetLikesByPostIdAsync(postId);
        return SuccessMessage(likes);
    }

    [HttpPost("addComment/{postId}")]
    public async Task<IActionResult> CreateComment(int postId, [FromBody] string content)
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == SparkSketchClaims.UserId)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var sketch = await _sketchRepository.GetSketchByIdAsync(postId);
        if (sketch == null)
        {
            return NotFound("Sketch not found.");
        }

        // Retrieve the User associated with the CommenterID
        var user = await _userRepository.GetUserByIdAsync(Guid.Parse(currentUserId));
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var comment = new Comment
        {
            PostID = postId,
            CommenterID = Guid.Parse(currentUserId),
            Content = content,
            Sketch = sketch,
            User = user,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow 
        };

        var createdComment = await _commentRepository.CreateCommentAsync(comment);
        return SuccessMessage(createdComment);
    }

    [HttpGet("getCommentsByPost/{postId}")]
    public async Task<IActionResult> GetCommentsByPost(int postId)
    {
        var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);
        return SuccessMessage(comments);
    }

    //[HttpGet("user/{userId}/comments")]
    //public async Task<IActionResult> GetCommentsByUser(Guid userId)
    //{
    //    var comments = await _commentRepository.GetCommentsByUserIdAsync(userId);
    //    return Ok(comments);
    //}

    [HttpDelete("deleteComment/{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var result = await _commentRepository.DeleteCommentAsync(commentId);
        if (!result)
        {
            return NotFound();
        }
        return SuccessMessage();
    }
}
