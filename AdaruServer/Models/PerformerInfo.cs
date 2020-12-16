using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class PerformerInfo
    {
        public int? Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Path { get; set; }
        public string Resume { get; set; }
        public decimal? Raiting { get; set; }
        public long? Expirience { get; set; }
        // TODO: теги
        //public ICollection<Tag> Tags { get; set; }
    }
}
