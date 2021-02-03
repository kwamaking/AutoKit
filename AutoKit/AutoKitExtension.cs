namespace Oxide.Ext.AutoKit
{
    using Oxide.Core;
    using Oxide.Core.Extensions;
    public class AutoKitExtension : Extension
    {
        public override string Name => "AutoKit";
        public override string Author => "kwamaking";
        public override VersionNumber Version => new VersionNumber( 1, 0, 0 );

        public AutoKitExtension( ExtensionManager manager ) : base( manager )
        {
        }
    }
}