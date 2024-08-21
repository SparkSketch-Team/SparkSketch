
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SketchController : ApiController {
    private readonly ISketchRepository _sketchRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ILikeRepository _likeRepository;

    public SketchController(ISketchRepository sketchRepository, ICommentRepository commentRepository, ILikeRepository likeRepository, ILogger<SketchController> logger) : base(logger) {
        _sketchRepository = sketchRepository;
        _commentRepository = commentRepository;
        _likeRepository = likeRepository;
    }

    [HttpPost("{postId}/like")]
    public async Task<IActionResult> CreateLike(int postId)
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == SparkSketchClaims.UserId)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var like = new Like
        {
            PostID = postId,
            UserID = Guid.Parse(currentUserId)
        };

        var createdLike = await _likeRepository.CreateLikeAsync(like);
        return Ok(createdLike);
    }

    [HttpDelete("like/{likeId}")]
    public async Task<IActionResult> RemoveLike(int likeId)
    {
        var result = await _likeRepository.RemoveLikeAsync(likeId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("{postId}/likes")]
    public async Task<IActionResult> GetLikes(int postId)
    {
        var likes = await _likeRepository.GetLikesByPostIdAsync(postId);
        return Ok(likes);
    }

    [HttpPost("{postId}/comment")]
    public async Task<IActionResult> CreateComment(int postId, [FromBody] string content)
    {
        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == SparkSketchClaims.UserId)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var comment = new Comment
        {
            PostID = postId,
            CommenterID = Guid.Parse(currentUserId),
            Content = content
        };

        var createdComment = await _commentRepository.CreateCommentAsync(comment);
        return Ok(createdComment);
    }

    [HttpGet("{postId}/comments")]
    public async Task<IActionResult> GetCommentsByPost(int postId)
    {
        var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);
        return Ok(comments);
    }

    [HttpGet("user/{userId}/comments")]
    public async Task<IActionResult> GetCommentsByUser(Guid userId)
    {
        var comments = await _commentRepository.GetCommentsByUserIdAsync(userId);
        return Ok(comments);
    }

    [HttpDelete("comment/{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var result = await _commentRepository.DeleteCommentAsync(commentId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
