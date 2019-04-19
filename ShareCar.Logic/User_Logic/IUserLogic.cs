using ShareCar.Dto;
using ShareCar.Dto.Identity;
using ShareCar.Dto.Identity.Cognizant;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareCar.Logic.User_Logic
{
    public interface IUserLogic
    {
        Task<UserDto> GetUserAsync(ClaimsPrincipal principal);
        Task UpdateUserAsync(UserDto updatedUser, ClaimsPrincipal User);
        int CountPoints(string email);
        List<Tuple<UserDto, int>> GetWinnerBoard();
        IEnumerable<UserDto> GetAllUsers();
        UnauthorizedUserDto GetUnauthorizedUser(string email);
        Task CreateUser(UserDto userDto);
        void CreateUnauthorizedUser(UnauthorizedUserDto user);
        void SetUsersCognizantEmail(CognizantData data);
        void VerifyUser(bool faceBookVerified, string loginEmail); 
        UserDto GetUserByEmail(EmailType type, string email);
        bool DoesUserExist(EmailType type, string cognizantEmail);
        int GetPoints(string userEmail);
        List<UserDto> GetDrivers(string email);

    }
}
