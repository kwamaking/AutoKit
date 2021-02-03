using System.Collections.Generic;
using Oxide.Ext.AutoKit.Messages;

namespace Oxide.Ext.AutoKit.Settings
{
    public sealed class AutoKitConfiguration<T>
    {
        public Dictionary<ulong, long> playerCoolDowns { get; private set; } = new Dictionary<ulong, long>();
        public IAutoKitMessages messages { get; private set; }
        public AutoKitSettings settings { get; private set; }

        public AutoKitConfiguration( IAutoKitMessages messages, AutoKitSettings settings )
        {
            this.messages = messages;
            this.settings = settings;
        }
    }
}