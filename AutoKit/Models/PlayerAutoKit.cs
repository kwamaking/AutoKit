using System.Collections.Generic;

namespace Oxide.Ext.AutoKit.Models
{
    public class PlayerAutoKit<T>
    {
        public ulong id { get; set; }
        public List<Kit<T>> kits { get; set; } = new List<Kit<T>>();
    }
}