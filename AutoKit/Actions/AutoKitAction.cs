using System;
using System.Collections.Generic;
using Oxide.Ext.AutoKit.Models;
using Oxide.Ext.AutoKit.Settings;

namespace Oxide.Ext.AutoKit.Actions
{
    public sealed class AutoKitAction<T> : IAutoKitAction<T>, IAutoKitDestructive<T>, IAutoKitNonDestructive<T>, IAutoKitNotify<T>, IDisposable
    {
        private AutoKitConfiguration<T> configuration { get; set; }
        private List<PlayerAutoKit<T>> playerAutoKits { get; set; }
        private List<string> notifications { get; set; }
        private BasePlayer player { get; set; }
        private Kit<T> kitForDestructiveAction { get; set; }

        public AutoKitAction( AutoKitConfiguration<T> configuration, BasePlayer player, List<PlayerAutoKit<T>> playerAutoKits )
        {
            this.configuration = configuration;
            this.player = player;
            this.playerAutoKits = playerAutoKits;
            this.notifications = new List<string>();
        }

        public IAutoKitDestructive<T> WithKit( string kitName )
        {
            var kit = FindKit( kitName );
            this.kitForDestructiveAction = kit;

            if ( null == kit )
                WithNotification( configuration.messages.noKit, kitName );

            return this;
        }

        public IAutoKitDestructive<T> WithNewKit( string kitName )
        {
            if ( null != FindKit( kitName ) )
                WithNotification( configuration.messages.kitExists, kitName );

            this.kitForDestructiveAction = new Kit<T> { name = kitName };

            return this;
        }

        public IAutoKitNotify<T> Apply( Action<BasePlayer, Kit<T>> callBack )
        {
            return WithCoolDown().MaybeApply( callBack );
        }

        public IAutoKitNotify<T> Save( Func<BasePlayer, Kit<T>, Kit<T>> callBack )
        {
            return WithCoolDown().MaybeSave( callBack );
        }

        public IAutoKitNotify<T> Remove()
        {
            return WithCoolDown().MaybeRemove(); ;
        }

        public IAutoKitAction<T> List( Action<Kit<T>[]> callBack )
        {
            callBack( FindPlayerAutoKit().kits.ToArray() );
            return this;
        }

        public IAutoKitNotify<T> ListToNotify()
        {
            FindPlayerAutoKit().kits.ForEach( kit => WithNotification( configuration.messages.list, kit.name ) );

            return ToNotify();
        }

        public IAutoKitAction<T> WithNotification( string message, params object[] args )
        {
            notifications.Add( $"{string.Format( configuration.settings.chatPrefix, configuration.settings.pluginName )} {string.Format( message, args )}" );

            return this;
        }

        public IAutoKitAction<T> Notify()
        {
            notifications.ForEach( player.ChatMessage );

            return this;
        }

        public IAutoKitNotify<T> MaybeApply( Action<BasePlayer, Kit<T>> callBack )
        {
            if ( notifications.Count == 0 )
            {
                callBack( player, kitForDestructiveAction );
                WithNotification( configuration.messages.applied, kitForDestructiveAction.name );
            }

            return this;
        }

        public IAutoKitNotify<T> MaybeRemove()
        {
            if ( notifications.Count == 0 )
            {
                FindPlayerAutoKit().kits.Remove( kitForDestructiveAction );
                WithNotification( configuration.messages.removed, kitForDestructiveAction.name );
            }

            return this;
        }

        public IAutoKitNotify<T> MaybeSave( Func<BasePlayer, Kit<T>, Kit<T>> callBack )
        {
            if ( FindPlayerAutoKit().kits.Count >= configuration.settings.kitLimit )
                WithNotification( configuration.messages.kitLimitReached, configuration.settings.kitLimit );

            if ( notifications.Count == 0 )
            {
                FindPlayerAutoKit().kits.Add( callBack( player, kitForDestructiveAction ) );
                WithNotification( configuration.messages.saved, kitForDestructiveAction.name );
            }

            return this;
        }

        public IAutoKitNotify<T> ToNotify()
        {
            return this;
        }

        public IAutoKitNonDestructive<T> ToNonDestructive()
        {
            return this;
        }

        public IAutoKitDestructive<T> WithCoolDown()
        {
            var userId = player.userID;
            var commandTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long lastCommandTime;

            if ( configuration.playerCoolDowns.TryGetValue( userId, out lastCommandTime ) && commandTime - lastCommandTime < configuration.settings.coolDown )
                WithNotification( configuration.messages.coolDown, lastCommandTime + configuration.settings.coolDown - commandTime );

            configuration.playerCoolDowns[userId] = commandTime;

            return this;
        }

        private Kit<T> FindKit( string kitName )
        {
            return FindPlayerAutoKit().kits.Find( s => s.name == kitName );
        }

        private PlayerAutoKit<T> FindPlayerAutoKit()
        {
            var playerAutoKit = playerAutoKits.Find( p => p.id == player.userID );

            if ( null == playerAutoKit )
            {
                playerAutoKit = new PlayerAutoKit<T> { id = player.userID };
                playerAutoKits.Add( playerAutoKit );
            }

            return playerAutoKit;
        }

        public void Dispose()
        {
            GC.SuppressFinalize( this );
        }
    }
}