using System;
using System.Collections.Generic;
using System.Text;

namespace VolleyballApp.Model
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string? Position { get; set; }
        public List<string> Clubs { get; set; }
    }
}
