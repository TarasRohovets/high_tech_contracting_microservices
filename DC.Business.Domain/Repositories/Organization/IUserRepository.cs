using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.ViewModel;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;

namespace DC.Business.Domain.Repositories.Organization
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAndPasswordsAsync(string username, List<string> hashedPasswords);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(long userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<long> CreateUserAsync(User newUser, string hashedPassword);
        Task UpdateUserAsync(User newUser);
        Task<IEnumerable<User>> SearchUsers(SearchPaginationRequestDto<UserSearchInput> inputDto);
        Task UpdateUserProfilePhoto(long id, string imageName, string imagePath);
        Task DeleteUserProfilePhoto(long userId);
    }
}
