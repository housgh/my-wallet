using Accounts.Domain.Accounts.Dtos;
using Accounts.Domain.Accounts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{customerId}/{id}")]
        public async Task<IActionResult> GetAsync(string customerId, int id)
        {
            var account = await _accountService.GetAsync(customerId ,id);
            return Ok(account);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetAllAsync(string customerId)
        {
            var accounts = await _accountService.GetAllAsync(customerId);
            return Ok(accounts);
        }

        [HttpGet("MiniStatement/{customerId}")]
        public async Task<IActionResult> GetMiniStatement(string customerId, int? accountId, int? walletId)
        {
            var miniStatement = await _accountService.GetMiniStatementAsync(customerId, accountId, walletId);
            return Ok(miniStatement);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ExtendedAccountDto accountDto)
        {
            var account = await _accountService.AddAsync(accountDto);
            return StatusCode(StatusCodes.Status201Created, account);
        }

        [HttpPatch("Status/{customerId}/{id}")]
        public async Task<IActionResult> UpdateStatusAsync(string customerId, int id)
        {
            var account = await _accountService.UpdateStatusAsync(customerId, id);
            return Ok(account);
        }

        [HttpDelete("{customerId}/{id}")]
        public async Task<IActionResult> DeleteAsync(string customerId, int id)
        {
            await _accountService.RemoveAsync(customerId, id);
            return Ok();
        }
    }
}
