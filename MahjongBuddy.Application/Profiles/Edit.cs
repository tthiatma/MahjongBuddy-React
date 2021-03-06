﻿using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.EntityFramework.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MahjongBuddy.Application.Profiles
{
    public class Edit
    {
        public class Command : IRequest
        {
            public string DisplayName { get; set; }
            public string Bio { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly MahjongBuddyDbContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(MahjongBuddyDbContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());

                user.DisplayName = request.DisplayName ?? user.DisplayName;
                user.Bio = request.Bio ?? user.Bio;

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
