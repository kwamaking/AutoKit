namespace Oxide.Ext.AutoKit.Messages
{
    public interface IAutoKitMessages
    {
        string saved { get; set; }
        string removed { get; set; }
        string noKit { get; set; }
        string kitExists { get; set; }
        string applied { get; set; }
        string list { get; set; }
        string coolDown { get; set; }
        string kitLimitReached { get; set; }
    }
}