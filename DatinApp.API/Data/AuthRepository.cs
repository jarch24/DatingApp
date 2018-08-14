using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatinApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatinApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _db;
        public AuthRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<User> Login(string username, string password)
        {
            try
            {
                if(String.IsNullOrEmpty(password))
                    return null;
                
                
                var user = await _db.Users.FirstOrDefaultAsync(t => t.Username == username);
                if(user != null){

                    if(!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                        return null;
                    
                }
                return user;

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            try
            {
                using (var hmac = new HMACSHA512(passwordSalt))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if(computedHash[i] != passwordHash[i])
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public async Task<User> Register(User user, string password)
        {
           try
            {
                byte[] PasswordHash, PasswordSalt;
                CreatePasswordHash(password, out PasswordHash, out PasswordSalt);

                user.PasswordHash = PasswordHash;
                user.PasswordSalt = PasswordSalt;
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            try
            {
                using (var hmac = new HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public async Task<bool> UserExists(string username)
        {
            if(await _db.Users.AnyAsync(t => t.Username == username))
                return true;
            else
                return false;    
        }
    }
}