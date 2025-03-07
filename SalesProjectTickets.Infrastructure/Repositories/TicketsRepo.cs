using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Enums;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using SalesProjectTickets.Infrastructure.Contexts;

namespace SalesProjectTickets.Infrastructure.Repositories
{
    public class TicketsRepo(ContextsDaBa context, IHttpContextAccessor httpContextAccessor, Cloudinary cloudinary) : IRepoTickets<Tickets>
    {
        private readonly ContextsDaBa _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly Cloudinary _cloudinary = cloudinary;

        public async Task<Tickets> Add(Tickets entity, IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                throw new ValidationException("Imagen requerida");
            }

            var uploadsParams = new ImageUploadParams()
            {
                File = new FileDescription(formFile.FileName, formFile.OpenReadStream()),
                AssetFolder = "ApiTicketsConciertos"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadsParams);

            if (uploadResult == null || string.IsNullOrWhiteSpace(uploadResult.SecureUrl.ToString()))
            {
                throw new ValidationException("Error al subir la imagen");
            }

            entity.ImageUrl = uploadResult.SecureUrl.ToString();

            await _context.Tickets.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task ChangeStateByValidation(Tickets entity)
        {
            var ticketsForChange = await _context.Tickets.FirstOrDefaultAsync(tickets => tickets.Id == entity.Id);

            if (ticketsForChange != null)
            {
                ticketsForChange.State = entity.State;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(Guid entityID)
        {
            var TickestForDelete = await _context.Tickets.FirstOrDefaultAsync(tickets => tickets.Id == entityID);
            if (TickestForDelete != null)
            {
                if (!string.IsNullOrWhiteSpace(TickestForDelete.ImageUrl))
                {
                    var publicId = GetPublicIdFromUrl(TickestForDelete.ImageUrl);
                    if (!string.IsNullOrEmpty(publicId))
                    {
                        var deletionParams = new DeletionParams(publicId);
                        await _cloudinary.DestroyAsync(deletionParams);
                    }
                }
                _context.Tickets.Remove(TickestForDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Edit(Tickets entity, IFormFile? formFile)
        {
            var TickestForEdit = await _context.Tickets.FirstOrDefaultAsync(tickets => tickets.Id == entity.Id);

            if (TickestForEdit != null)
            {
                TickestForEdit.Name = entity.Name;
                TickestForEdit.Description = entity.Description;
                TickestForEdit.Quantity = entity.Quantity;
                TickestForEdit.Price = entity.Price;
                TickestForEdit.Event_date = entity.Event_date;
                TickestForEdit.Event_location = entity.Event_location;
                TickestForEdit.Event_time = entity.Event_time;

                if (formFile == null)
                {
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(TickestForEdit.ImageUrl))
                    {
                        var publicId = GetPublicIdFromUrl(TickestForEdit.ImageUrl);

                        if (!string.IsNullOrEmpty(publicId))
                        {
                            var deletionParams = new DeletionParams(publicId);
                            await _cloudinary.DestroyAsync(deletionParams);
                        }
                    }

                    var uploadsParams = new ImageUploadParams()
                    {
                        File = new FileDescription(formFile?.FileName, formFile?.OpenReadStream()),
                        AssetFolder = "ApiTicketsConciertos"
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadsParams);

                    if (uploadResult == null || string.IsNullOrWhiteSpace(uploadResult.SecureUrl.ToString()))
                    {
                        throw new ValidationException("Error al subir la imagen");
                    }

                    entity.ImageUrl = uploadResult.SecureUrl.ToString();

                    TickestForEdit.ImageUrl = entity.ImageUrl;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<Tickets>> ListAllTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<bool> ListByPermissions(Permissions entityPermi)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var permissionsClaim = user?.Claims.FirstOrDefault(c => c.Type == "Permissions");

            return await Task.FromResult(entityPermi switch
            {
                Permissions.Administrador => permissionsClaim?.Value == "Administrador",
                Permissions.Consumidor => permissionsClaim?.Value == "Consumidor",
                _ => false
            });
        }

        public async Task<Tickets> SelectionById(Guid entity)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(ticket => ticket.Id == entity);
            return ticket ?? throw new ValidationException("No se encontro el ticket");
        }

        private static string GetPublicIdFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.Segments;
            var fileName = segments[^1];
            var publicId = System.IO.Path.GetFileNameWithoutExtension(fileName);
            return publicId;
        }
    }
}
