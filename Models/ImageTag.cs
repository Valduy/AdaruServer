#nullable disable

namespace Models
{
    public partial class ImageTag
    {
        public int IdImage { get; set; }
        public int IdTag { get; set; }

        public virtual Image IdImageNavigation { get; set; }
        public virtual Tag IdTagNavigation { get; set; }
    }
}
