using System.Collections.Generic;

namespace Oxide.Ext.AutoKit.Models
{
    public interface IKit<T>
    {
        string name { get; set; }
        List<T> items { get; set; }
    }
}