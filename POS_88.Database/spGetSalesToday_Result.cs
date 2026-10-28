using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_88.Database
{
    public class spGetSalesToday_Result
    {
        public Decimal? SALES { get; set; }

        public Decimal? COST { get; set; }

        public Decimal? DISCOUNT { get; set; }
    }
}
