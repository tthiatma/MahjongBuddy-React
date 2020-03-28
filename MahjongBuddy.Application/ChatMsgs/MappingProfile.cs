using AutoMapper;
using MahjongBuddy.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MahjongBuddy.Application.ChatMsgs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChatMsg, ChatMsgDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName));
        }
    }
}
