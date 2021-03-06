﻿using MahjongBuddy.EntityFramework.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace MahjongBuddy.Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MahjongBuddyDbContext _context;

        public IsHostRequirementHandler(IHttpContextAccessor httpContextAccessor, MahjongBuddyDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var currentUserName = _httpContextAccessor.HttpContext.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var gameIdString = _httpContextAccessor.HttpContext.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value;
            var gameId = Int32.Parse(gameIdString.ToString());

            var game = _context.Games.FindAsync(gameId).Result;

            var host = game.GamePlayers.FirstOrDefault(x => x.IsHost);

            if (host?.Player?.UserName == currentUserName)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
