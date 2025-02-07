using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Dal
{
    public class Table : Attribute
    {
        public string Name { get; }

        public Table(string name)
        {
            Name = name;
        }
    }

    public class Identity : Attribute { }

    public class Composite : Attribute { }
}
