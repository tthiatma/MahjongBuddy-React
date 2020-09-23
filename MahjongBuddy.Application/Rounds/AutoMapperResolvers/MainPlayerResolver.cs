﻿using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.AutoMapperResolvers
{
    public class MainPlayerResolver : IValueResolver<Round, RoundDto, RoundOtherPlayerDto>
    {
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public MainPlayerResolver(IMapper mapper, IUserAccessor userAccessor)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public RoundOtherPlayerDto Resolve(Round source, RoundDto destination, RoundOtherPlayerDto destMember, ResolutionContext context)
        {
            var roundId = source.Id;
            var mainPlayer = source.RoundPlayers.First(rp => rp.AppUser.UserName == _userAccessor.GetCurrentUserName());
            return _mapper.Map<RoundPlayer, RoundOtherPlayerDto>(mainPlayer);
        }
    }
}
