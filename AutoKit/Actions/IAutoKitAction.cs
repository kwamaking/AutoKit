using System;
using Oxide.Ext.AutoKit.Models;

namespace Oxide.Ext.AutoKit.Actions
{
    public interface IAutoKitActions<T>
    {
        void Apply( string kitName, Action<BasePlayer, Kit<T>> callBack );
        void Save( string kitName, Func<BasePlayer, string, Kit<T>> callBack );
        void HasCoolDown( Action<bool> callBack );
        void Remove( string kitName );
        void List();
        void Notify( string message, params object[] args );
    }
}