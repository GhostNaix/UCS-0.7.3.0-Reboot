﻿using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers.Binary;
using UCS.Logic;
using UCS.Logic.StreamEntry;
using UCS.Packets.Commands.Server;
using UCS.Packets.Messages.Server;

namespace UCS.Packets.Messages.Client
{
    // Packet 14306
    internal class PromoteAllianceMemberMessage : Message
    {
        public PromoteAllianceMemberMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        public long m_vId;
        public int m_vRole;

        internal override void Decode()
        {
            this.m_vId   = this.Reader.ReadInt64();
            this.m_vRole = this.Reader.ReadInt32();
        }

        internal override async void Process()
        {
            try
            {
                Level target = await ResourcesManager.GetPlayer(m_vId);
                ClientAvatar player = this.Device.Player.Avatar;
                Alliance alliance = await ObjectManager.GetAlliance(player.AllianceID);
                if (await player.GetAllianceRole() == 2 || await player.GetAllianceRole() == 4)
                    if (player.AllianceID == target.Avatar.AllianceID)
                    {
                        int oldrole = await target.Avatar.GetAllianceRole();
                        target.Avatar.SetAllianceRole(m_vRole);
                        if (m_vRole == 2)
                        {
                            player.SetAllianceRole(4);

                            AllianceEventStreamEntry demote = new AllianceEventStreamEntry();
                            demote.SetId(alliance.GetChatMessages().Count + 1);
                            demote.SetSender(player);
                            demote.SetEventType(6);
                            demote.SetAvatarId(player.UserID);
                            demote.SetUsername(player.Username);

                            alliance.AddChatMessage(demote);

                            AllianceEventStreamEntry promote = new AllianceEventStreamEntry();
                            promote.SetId(alliance.GetChatMessages().Count + 1);
                            promote.SetSender(target.Avatar);
                            promote.SetEventType(5);
                            promote.SetAvatarId(player.UserID);
                            promote.SetUsername(player.Username);

                            alliance.AddChatMessage(promote);

                            AllianceRoleUpdateCommand p = new AllianceRoleUpdateCommand(this.Device);
                            AvailableServerCommandMessage pa = new AvailableServerCommandMessage(Device, p.Handle());

                            AllianceRoleUpdateCommand t = new AllianceRoleUpdateCommand(target.Client);
                            AvailableServerCommandMessage ta = new AvailableServerCommandMessage(target.Client, t.Handle());

                            PromoteAllianceMemberOkMessage rup = new PromoteAllianceMemberOkMessage(Device)
                            {
                                Id = this.Device.Player.Avatar.UserID,
                                Role = 4
                            };
                            PromoteAllianceMemberOkMessage rub = new PromoteAllianceMemberOkMessage(target.Client)
                            {
                                Id = target.Avatar.UserID,
                                Role = 2
                            };

                            t.SetAlliance(alliance);
                            p.SetAlliance(alliance);
                            t.SetRole(2);
                            p.SetRole(4);
                            t.Tick(target);
                            p.Tick(this.Device.Player);

                            if (ResourcesManager.IsPlayerOnline(target))
                            {
                                ta.Send();
                                rub.Send();
                            }
                            rup.Send();
                            pa.Send();

                            foreach (AllianceMemberEntry op in alliance.GetAllianceMembers())
                            {
                                Level aplayer = await ResourcesManager.GetPlayer(op.GetAvatarId());
                                if (aplayer.Client != null)
                                {
                                    AllianceStreamEntryMessage a = new AllianceStreamEntryMessage(aplayer.Client);
                                    AllianceStreamEntryMessage b = new AllianceStreamEntryMessage(aplayer.Client);

                                    a.SetStreamEntry(demote);
                                    b.SetStreamEntry(promote);

                                    a.Send();
                                    b.Send();
                                }

                            }
                        }
                        else
                        {
                            AllianceRoleUpdateCommand t = new AllianceRoleUpdateCommand(target.Client);
                            AvailableServerCommandMessage ta = new AvailableServerCommandMessage(target.Client, t.Handle());
                            AllianceEventStreamEntry stream = new AllianceEventStreamEntry();

                            stream.SetId(alliance.GetChatMessages().Count + 1);
                            stream.SetSender(target.Avatar);
                            stream.SetAvatarId(player.UserID);
                            stream.SetUsername(player.Username);
                            stream.SetEventType(m_vRole > oldrole ? 5 : 6);

                            t.SetAlliance(alliance);
                            t.SetRole(m_vRole);
                            t.Tick(target);

                            PromoteAllianceMemberOkMessage ru = new PromoteAllianceMemberOkMessage(target.Client)
                            {
                                Id = target.Avatar.UserID,
                                Role =  m_vRole
                            };

                            alliance.AddChatMessage(stream);

                            if (ResourcesManager.IsPlayerOnline(target))
                            {
                                ta.Send();
                                ru.Send();
                            }

                            foreach (AllianceMemberEntry op in alliance.GetAllianceMembers())
                            {
                                Level aplayer = await ResourcesManager.GetPlayer(op.GetAvatarId());
                                if (aplayer.Client != null)
                                {
                                    AllianceStreamEntryMessage b = new AllianceStreamEntryMessage(aplayer.Client);
                                    b.SetStreamEntry(stream);
                                    b.Send();
                                }
                            }
                        }
                    }
            } catch (Exception) { }
        }
    }
}
