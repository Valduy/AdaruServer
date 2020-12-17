#nullable disable

namespace Models
{
    public partial class PerformerTag
    {
        public int IdPerformer { get; set; }
        public int IdTag { get; set; }

        public virtual Client IdPerformerNavigation { get; set; }
        public virtual Tag IdTagNavigation { get; set; }
    }
}
