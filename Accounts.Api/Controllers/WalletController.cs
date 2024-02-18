using Accounts.Domain.Wallets.Dtos;
using Accounts.Domain.Wallets.Models;
using Accounts.Domain.Wallets.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWallet.Common.Models;

namespace Accounts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("{customerId}/{accountId}/{id}")]
        public async Task<IActionResult> GetAsync(string customerId, int accountId, int id)
        {
            var wallet = await _walletService.GetAsync(customerId, accountId, id);
            return Ok(wallet);
        }

        [HttpGet("{customerId}/{accountId}")]
        public async Task<IActionResult> GetAllAsync(string customerId, int accountId)
        {
            var wallets = await _walletService.GetAllAsync(customerId, accountId);
            return Ok(wallets);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(WalletDto wallet)
        {
            wallet = await _walletService.AddAsync(wallet);
            return StatusCode(StatusCodes.Status201Created, wallet);
        }

        [HttpPost("Debit/{customerId}/{accountId}/{walletId}")]
        public async Task<IActionResult> DebitAsync(string customerId, int accountId, int walletId, [FromBody] Money money)
        {
            await _walletService.DebitAsync(customerId, accountId, walletId, money);
            return Ok();
        }

        [HttpPost("Credit/{customerId}/{accountId}/{walletId}")]
        public async Task<IActionResult> CreditAsync(string customerId, int accountId, int walletId, [FromBody] Money money)
        {
            await _walletService.CreditAsync(customerId, accountId, walletId, money);
            return Ok();
        }

        [HttpPost("Transfer")]
        public async Task<IActionResult> TransferAsync([FromBody] TransferRequest request)
        {
            await _walletService.TransferAsync(request);
            return Ok();
        }

        [HttpPatch("Status/{customerId}/{accountId}/{walletId}")]
        public async Task<IActionResult> UpdateStatusAsync(string customerId, int accountId, int walletId)
        {
            await _walletService.UpdateActive(customerId, accountId, walletId);
            return Ok();
        }

        [HttpDelete("{customerId}/{accountId}/{walletId}")]
        public async Task<IActionResult> DeleteAsync(string customerId, int accountId, int walletId)
        {
            await _walletService.DeleteAsync(customerId, accountId, walletId);
            return Ok();
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> ReverseAsyc(int transactionId)
        {
            await _walletService.ReverseAsync(transactionId);
            return Ok();
        }
    }
}
