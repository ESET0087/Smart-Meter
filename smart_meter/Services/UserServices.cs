using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;
using System.Text;

namespace smart_meter.Services
{
    public class UserServices
    {
        public readonly AppDbContext _context;

        public UserServices(AppDbContext context)
        {
            _context = context;
        }
        public class UpdateResult
        {
            public bool result { get; set; }
            public string message { get; set; }
        }

        // Add Consumer
        public async Task<(bool success, Consumer consumer)> AddConsumerAsync(ConsumerCreateDto dto, string createdby)
        {
            if (await _context.Consumers.AnyAsync(c => c.Email == dto.Email))
            {
                return (false, null);
            }

            // default password for new user
            string defaultPassword = "abcd";
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(defaultPassword);

            var consumer = new Consumer
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Orgunitid = dto.Orgunitid,
                Tariffid = dto.Tariffid,
                Status = "Active",
                Createdat = DateTime.UtcNow,
                Createdby = createdby ?? "system",
                Password = Encoding.UTF8.GetBytes(passwordHash)
            };

            _context.Consumers.Add(consumer);
            await _context.SaveChangesAsync();

            return (true, consumer);
        }
        
        // update user password
        public async Task<UpdateResult> UpdateUser(int id , UpdatePasswordDto passwordDto)
        {
            try
            {
                var user = _context.User.FirstOrDefault(u => u.Userid == id);
                if (user == null) return new UpdateResult { message = "No user found", result= false};


                if (passwordDto.NewPassword != passwordDto.ConfirmePassword) return new UpdateResult { message = "Confirm password should be same as new password", result = false };



                string hashpassword = BCrypt.Net.BCrypt.HashPassword(passwordDto.ConfirmePassword);
                byte[] encodedhashpassword = Encoding.UTF8.GetBytes(hashpassword);

                string oldpassword = Encoding.UTF8.GetString(user.Passwordhash);

                if (!BCrypt.Net.BCrypt.Verify(passwordDto.oldPassword, oldpassword)) return new UpdateResult { message = "Please Enter correct Old password" };


                user.Passwordhash = encodedhashpassword;

                _context.User.Update(user);
                await _context.SaveChangesAsync();

                return new UpdateResult { message = "Your password has been updated successfully", result = true };

            }
            catch (Exception ex)
            {
                return new UpdateResult{ message = ex.Message, result = false};
            }
        }
    }
}
