using Microsoft.AspNetCore.Mvc;
using Spents.EventSourcing.Domain.Interfaces;

namespace Spents.EventSourcing.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class ReceiptEventsController : Controller
    {
        private readonly IReceiptEvents receiptCreatedEventRepository;
        public ReceiptEventsController(IReceiptEvents receiptCreatedEventRepository) => this.receiptCreatedEventRepository = receiptCreatedEventRepository;

        [HttpGet]
        [Route("/getAllReceiptEvents", Name = nameof(ReceiptEventsController.GetAllEvents))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllEvents([FromQuery] Guid receiptId) => Ok(await receiptCreatedEventRepository.GetAllEvents(receiptId));
        
    }
}
