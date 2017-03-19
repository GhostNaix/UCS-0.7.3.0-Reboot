using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers.Binary;
using UCS.Logic;

namespace UCS.Packets.Commands.Client
{
    // Packet 505
    internal class CancelConstructionCommand : Command
    {
        public CancelConstructionCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                if (go.ClassId == 0 || go.ClassId == 4)
                {
                    var constructionItem = (ConstructionItem) go;
                    if (constructionItem.IsConstructing())
                    {
                        var ca = this.Device.Player.Avatar;
                        string name = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId).GetData().GetName();
                        Logger.Write("Canceling Building Upgrade: " + name + " (" + BuildingId + ')');
                        if (string.Equals(name, "Alliance Castle"))
                        {
                            ca.Castle_Level--;
                            Building a = (Building)go;
                            BuildingData al = a.GetBuildingData();
                            ca.Castle_Total = al.GetUnitStorageCapacity(ca.Castle_Level- 1);
                        }
                        else if (string.Equals(name, "Town Hall"))
                            ca.TownHall_Level--;

                        constructionItem.CancelConstruction();
                    }
                }
                else if (go.ClassId == 3)
                {
                }
            }
        }

        public int BuildingId;
        public uint Unknown1;
    }
}
