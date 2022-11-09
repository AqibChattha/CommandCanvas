using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    class Circle : Shape
    {
        int radius;

        public Circle(Color color, int x, int y, int radius) : base(color, x, y)
        {
            this.radius = radius;

        }

        public override void draw(Graphics g)
        {
            int halfRadius = radius / 2;
            Pen penb = new Pen(color, (float) 1.3);
            SolidBrush b = new SolidBrush(color);
            g.FillEllipse(b, x - halfRadius, y - halfRadius, radius, radius);
            g.DrawEllipse(penb, x - halfRadius, y - halfRadius, radius, radius);
            penb.Dispose();
            b.Dispose();

        }


        public void drawFilled(Graphics g)
        {
            int halfRadius = radius / 2;
            SolidBrush b = new SolidBrush(color);
            g.FillEllipse(b, x - halfRadius, y - halfRadius, radius, radius);
            b.Dispose();
        }

        public void drawOutLined(Graphics g)
        {
            int halfRadius = radius / 2;
            Pen penb = new Pen(color, (float)1.3);
            g.DrawEllipse(penb, x - halfRadius, y - halfRadius, radius, radius);
            penb.Dispose();
        }

        public override string ToString()
        {
            return base.ToString() + "    " + this.radius;
        }
    }
}
