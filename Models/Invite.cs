#nullable disable

namespace Models
{
    public partial class Invite
    {
        public int IdTask { get; set; }
        public int IdPerformer { get; set; }

        public virtual Client IdPerformerNavigation { get; set; }
        public virtual Task IdTaskNavigation { get; set; }
    }
}
