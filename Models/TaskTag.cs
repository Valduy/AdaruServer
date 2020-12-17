#nullable disable

namespace Models
{
    public partial class TaskTag
    {
        public int IdTask { get; set; }
        public int IdTag { get; set; }

        public virtual Tag IdTagNavigation { get; set; }
        public virtual Task IdTaskNavigation { get; set; }
    }
}
