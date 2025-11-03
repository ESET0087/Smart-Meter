
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;

namespace smart_meter.Services
{
    public class ConsumerService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ConsumerService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Add Consumer
        public async Task<Consumer> AddConsumerAsync(ConsumerCreateDto dto)
        {
            var consumer = new Consumer
            {
                Name = dto.Name,
               // Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                Orgunitid = dto.Orgunitid,
                Tariffid = dto.Tariffid,
                Status = "Active",
                Createdat = DateTime.UtcNow,
                Createdby = "System", // Or from logged-in user
                Isdeleted = false
            };

            _context.Consumers.Add(consumer);
            await _context.SaveChangesAsync();

            return consumer;
        }

        // Upload Consumer Photo
        public async Task<string> UploadPhotoAsync(ConsumerPhotoUploadDto dto)
        {
            var consumer = await _context.Consumers.FindAsync(dto.ConsumerId);
            if (consumer == null)
                return null; // Consumer not found

            if (dto.File == null || dto.File.Length == 0)
                return null; // No file uploaded

            // Allow only images
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.File.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return null; // Invalid file type

            // Create uploads folder
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique file name
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Update consumer photo in DB
            consumer.Photo = $"/uploads/{uniqueFileName}";
            consumer.Updatedat = DateTime.UtcNow;
            consumer.Updatedby = "Consumer";

            await _context.SaveChangesAsync();

            return consumer.Photo; // Return the URL of the uploaded photo
        }
    }
}
