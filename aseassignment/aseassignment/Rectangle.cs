using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace aseassignment
{
    class Rectangle : Shape
    {
        int width, height;

        public Rectangle(Color color, int x, int y, int width, int height) : base(color,x,y)
        {
            this.width = width;     
            this.height = height;

        }

        public override void draw(Graphics g)
        {
            Pen penb = new Pen(color, (float) 1.3);
            SolidBrush b = new SolidBrush(color);
            g.FillRectangle(b,x,y,width,height);
            g.DrawRectangle(penb,x,y,width,height);
            penb.Dispose();
            b.Dispose();
        }


        public void drawFilled(Graphics g)
        {
            SolidBrush b = new SolidBrush(color);
            g.FillRectangle(b, x, y, width, height);
            b.Dispose();
        }

        public void drawOutLined(Graphics g)
        {
            Pen penb = new Pen(color, (float)1.3);
            g.DrawRectangle(penb, x, y, width, height);
            penb.Dispose();
        }

    }
}
