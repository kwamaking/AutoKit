namespace Oxide.Ext.AutoKit.Messages
{
    public class AutoKitMessages : IAutoKitMessages
    {
        public string saved { get; set; } = "You have saved: <color=green>{0}</color>.";
        public string removed { get; set; } = "You have removed: <color=green>{0}</color>.";
        public string noKitName { get; set; } = "You must supply a kit name.";
        public string noKit { get; set; } = "You don't have a kit named: <color=green>{0}</color> ";
        public string applied { get; set; } = "Your kit <color=green>{0}</color> has been applied";
        public string list { get; set; } = "Kit: <color=green>{0}</color>";
        public string coolDown { get; set; } = "Please wait <color=green>{0}s</color> and try again.";
        public string kitLimitReached { get; set; } = "You have saved the maximum of <color=green>{0}</color> kits. Consider removing a kit if you would like to add another.";
    }
}