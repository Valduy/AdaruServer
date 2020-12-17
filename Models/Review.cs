using System;

#nullable disable

namespace Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public short Mark { get; set; }
        public int IdSource { get; set; }
        public int IdTarget { get; set; }
        public DateTime Time { get; set; }

        public virtual Client IdSourceNavigation { get; set; }
        public virtual Client IdTargetNavigation { get; set; }
    }
}
