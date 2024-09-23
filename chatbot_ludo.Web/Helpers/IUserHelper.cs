namespace chatbot_ludo.Web.Helpers
{
    using chatbot_ludo.Web.Data.Entities;
    using Microsoft.AspNetCore.Identity;
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

    }
}
