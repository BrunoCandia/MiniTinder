using API.DTO;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedDataAsync(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var members = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData);

            if (members is null)
            {
                Console.WriteLine("No members found in the seed data.");
                return;
            }            

            foreach (var member in members)
            {
                using var hmac = new HMACSHA512();

                var user = new User
                {
                    Email = member.Email,               
                    DisplayName = member.DisplayName,
                    ImageUrl = member.ImageUrl,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
                    PasswordSalt = hmac.Key,
                    Member = new Member
                    {                        
                        DisplayName = member.DisplayName,
                        Description = member.Description,
                        DateOfBirth = member.DateOfBirth,
                        ImageUrl = member.ImageUrl,
                        Gender = member.Gender,
                        City = member.City,
                        Country = member.Country,
                        LastActive = member.LastActive,
                        Created = member.Created
                    }
                };                

                await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();

            Console.WriteLine("Users added successfully.");

            var users = await context.Users.ToListAsync();

            foreach (var user in users)
            {
                user.Member.Photos.Add(new Photo
                {
                    MemberId = user.Member.Id,
                    Url = user.Member.ImageUrl!                    
                });

                context.Users.Update(user);
            }

            await context.SaveChangesAsync();

            Console.WriteLine("Users updated successfully.");
        }
    }
}
