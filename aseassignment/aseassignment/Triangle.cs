using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aseassignment
{
    class Triangle : Shape
    {
        int size;

        public Triangle(Color color, int x, int y, int size) : base(color, x, y)
        {
            this.size = size;

        }

        public override void draw(Graphics g)
        {
            Pen pen = new Pen(color, (float)1.3);
            SolidBrush b = new SolidBrush(color);
            int halfSize = size / 2;
            int xvar = x - halfSize;
            int yvar = y - halfSize;
            Point[] points = new Point[3];
            points[0] = new Point(xvar, yvar);
            points[1] = new Point(xvar + size, yvar);
            points[2] = new Point(xvar + size / 2, yvar + size);
            g.FillPolygon(b, points);
            g.DrawPolygon(pen, points);
            pen.Dispose();
            b.Dispose();
        }

        public void drawFilled(Graphics g)
        {
            SolidBrush b = new SolidBrush(color);
            int halfSize = size / 2;
            int xvar = x - halfSize;
            int yvar = y - halfSize;
            Point[] points = new Point[3];
            points[0] = new Point(xvar, yvar);
            points[1] = new Point(xvar + size, yvar);
            points[2] = new Point(xvar + size / 2, yvar + size);
            g.FillPolygon(b, points);
            b.Dispose();
        }

        public void drawOutLined(Graphics g)
        {
            Pen pen = new Pen(color, (float)1.3);
            int halfSize = size / 2;
            int xvar = x - halfSize;
            int yvar = y - halfSize;
            Point[] points = new Point[3];
            points[0] = new Point(xvar, yvar);
            points[1] = new Point(xvar + size, yvar);
            points[2] = new Point(xvar + size / 2, yvar + size);
            g.DrawPolygon(pen, points);
            pen.Dispose();
        }

        public override string ToString()
        {
            return base.ToString() + "    " + this.size;
        }
    }
}
