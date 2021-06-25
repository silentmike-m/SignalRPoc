using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    using System.Threading.Tasks;
    using MediatR;
    using Server.Users.Queries;

    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator) => this.mediator = mediator;

        [HttpPost(Name = "Login")]
        public async Task<string> Login([FromBody] GetToken request) => await this.mediator.Send(request);

        [HttpPost(Name = "GetUsers")]
        public async Task GetUsers() => await this.mediator.Send(new GetUsers());
    }
}