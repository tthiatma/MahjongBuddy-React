using AutoMapper;
using MahjongBuddy.Application.Dtos;
using MahjongBuddy.Application.Enums;
using MahjongBuddy.Application.Interfaces;
using MahjongBuddy.Core;
using MahjongBuddy.EntityFramework.EntityFramework;
using System;
using System.Linq;

namespace MahjongBuddy.Application.Rounds.AutoMapperResolvers
{
    public class SeatOrientationResolver : IValueResolver<RoundPlayer, RoundOtherPlayerDto, SeatOrientation>
    {
        private readonly MahjongBuddyDbContext _context;
        private readonly IUserAccessor _userAccessor;

        public SeatOrientationResolver(MahjongBuddyDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public SeatOrientation Resolve(RoundPlayer source, RoundOtherPlayerDto destination, SeatOrientation destMember, ResolutionContext context)
        {
            WindDirection userWind = WindDirection.East;
            if (context.Options.Items.Count() > 0)
            {
                if (context.Options.Items.ContainsKey("MainRoundPlayer"))
                {
                    var rp = context.Items["MainRoundPlayer"] as RoundPlayer;
                    userWind = rp.Wind;
                }
            }

            switch (userWind)
            {
                case WindDirection.East:
                    switch (source.Wind)
                    {
                        case WindDirection.East:
                            return SeatOrientation.None;
                        case WindDirection.South:
                            return SeatOrientation.Right;
                        case WindDirection.West:
                            return SeatOrientation.Top;
                        case WindDirection.North:
                            return SeatOrientation.Left;
                        default:
                            return SeatOrientation.None;
                    }
                case WindDirection.South:
                    switch (source.Wind)
                    {
                        case WindDirection.East:
                            return SeatOrientation.Left;
                        case WindDirection.South:
                            return SeatOrientation.None;
                        case WindDirection.West:
                            return SeatOrientation.Right;
                        case WindDirection.North:
                            return SeatOrientation.Top;
                        default:
                            return SeatOrientation.None;
                    }
                case WindDirection.West:
                    switch (source.Wind)
                    {
                        case WindDirection.East:
                            return SeatOrientation.Top;
                        case WindDirection.South:
                            return SeatOrientation.Left;
                        case WindDirection.West:
                            return SeatOrientation.None;
                        case WindDirection.North:
                            return SeatOrientation.Right;
                        default:
                            return SeatOrientation.None;
                    }
                case WindDirection.North:
                    switch (source.Wind)
                    {
                        case WindDirection.East:
                            return SeatOrientation.Right;
                        case WindDirection.South:
                            return SeatOrientation.Top;
                        case WindDirection.West:
                            return SeatOrientation.Left;
                        case WindDirection.North:
                            return SeatOrientation.None;
                        default:
                            return SeatOrientation.None;
                    }
                default:
                    return SeatOrientation.None;
            }
        }
    }
}
