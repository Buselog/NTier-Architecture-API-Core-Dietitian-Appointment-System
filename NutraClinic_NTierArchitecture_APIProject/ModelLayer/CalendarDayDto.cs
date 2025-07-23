using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class CalendarDayDto
    {
        public DateTime Date { get; set; }
        public bool IsFull { get; set; }
        public List<string> TakenHours { get; set; }
    }
}
