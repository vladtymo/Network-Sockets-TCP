using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData
{
    public enum OperationType { Add = 1, Sub, Mult, Div };
    public class Request
    {
        public double A { get; set; }
        public double B { get; set; }
        public OperationType Operation { get; set; }
    }

}
