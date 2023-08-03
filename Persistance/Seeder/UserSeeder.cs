﻿using Microsoft.AspNetCore.Identity;
using Models.Entities.Identity;

namespace Persistance.Seeder;

public static class UserSeeder
{
    public static async Task SeedAsync(UserManager<User> _userManager)
    {
        var usersCount = await _userManager.Users.CountAsync();
        if (usersCount <= 0)
        {
            var defaultuser = new User()
            {
                UserName = "admin@mail.com",
                Email = "admin@mail.com",
                Name = "organizationProject",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
            };
            await _userManager.CreateAsync(defaultuser, "123456");
            await _userManager.AddToRoleAsync(defaultuser, "SuperAdmin");
        }
    }
}
