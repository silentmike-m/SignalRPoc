namespace Server.Users.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Server.Commons;
    using Server.Entities;
    using Server.Users.Queries;

    internal sealed class GetTokenHandler : IRequestHandler<GetToken, string>
    {
        private readonly ApiContext context;
        private readonly JwtConfiguration jwtConfiguration;
        private readonly ILogger<GetTokenHandler> logger;

        public GetTokenHandler(IConfiguration configuration, ApiContext context, ILogger<GetTokenHandler> logger)
        {
            this.context = context;
            this.jwtConfiguration = configuration.GetSection("Jwt").Get<JwtConfiguration>();
            this.logger = logger;
        }

        public Task<string> Handle(GetToken request, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Try to log user {UserName}", request.UserName);

            var user = this.context.Users.SingleOrDefault(i => i.UserName == request.UserName);

            if (user is null)
            {
                throw new Exception("User not found");
            }

            if (!user.Password.Equals(request.Password))
            {
                throw new Exception("Invalid user password");
            }

            var token = this.CreateToken(user, request.GroupId);

            return Task.FromResult(token);
        }

        private string CreateToken(User user, string groupId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new(JwtRegisteredClaimNames.GivenName, user.UserName),
                new("GroupId", groupId),
            };

            var dateTime = DateTime.Now;

            var token = new JwtSecurityToken
            (
                audience: jwtConfiguration.Audience,
                claims: claims,
                issuer: jwtConfiguration.Issuer,
                notBefore: dateTime,
                expires: dateTime.AddYears(1),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}