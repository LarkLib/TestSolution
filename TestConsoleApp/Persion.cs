using System;
using System.Collections.Generic;

namespace TestConsoleApp
{
    public partial class Persion
    {
        public Guid Id { get; set; }
        public int Seq { get; set; }
        public int? Age { get; set; }
        public string Name { get; set; }
        public decimal Wage { get; set; }
        public DateTime? Hiredate { get; set; }
        public DateTime Lastupdatetime { get; set; }
    }
}
