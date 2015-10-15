using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pegPuzzle
{
    public partial class VisualPuzzle : Form
    {
        Pen lightGrayPen = new Pen(Color.LightGray, 0.2f);
        Pen blackPen = new Pen(Color.Black, 0.5f);
        Pen redPen = new Pen(Color.Red, 1f);
        Brush brushOut = new SolidBrush(Color.Red);
        Brush brushIn = new SolidBrush(Color.Green);
        private int n;
        List<Cells> cells = new List<Cells>();

        public VisualPuzzle(int n)
        {
            InitializeComponent();
            this.n = n;
            displayTriangle();
        }

        private void VisualPuzzle_Load(object sender, EventArgs e)
        {

        }

        private void VisualPuzzle_Paint(object sender, PaintEventArgs e)
        {
            Point[] points = {
                      new Point(12, 0),
                      new Point(25, 5),
                      new Point(25, 18),
                      new Point(12, 23),
                      new Point(0, 18),
                      new Point(0, 5)
            };

            for (int x = 0; x <= this.Width; x += 25)
            {
                for (int y = 0; y <= this.Height; y += 36)
                {
                    e.Graphics.DrawPolygon(Pens.LightGray, Array.ConvertAll(points, p => new Point(p.X + x, p.Y + y)));
                    e.Graphics.DrawPolygon(Pens.LightGray, Array.ConvertAll(points, p => new Point(p.X + x - 13, p.Y + y + 18)));
                }
            }

            for (int x = 0; x <= cells.Count - 1; x++)
            {
                Rectangle r = cellRect(cells[x].column, cells[x].row);
                if (!(cells[x].fillColor == null))
                {
                    r.Inflate(-6, 0);
                    e.Graphics.FillEllipse(new SolidBrush(cells[x].fillColor), r);
                }
            }
        }

        private Rectangle cellRect(int columnIndex, int rowIndex)
        {
            return new Rectangle(cellX(columnIndex, rowIndex), cellTop(columnIndex, rowIndex) + 5, 25, 13);
        }

        private int cellX(int columnIndex, int rowIndex)
        {
            if (rowIndex % 2 == 0)
            {
                return columnIndex * 25;
            }
            else
            {
                return (columnIndex * 25) - 12;
            }
        }


        private int cellTop(int columnIndex, int rowIndex)
        {
            if (rowIndex % 2 == 0)
            {
                if (rowIndex == 0)
                {
                    return 0;
                }
                return Convert.ToInt32(rowIndex / 2) * 36;
            }
            else
            {
                return Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(rowIndex) / 2) * 36 - 18);
            }
        }

        private void displayTriangle()
        {
            cells.Clear();

            int startColumn = 0;

            startColumn = 15;
            fillCell(Color.Red, startColumn, 2);

            for (int y = 3; y <= 3 + (Convert.ToInt32(n) - 2); y++)
            {
                for (int x = 0; x <= y - 2; x++)
                {
                    fillCell(Color.Red, startColumn + x, y);
                }
                if (y % 2 == 1)
                {
                    startColumn -= 1;
                }
            }

            this.Invalidate();
        }

        private void fillCell(Color fillColor, int columnIndex, int rowIndex)
        {
            cells.Add(new Cells
            {
                column = columnIndex,
                row = rowIndex,
                fillColor = fillColor
            });
            this.Invalidate();
        }
    }
}
