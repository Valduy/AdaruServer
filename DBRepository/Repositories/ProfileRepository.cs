﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql;
using Task = System.Threading.Tasks.Task;

namespace DBRepository.Repositories
{
    public class ProfileRepository : BaseRepository, IProfileRepository
    {
        public ProfileRepository(string connectionString, IRepositoryContextFactory contextFactory) 
            : base(connectionString, contextFactory)
        {
        }

        public async Task<Profile> GetProfile(int profileId)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            return context.Profiles.FirstOrDefault(p => p.IdClient == profileId);
        }

        public async Task AddProfile(Profile profile)
        {
            try
            {
                await using var context = ContextFactory.CreateDbContext(ConnectionString);
                await context.AddAsync(profile);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException innerException)
                {
                    switch (innerException.SqlState)
                    {
                        case PgsqlErrors.UniqueViolation:
                            throw new RepositoryException("У пользователя уже есть профиль.");
                    }
                }

                throw;
            }
        }

        public async Task AddImage(Profile profile, Image image)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.ProfileImages.AddAsync(new ProfileImage()
            {
                IdProfile = profile.IdClient,
                IdImage = image.Id
            });
            await context.SaveChangesAsync();
        }

        public async Task AddImages(Profile profile, IEnumerable<Image> images)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);

            foreach (var image in images)
            {
                await context.ProfileImages.AddAsync(new ProfileImage()
                {
                    IdProfile = profile.IdClient,
                    IdImage = image.Id
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task RemoveImage(Profile profile, Image image)
        {
            await using var context = ContextFactory.CreateDbContext(ConnectionString);
            var profileImage = await context.ProfileImages.FirstOrDefaultAsync(
                pi => pi.IdProfile == profile.IdClient && pi.IdImage == image.Id);

            if (profileImage != null)
            {
                context.ProfileImages.Remove(profileImage);
            }

            await context.SaveChangesAsync();
        }
    }
}
