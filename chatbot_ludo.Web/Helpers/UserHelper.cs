namespace chatbot_ludo.Web.Helpers
{
    using System.Threading.Tasks;
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;

    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> userManager;

        public UserHelper(UserManager<User> userManager) //Inyecta el user manager. No se programa en el program, viene por el core.
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await this.userManager.CreateAsync(user, password); //Agregamos usuario. Creamos con user y password. Esta es una interface, toca mapearla en program,
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await this.userManager.FindByEmailAsync(email); //Devuelme el email de manera async.
        }
    }

}
