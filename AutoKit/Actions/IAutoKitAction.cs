namespace Oxide.Ext.AutoKit.Actions
{
    public interface IAutoKitAction<T>
    {
        IAutoKitDestructive<T> WithKit( string kitName );
        IAutoKitDestructive<T> WithNewKit( string kitName );
        IAutoKitNotify<T> ToNotify();
        IAutoKitNonDestructive<T> ToNonDestructive();
    }
}