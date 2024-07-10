using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

public class ApiController : ControllerBase
{
    protected JsonResult SuccessMessage(object? data = null)
    {
        var message = new MessageObject()
        {
            success = true,
            results = data
        };
        var result = new JsonResult(message);
        return result;
    }

    protected JsonResult FailMessage(string? failMessage = null, string? ex = null)
    {
        var message = new MessageObject()
        {
            success = false,
            results = failMessage,
            error = ex
        };
        var result = new JsonResult(message);
        return result;
    }

    public class MessageObject
    {
        public object? results { get; set; }
        public bool success { get; set; }
        public string? error { get; set; }
    }
}