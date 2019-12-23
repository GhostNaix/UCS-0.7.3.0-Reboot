using System;
using UCS.Core;
using UCS.Core.Network;
using UCS.Logic;
using UCS.Packets.Messages.Server;

namespace UCS.Packets
{
    internal class GameOpCommand
    {
        byte m_vRequiredAccountPrivileges;

        public static void SendCommandFailedMessage(Device c)
        {
            Console.WriteLine("GameOp command failed. Insufficient privileges. Requster ID -> " + c.Player.Avatar.UserId);
            var p = new GlobalChatLineMessage(c)
            {
                Message = "GameOp command failed. Insufficient privileges.",
                HomeId = 0,
                CurrentHomeId = 0,
                LeagueId = 22,
                PlayerName = "Ultrapower Clash Server AI"
            };
            p.Send();
        }

        public virtual void Execute(Level level)
        {
        }

        public byte GetRequiredAccountPrivileges() => m_vRequiredAccountPrivileges;

        public void SetRequiredAccountPrivileges(byte level)
        {
            m_vRequiredAccountPrivileges = level;
        }
    }
}