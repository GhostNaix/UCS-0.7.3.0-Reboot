using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UCS.Logic.Manager;
using UCS.Packets;

namespace UCS.Logic
{
    internal class Level
    {
        internal GameObjectManager GameObjectManager;
        internal WorkerManager WorkerManager;
        internal Device Client;
        internal ClientAvatar Avatar;


        public Level()
        {
            this.WorkerManager = new WorkerManager();
            this.GameObjectManager = new GameObjectManager(this);
            this.Avatar = new ClientAvatar();
        }

        public Level(long id, string token)
        {
            this.WorkerManager = new WorkerManager();
            this.GameObjectManager = new GameObjectManager(this);
            this.Avatar = new ClientAvatar(id);
        }

        public ComponentManager GetComponentManager() => GameObjectManager.GetComponentManager();

        public bool HasFreeWorkers() => WorkerManager.GetFreeWorkers() > 0;

        public void LoadFromJSON(string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);
            GameObjectManager.Load(jsonObject);
        }

        public string SaveToJSON() => JsonConvert.SerializeObject(GameObjectManager.Save(), Formatting.Indented);

        public void SetHome(string jsonHome) => GameObjectManager.Load(JObject.Parse(jsonHome));

        public void Tick()
        {
            this.Avatar.Update = DateTime.UtcNow;
            GameObjectManager.Tick();
        }
    }
}
