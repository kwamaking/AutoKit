using System;
using System.Collections.Generic;
using Oxide.Ext.AutoKit.Models;
using Oxide.Ext.AutoKit.Settings;

namespace Oxide.Ext.AutoKit.Actions
{
    public sealed class AutoKitAction<T> : IAutoKitActions<T>, IDisposable
    {
        private AutoKitConfiguration<T> configuration { get; set; }
        private List<PlayerAutoKit<T>> playerAutoKits { get; set; }
        private BasePlayer player { get; set; }

        public AutoKitAction( AutoKitConfiguration<T> configuration, BasePlayer player, List<PlayerAutoKit<T>> playerAutoKits )
        {
            this.configuration = configuration;
            this.player = player;
            this.playerAutoKits = playerAutoKits;
        }

        public void Apply( string kitName, Action<BasePlayer, Kit<T>> callBack )
        {
            if ( !CanProceed( kitName ) ) return;

            var kit = FindKit( kitName );

            if ( null == kit )
            {
                Notify( configuration.messages.noKit, kitName );
                return;
            }

            callBack( player, kit );

            Notify( configuration.messages.applied, kitName );
        }

        public void Save( string kitName, Func<BasePlayer, string, Kit<T>> callBack )
        {

            if ( !CanProceed( kitName ) ) return;

            Remove( kitName );

            if ( HasReachedKitLimit() ) return;

            FindPlayerAutoKit().kits.Add( callBack( player, kitName ) );

            Notify( configuration.messages.saved, kitName );
        }

        public void Remove( string kitName )
        {
            var kit = FindKit( kitName );

            if ( null != kit )
            {
                playerAutoKits.Find( p => p.id == player.userID )?.kits?.Remove( kit );
                Notify( configuration.messages.removed, kitName );
            }
        }

        public void List()
        {
            playerAutoKits.Find( p => p.id == player.userID )?.kits?.ForEach( k => Notify( configuration.messages.list, k.name ) );
        }

        public void HasCoolDown( Action<bool> callBack )
        {
            long coolDown = 0;
            if ( HasCoolDown( out coolDown ) )
            {
                callBack( true );
                Notify( configuration.messages.coolDown, coolDown );
            }
            callBack( false );
        }

        public void Notify( string message, params object[] args )
        {
            player.ChatMessage( $"{string.Format( configuration.settings.chatPrefix, configuration.settings.pluginName )} {string.Format( message, args )}" );
        }

        private bool CanProceed( string kitName )
        {
            long coolDown = 0;

            if ( !IsKitNameValid( kitName ) )
            {
                Notify( configuration.messages.noKitName );
                return false;
            }

            if ( HasCoolDown( out coolDown ) )
            {
                Notify( configuration.messages.coolDown, coolDown );
                return false;
            }

            return true;
        }

        private bool HasReachedKitLimit()
        {
            if ( FindPlayerAutoKit().kits?.Count >= configuration.settings.kitLimit )
            {
                Notify( configuration.messages.kitLimitReached, configuration.settings.kitLimit );
                return true;
            }
            return false;
        }

        private bool HasCoolDown( out long coolDown )
        {
            var userId = player.userID;
            var commandTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long lastCommandTime;
            coolDown = 0;

            if ( configuration.playerCoolDowns.TryGetValue( userId, out lastCommandTime ) && commandTime - lastCommandTime < configuration.settings.coolDown )
            {
                coolDown = lastCommandTime + configuration.settings.coolDown - commandTime;
                return true;
            }

            configuration.playerCoolDowns[userId] = commandTime;

            return false;
        }

        private bool IsKitNameValid( string kitName )
        {
            return !string.IsNullOrEmpty( kitName );
        }

        private Kit<T> FindKit( string kitName )
        {
            return playerAutoKits.Find( p => p.id == player.userID )?.kits?.Find( s => s.name == kitName );
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