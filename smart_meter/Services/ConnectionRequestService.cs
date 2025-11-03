using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace smart_meter.Services
{
    public class ConnectionRequestService
    {
        private readonly AppDbContext _context;

        public ConnectionRequestService(AppDbContext context)
        {
            _context = context;
        }

        // Create a new connection/disconnection request
        public async Task<ConnectionRequest> CreateRequestAsync(ConnectionRequestDto dto)
        {
            var request = new ConnectionRequest
            {
                ConsumerId = dto.ConsumerId,
                Name = dto.Name,
                Email = dto.Email,
                Address = dto.Address,
                RequestType = dto.RequestType,
                Remarks = dto.Remarks,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.ConnectionRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        // Get all requests (admin side)
        public async Task<List<ConnectionRequest>> GetAllRequestsAsync()
        {
            return await _context.ConnectionRequests
                .Include(r => r.Consumer)
                .Include(r => r.ActionByUser)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // Get requests for a specific consumer
        public async Task<List<ConnectionRequest>> GetRequestsByConsumerIdAsync(long consumerId)
        {
            return await _context.ConnectionRequests
                .Where(r => r.ConsumerId == consumerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // Admin approves/rejects a request
        public async Task<bool> UpdateRequestStatusAsync(long requestId, string status, long userId, string? remarks = null)
        {
            var request = await _context.ConnectionRequests.FindAsync(requestId);
            if (request == null) return false;

            request.Status = status;
            request.ActionBy = userId;
            request.ApprovedAt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(remarks))
                request.Remarks = remarks;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
