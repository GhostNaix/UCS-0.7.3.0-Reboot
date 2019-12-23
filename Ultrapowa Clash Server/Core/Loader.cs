namespace UCS.Core
{
    using UCS.Core.Checker;
    using UCS.Database;
    using UCS.Core.Events;
    using UCS.Core.Settings;
    using UCS.Core.Threading;
    using UCS.Helpers;
    using UCS.Packets;
    using UCS.WebAPI;
    internal class Loader
    {
        internal CSVManager CsvManager;
        internal ConnectionBlocker ConnectionBlocker;
        internal DirectoryChecker DirectoryChecker;
        internal API API;
        internal Redis Redis;
        internal Logger Logger;
        internal ParserThread Parser;
        internal ResourcesManager ResourcesManager;
        internal ObjectManager ObjectManager;
        internal CommandFactory CommandFactory;
        internal MessageFactory MessageFactory;
        internal MemoryThread MemThread;
        internal LicenseChecker LicenseChecker;
        internal EventsHandler Events;

        public Loader()
        {
            // CSV Files and Logger
            this.Logger = new Logger();
            this.DirectoryChecker = new DirectoryChecker();
            this.CsvManager = new CSVManager();

            this.ConnectionBlocker = new ConnectionBlocker();
            if (Utils.ParseConfigBoolean("UseWebAPI"))
                this.API = new API();


            // Core
            this.LicenseChecker = new LicenseChecker();
            this.ResourcesManager = new ResourcesManager();
            this.ObjectManager = new ObjectManager();
            this.Events = new EventsHandler();
            if (Constants.UseCacheServer)
                this.Redis = new Redis();


            this.CommandFactory = new CommandFactory();

            this.MessageFactory = new MessageFactory();

            // Optimazions
            this.MemThread = new MemoryThread();

            // User
            this.Parser = new ParserThread();

        }
    }
}
