using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models.Tables;
using Microsoft.Extensions.Logging;

namespace MuseMusic.Controllers.AdminManagement;

[Route("admin")]
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
    [Route("upload-image")]
    public async Task<IActionResult> UploadImage(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest(new { Error = "No files selected." });
        }

        var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "product");
        Directory.CreateDirectory(uploadDirectory);

        var fileUrls = new List<string>();

        foreach (var file in files)
        {
            try
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    var filePath = Path.Combine(uploadDirectory, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var fileUrl = $"/images/product/{uniqueFileName}";
                    fileUrls.Add(fileUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {file.FileName}. Exception: {ex.Message}");
                return StatusCode(500, new { Error = "Error uploading image" });
            }
        }

        return Ok(new { FileUrls = fileUrls });
    }
}