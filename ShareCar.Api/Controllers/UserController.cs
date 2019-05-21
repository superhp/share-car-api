using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Dto;
using ShareCar.Dto.Identity;
using ShareCar.Logic.User_Logic;

namespace ShareCar.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserLogic _userLogic;
        private readonly IUserRepository _userRepository;

        public UserController(IUserLogic userLogic, IUserRepository userRepository)
        {
            _userLogic = userLogic;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Get()
        {
            var userDto = await _userLogic.GetUserAsync(User);
            int points = _userLogic.CountPoints(userDto.Email);
            return Ok(new
            {
                user = userDto,
                pointCount = points
            });
        }
        [HttpGet("WinnerBoard")]
        public IActionResult GetWinnerBoard()
        {
            var tupleList = _userLogic.GetWinnerBoard();
            Dictionary<UserDto, int> users = new Dictionary<UserDto, int>();

            foreach(var item in tupleList)
            {
                users.Add(item.Item1, item.Item2);
            }

            return Ok(new
            {
                users = users.Keys,
                points = users.Values
            });
        }

        [HttpGet("getDrivers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var userDto = await _userRepository.GetLoggedInUser(User);

            var drivers = _userLogic.GetDrivers(userDto.Email);

            return Ok(drivers);
        }

        [HttpGet("getPoints")]
        public async Task<IActionResult> GetPointsAsync()
        {
            var userDto = await _userRepository.GetLoggedInUser(User);

            return Ok(_userLogic.GetPoints(userDto.Email));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            await _userLogic.UpdateUserAsync(user);
            return Ok();
        }

    }
}