using Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DBService.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDBContext _context;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthRepository(AppDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Checks if the email address belongs to another user
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<bool> IsEmailAvailable(string emailAddress)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == emailAddress.ToLower());
            return user == null;
            // return true;
        }

        public async Task<bool> CheckPasswordAndLockOn5FailedAttempts(AppUser User, string Password)
        {
            if (!await checkPassword(User, Password))
            {
                // Increase failed attemptes
                await _userManager.AccessFailedAsync(User);

                // Throw invalid login agian
                return false;
            }

            return true;
        }

        public async Task<bool> checkPassword(AppUser user, string Password)
        {
            return await _userManager.CheckPasswordAsync(user, Password);
        }

        public async Task<bool> changePassword(AppUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            return result.Succeeded;
        }

        public async Task<bool> resetpassword(AppUser user, string newPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task RemoveFromAllRoles(AppUser user)
        {
            var roles = user.UserRoles.Select(x => x.Role.Name);
            await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task AddToRoles(AppUser user, IEnumerable<string> roles)
        {
            await _userManager.AddToRolesAsync(user, roles);
        }

    }
}
