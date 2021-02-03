## AutoKit Usage

```csharp
using Newtonsoft.Json;
using Oxide.Ext.AutoKit;
using Oxide.Ext.AutoKit.Messages;
using Oxide.Ext.AutoKit.Models;

namespace Oxide.Plugins
{
    [Info("AutoKit Extension Example", "Your Name", "1.0.0")]
    class AutoKitExtExample : RustPlugin
    {
       private AutoKit<YourKit> autoKit {get; set;}
       private YourKitMessages yourKitMessages {get; set;}

       // Create your AutoKit in the OnServerInitialized Oxide Hook.
       void OnServerInitialized()
        {
            messages = new YourKitMessages(); // or new AutoKitMessages()

            autoKit = new AutoKit<ItemSkin>( this, messages );  // You can optionally send in an int cooldown time
        }

        // Example of a command using the AutoKit delegates
        [ChatCommand( "yourcommand" )]
        void YourCommand( BasePlayer player, string command, string[] args ) 
        {
            var action = args.ElementAtOrDefault( 0 );
            var kitName = args.ElementAtOrDefault( 1 ) ?? action;

            switch ( action )
            {
                case "save":
                    autoKit.SaveKit( player, kitName, SaveKit );  // SaveKit requires a delegate implementation
                    break;
                case "list":
                    autoKit.ListKits( player ); // Lists kits to players
                    break;
                case "remove":
                    autoKit.RemoveKit( player, kitName ); // Remove a Kit
                    break;
                case "whateverYouWant":
                    DoAThing( player );  // A thing you want to do with AutoKit
                    break;
                default:
                    autoKit.ApplyKit( player, kitName, ApplyKit ); // ApplyKit requires a delegate implementation
                    break;
            }
        }

        // Example implementations of the Save and Apply delegates in AutoKit
        private AutoKit<YourKit>.KitToBeApplied ApplyKit = ( BasePlayer player, ref Kit<YourKit> kit ) =>
        {
            if ( null == kit ) return;
                // Do stuff with your kit in here on apply, the kit passed is a reference, so you can't change the kit here.
                // Kits are not edited, if a kit name is passed in on save that matches an existing kit, it will overwrite that kit.
        };

        private AutoKit<YourKit>.KitToSave SaveKit = ( player, kitName ) =>
        {
            return new Kit<YourKit>
            {
                name = kitName, // This is required
                items = new List<YourKit> { new YourKit() } // Whatever you want saved in YourKit, this is where you do it.
            };
        };

        // Your plugin can also do other things and call on AutoKit for help
        public void DoAThing( BasePlayer player ) 
        {
            autoKit.SendMessage( player, yourKitMessages.Random ) // Send a Message
            autoKit.SendMessage( player, yourKitMessage.MessageWithArgs, "your args" ) // uses params object[] so send whatever, it will format your message.

            autoKit.HasCoolDown( player ) // check if a player has a cool down.

            // See source for more useful methods.
        }

        // You need to define what a kit is, AutoKit doesn't care, but the properties you want saved need to be Json serializable. 
        public class YourKit
        {
            [JsonProperty( "anInterestingProperty" )]
            public ulong anInterestingProperty { get; set; }
            [JsonProperty( "anotherInterestingProperty" )]
            public ulong anotherInterestingProperty { get; set; }
            [JsonIgnore]
            public string aThingIDoNotWantSaved {get; set;}
        }

        // You can define your own messages for use with the SendMessage method.  AutoKit requires an IAutoKitMessages, pass in the existing AutoKitMessages or make your own.
        public class YourKitMessages : AutoKitMessages
        {
            public string Random { get; set; } = "Your random message";
            public string MessageWithArgs { get; set; } = "Your random message with args {0}";
        }
    }
}