using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManagerV2.Models
{
    public class Workout
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<Exercise> Exercises { get; set; } = new();
    }


}
