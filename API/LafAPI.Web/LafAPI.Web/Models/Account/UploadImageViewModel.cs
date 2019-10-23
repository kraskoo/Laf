namespace LafAPI.Web.Models.Account
{
    using Microsoft.AspNetCore.Http;

    public class UploadImageViewModel
    {
        public IFormFile File { get; set; }
    }
}