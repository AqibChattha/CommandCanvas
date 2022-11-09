using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    abstract class Shape : Shapes
    {
        protected Color color;
        protected int x, y;

        public Shape(Color color, int x, int y)
        {
            this.color = color;
            this.x = x; this.y = y;
        }

        public abstract void draw(Graphics g);

        public override string ToString()
        {
            return base.ToString() + "    " + this.x + "," + this.y + ":";
        }

    }
}
