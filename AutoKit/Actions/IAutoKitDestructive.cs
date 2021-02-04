using System;
using Oxide.Ext.AutoKit.Models;
namespace Oxide.Ext.AutoKit.Actions
{
    public interface IAutoKitDestructive<T>
    {
        IAutoKitNotify<T> Apply( Action<BasePlayer, Kit<T>> callBack );
        IAutoKitNotify<T> Remove();
        IAutoKitNotify<T> Save( Func<BasePlayer, Kit<T>, Kit<T>> callBack );
        IAutoKitNotify<T> MaybeApply( Action<BasePlayer, Kit<T>> callBack );
        IAutoKitNotify<T> MaybeRemove();
        IAutoKitNotify<T> MaybeSave( Func<BasePlayer, Kit<T>, Kit<T>> callBack );
    }
}