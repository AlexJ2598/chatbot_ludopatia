namespace chatbot_ludo.Web.Helpers
{
    using Microsoft.AspNetCore.Identity;
    using Data.Entities;
    using Models;
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        //Agregamos los metodos async para login o logout.
        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<bool> CheckPasswordAsync(User user, string password);

        //Metodos para modificar y actualizar contraseña.
        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);



    }
}
