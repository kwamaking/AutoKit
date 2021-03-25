namespace Oxide.Ext.AutoKit.Settings
{
    public sealed class AutoKitSettings
    {
        public const int DefaultCoolDown = 5;
        public const int DefaultKitLimit = 10;
        public const string DefaultChatPrefix = "[<color=yellow>{0}</color>]";
        public const ulong DefaultIconId = 76561199152661014;

        public int coolDown { get; private set; }
        public int kitLimit { get; private set; }
        public string chatPrefix { get; private set; }
        public string pluginName { get; private set; }
        public ulong iconId { get; private set; }

        public AutoKitSettings( string pluginName, int? coolDown = null, int? kitLimit = null, string chatPrefix = null, ulong? iconId = null )
        {
            this.pluginName = pluginName;
            this.coolDown = coolDown ?? DefaultCoolDown;
            this.kitLimit = kitLimit ?? DefaultKitLimit;
            this.chatPrefix = chatPrefix ?? DefaultChatPrefix;
            this.iconId = iconId ?? DefaultIconId;
        }
    }
}