
using Conta360.Application.DTOs;
using Conta360.Application.Features.PgcAccounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conta360.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PgcAccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PgcAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PgcAccountDto>>> Get()
        {
            var result = await _mediator.Send(new GetAllPgcAccountsQuery());
            return Ok(result);
        }
        [HttpGet("tree")]
        public async Task<ActionResult<List<PgcAccountTreeDto>>> GetTree()
        {
            var result = await _mediator.Send(new GetPgcAccountTreeQuery());
            return Ok(result);
        }
    }
}
