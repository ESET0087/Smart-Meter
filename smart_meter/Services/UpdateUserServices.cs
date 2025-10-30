using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Model.DTOs;
using System.Text;

namespace smart_meter.Services
{
    public class UpdateUserServices
    {
        public readonly AppDbContext _context;

        public UpdateUserServices(AppDbContext context)
        {
            _context = context;
        }
        public class UpdateResult
        {
            public bool result { get; set; }
            public string message { get; set; }
        }
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
