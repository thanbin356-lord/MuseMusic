using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.Tables;

namespace MuseMusic.Controllers.AdminManagement
{
    [Route("admin/upload-image")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly shopmanagementContext _context;
        private readonly ILogger<ImageUploadController> _logger;

        public ImageUploadController(shopmanagementContext context, ILogger<ImageUploadController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages([FromForm] IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
            {
                _logger.LogWarning("No files received for upload.");
                return BadRequest(new { Message = "No files received." });
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "product");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
                _logger.LogInformation("Created upload directory at {UploadPath}", uploadPath);
            }

            var fileUrls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    try
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var fileExtension = Path.GetExtension(file.FileName);
                        var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";
                        var filePath = Path.Combine(uploadPath, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var fileUrl = $"/images/product/{uniqueFileName}";
                        fileUrls.Add(fileUrl);

                        _logger.LogInformation("Successfully uploaded file: {FileName} to {FileUrl}", file.FileName, fileUrl);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to upload file: {FileName}", file.FileName);
                        return StatusCode(500, new { Message = "Error uploading one or more files." });
                    }
                }
                else
                {
                    _logger.LogWarning("Skipped empty file: {FileName}", file.FileName);
                }
            }

            return Ok(new { FileUrls = fileUrls });
        }

        [HttpDelete("delete-image/{id}")]
        public IActionResult DeleteImage(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid image ID provided for deletion.");
                return BadRequest(new { Message = "Invalid image ID." });
            }

            try
            {
                // Validate the file name
                id = Path.GetFileName(id); // Prevent directory traversal attacks
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "product", id);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                    _logger.LogInformation("Image deleted successfully: {ImageId}", id);

                    // If there is a database record associated with the image, handle its deletion here
                    var imageRecord = _context.ImageUrls.FirstOrDefault(img => img.Url.EndsWith(id));
                    if (imageRecord != null)
                    {
                        _context.ImageUrls.Remove(imageRecord);
                        _context.SaveChanges();
                        _logger.LogInformation("Database record for image {ImageId} removed.", id);
                    }

                    return Ok(new { Message = "Image deleted successfully." });
                }
                else
                {
                    _logger.LogWarning("Image not found: {ImageId}", id);
                    return NotFound(new { Message = "Image not found." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting image: {ImageId}", id);
                return StatusCode(500, new { Message = "An error occurred while deleting the image." });
            }
        }
    }
}