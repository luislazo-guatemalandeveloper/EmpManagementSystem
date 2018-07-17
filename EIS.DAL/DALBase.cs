using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIS.DAL
{
    public class DALBase
    {
        protected EISDBContext db;

        public DALBase()
        {
            db = new EISDBContext();
        }
    }
}
