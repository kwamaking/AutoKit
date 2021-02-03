using System.Collections.Generic;

namespace Oxide.Ext.AutoKit.Models
{
    public class Kit<T>
    {
        public string name { get; set; }
        public List<T> items { get; set; } = new List<T>();
    }
}