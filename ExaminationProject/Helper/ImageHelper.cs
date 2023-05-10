namespace Web.Helper
{
    public static class ImageHelper
    {
        public static string UploadImage(IFormFile image, IWebHostEnvironment _env)
        {
            var path = "/uploads/" + Guid.NewGuid() + image.FileName;
            using (var fileStream = new FileStream(_env.WebRootPath + path, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
            return path;
        }
    }
}
