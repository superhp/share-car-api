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

        public UserController(IUserLogic userLogic )
        {
            _userLogic = userLogic;
        }

        public async Task<IActionResult> Get()
        {
            var userDto = await _userLogic.GetLoggedInUser();
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
            var userDto = await _userLogic.GetLoggedInUser();

            var drivers = _userLogic.GetDrivers(userDto.Email);

            return Ok(drivers);
        }

        [HttpGet("getPoints")]
        public async Task<IActionResult> GetPointsAsync()
        {
            var userDto = await _userLogic.GetLoggedInUser();

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
        [HttpPost("updateHomeAddress")]
        public async Task<IActionResult> UpdateHomeAddress([FromBody] AddressDto address)
        {
            if (address == null)
            {
                return BadRequest();
            }
            var userDto = await _userLogic.GetLoggedInUser();

            _userLogic.UpdateHomeAddress(address, userDto.Email);
            return Ok();
        }
        [HttpGet("homeAddress")]
        public async Task<IActionResult> GetHomeAddress()
        {
            var userDto = await _userLogic.GetLoggedInUser();

            return Ok(userDto.HomeAddress);
        }
    }
}