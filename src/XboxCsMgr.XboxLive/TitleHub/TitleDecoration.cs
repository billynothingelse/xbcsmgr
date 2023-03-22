namespace XboxCsMgr.XboxLive.TitleHub
{
    public class TitleDecoration
    {
        public string TitleId { get; set; }

        public string Pfn { get; set; }

        public string BingId { get; set; }

        public string ServiceConfigId { get; set; }

        public string WPProductId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string[] Devices { get; set; }

        public string DisplayImage { get; set; }
    }

    public class TitleDecorationResult
    {
        public string XboxUserId { get; set; }

        public TitleDecoration[] Titles { get; set; }
    }
}
