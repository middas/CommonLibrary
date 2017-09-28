namespace CommonLibrary.Pager
{
    public class PagerRenderOptions
    {
        public static PagerRenderOptions Default
        {
            get
            {
                return new PagerRenderOptions
                {
                    AlwaysShow = true,
                    Delimiter = "&nbsp;",
                    Ellipsis = null,
                    FirstHtml = "<<",
                    LastHtml = ">>",
                    NextHtml = ">",
                    PreviousHtml = "<",
                    ShowFirstLastLinks = true,
                    ShowNextPreviousLinks = true,
                    PagerTag = "div",
                    LinkedItemTag = null,
                    DisabledItemTag = "span",
                    PagerClass = "pager",
                    LinkedItemClass = null,
                    DisabledItemClass = null
                };
            }
        }

        public bool AlwaysShow { get; set; }
        public string Delimiter { get; set; }
        public string DisabledItemClass { get; set; }
        public string DisabledItemTag { get; set; }
        public EllipsisHelper Ellipsis { get; set; }
        public string FirstHtml { get; set; }
        public string LastHtml { get; set; }
        public string LinkedItemClass { get; set; }
        public string LinkedItemTag { get; set; }
        public string NextHtml { get; set; }
        public string PagerClass { get; set; }
        public string PagerTag { get; set; }
        public string PreviousHtml { get; set; }
        public bool ShowFirstLastLinks { get; set; }
        public bool ShowNextPreviousLinks { get; set; }

        public class EllipsisHelper
        {
            public bool ShowEllipsis { get; set; }

            public int TotalPagesToShow { get; set; }
        }
    }
}