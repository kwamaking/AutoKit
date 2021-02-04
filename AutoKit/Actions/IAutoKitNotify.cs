namespace Oxide.Ext.AutoKit.Actions

{
    public interface IAutoKitNotify<T>
    {
        IAutoKitAction<T> Notify();
    }
}