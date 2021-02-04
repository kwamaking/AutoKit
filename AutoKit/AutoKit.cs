using System;
using System.Collections.Generic;
using Oxide.Core;
using Oxide.Ext.AutoKit.Messages;
using Oxide.Ext.AutoKit.Models;
using Oxide.Ext.AutoKit.Settings;
using Oxide.Ext.AutoKit.Actions;

namespace Oxide.Ext.AutoKit
{
    public sealed class AutoKit<T>
    {
        private AutoKitConfiguration<T> configuration { get; set; }
        private List<PlayerAutoKit<T>> playerAutoKits { get; set; }
        private object readWriteLock { get; set; } = new object();

        public AutoKit( IAutoKitMessages messages, AutoKitSettings settings )
        {
            configuration = new AutoKitConfiguration<T>( messages, settings );
            playerAutoKits = Load();
        }

        public void With( BasePlayer player, Action<IAutoKitAction<T>> action )
        {
            try
            {
                lock ( readWriteLock )
                {
                    using ( var autoKitAction = new AutoKitAction<T>( configuration, player, playerAutoKits ) )
                    {
                        action( autoKitAction );
                    }
                }
            }
            catch ( Exception e )
            {
                Interface.Oxide.LogError( $"Failed to execute auto kit action:  {e.Message} {e.StackTrace}" );

            }
        }

        public void Save()
        {
            try
            {
                lock ( readWriteLock )
                {

                    Interface.Oxide.DataFileSystem.WriteObject( configuration.settings.pluginName, playerAutoKits );
                }
            }
            catch ( Exception e )
            {
                Interface.Oxide.LogError( $"Failed to save player auto kits:  {e.Message} {e.StackTrace}" );

            }

        }

        private List<PlayerAutoKit<T>> Load()
        {
            try
            {
                return Interface.Oxide.DataFileSystem.ReadObject<List<PlayerAutoKit<T>>>( configuration.settings.pluginName );
            }
            catch ( Exception e )
            {
                Interface.Oxide.LogError( $"Failed to load player auto kits:  {e.Message} {e.StackTrace}" );
                return new List<PlayerAutoKit<T>>();
            }
        }
    }
}
