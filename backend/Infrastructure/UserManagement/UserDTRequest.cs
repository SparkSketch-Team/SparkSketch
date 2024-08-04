using Azure.Core;

public class UserDTRequest
{
    public DTRequest dTRequest { get; set; } = null!;
    public bool? isActive { get; set; }
}