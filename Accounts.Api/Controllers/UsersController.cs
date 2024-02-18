using Accounts.Application.Services;
using Microsoft.AspNetCore.Mvc;
using MyWallet.Common.Models;

namespace Accounts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Customer")]
        public async Task<IActionResult> PostAsync(CustomerDto customer)
        {
            await _userService.AddAsync(customer);
            return Ok();
        }
    }
}
