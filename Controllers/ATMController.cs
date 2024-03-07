using ATM.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ATM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ATMController : ControllerBase
    {
        private readonly ATMLogic _atmlogic;
        public ATMController(ATMLogic atmlogic)
        {
            _atmlogic = atmlogic;
        }

        [HttpPost("init")]
        public ActionResult InitATM(string number)
        {
            return Ok(_atmlogic.InitATM(number));
        }

        [HttpPost("authorize")]
        public ActionResult Authorize(string pin)
        {
            return Ok(_atmlogic.Authorize(pin));
        }

        [HttpPost("withdraw")]
        public ActionResult Withdraw(decimal amount)
        {
            return Ok(_atmlogic.Withdraw(amount));
        }

        [HttpGet("balance")]
        public ActionResult<decimal> GetBalance()
        {
            return Ok(_atmlogic.GetBalance());
        }

        [HttpPost("transfer")]
        public ActionResult Transfer([FromBody] TransferRequest transferRequest)
        {
            return Ok(_atmlogic.Transfer(transferRequest));
        }
    }
}