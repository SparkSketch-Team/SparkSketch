
public class DTRequest
{
    public int draw { get; set; }
    public int length { get; set; }
    public int orderColumn { get; set; }
    public bool orderIsAscending { get; set; }
    public int start { get; set; }
    public string search { get; set; } = null!;
}