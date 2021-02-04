using System;
using Oxide.Ext.AutoKit.Models;

namespace Oxide.Ext.AutoKit.Actions
{
    public interface IAutoKitNonDestructive<T>
    {
        IAutoKitAction<T> List( Action<Kit<T>[]> callBack );
        IAutoKitNotify<T> ListToNotify();
        IAutoKitAction<T> WithNotification( string message, params object[] args );
        IAutoKitDestructive<T> WithCoolDown();
    }
}