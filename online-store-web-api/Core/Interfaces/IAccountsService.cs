using Core.Models;
using Core.Models.Account;
using Core.Models.User;

namespace Core.Interfaces
{
    public interface IAccountsService
    {
        Task<IEnumerable<GetUserDto>> GetAll();
        Task<GetUserDto> GetById(string id);
        Task<GetUserDto> GetByUserName(string userName);
        Task<LoginResponseDto> Login(LoginDto loginDto);
        Task<LoginResponseDto> GoogleLogin(LoginGoogleDto loginDto);
        Task Register(RegisterDto registerDto);
        Task Logout();
        Task ChangePassword(ChangePasswordDto changePasswordDto);
        Task Delete(string id);
        Task Update(string userId, UpdateUserDto updateUserDto);
        Task<PaginatedList<GetUserDto>> GetByFilter(ItemsFilter itemsFilter);
    }
}