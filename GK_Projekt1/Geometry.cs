using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GK_Projekt1;

namespace GK_Projekt1
{
    public class Polygon
    {
        private List<System.Windows.Point> _vertices;
        public Color _color = Colors.Black;
        public Color _vertexColor = Colors.Red;
        public int _dotSize = 7;
        public int _lineSize = 2;
        public Canvas _canvas;
        public bool _finished = false;
        public int _clickAccuracy = 3;

        public Polygon(Canvas canvas)
        {
            _vertices = new List<System.Windows.Point>();
            _canvas = canvas;
        }

        public List<System.Windows.Point> Vertices
        {
            get
            {
                return _vertices;
            }
            set
            {
                _vertices = value;
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public void Draw(System.Windows.Controls.Canvas canvas)
        {
            for (int i = 0; i < _vertices.Count - 1; i++)
                DrawLine(_vertices[i], _vertices[i + 1], _color, _lineSize, canvas);
            if(_finished)
                DrawLine(_vertices[0], _vertices[_vertices.Count - 1], _color, _lineSize, canvas);
            foreach (var i in _vertices)
                DrawPoint(i, _vertexColor, _dotSize, canvas);
        }

        public Line DrawLine(Point p1, Point p2, Color c, int size, Canvas canvas)
        {
            var temp = new Line();
            var b = new SolidColorBrush();
            b.Color = c;
            temp.X1 = p1.X;
            temp.X2 = p2.X;
            temp.Y1 = p1.Y;
            temp.Y2 = p2.Y;
            temp.StrokeThickness = size;
            temp.Stroke = b;
            temp.SnapsToDevicePixels = true;
            temp.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            canvas.Children.Add(temp);
            canvas.UpdateLayout();
            return temp;
        }

        public void DrawPoint(Point p, Color c, int size, Canvas canvas)
        {
            var temp = new Ellipse();
            var b = new SolidColorBrush();
            b.Color = c;
            temp.Width = size;
            temp.Height = size;
            temp.Stroke = b;
            temp.StrokeThickness = 1;
            temp.Fill = b;
            Canvas.SetLeft(temp, p.X - size / 2);
            Canvas.SetTop(temp, p.Y - size / 2);
            temp.SnapsToDevicePixels = true;
            temp.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            canvas.Children.Add(temp);
        }

        public bool IsInsideFirstVertex(Point p)
        {
            if (IsInsideVertex(p, Vertices[0]) && Vertices.Count > 2)
                return true;
            return false;
        }

        public bool IsInsideAnyLine(Point p)
        {
            for (var i = 0; i < _vertices.Count - 1; i++)
                if (IsInsideLine(p, _vertices[i], _vertices[i + 1]))
                    return true;
            return false;
        }

        public Point IsInsideAnyVertex(Point p)
        {
            foreach (var i in Vertices)
                if (IsInsideVertex(p, i))
                    return i;
            return new Point(-1, -1);
        }

        public bool IsInsideVertex(Point cursor, Point vertex)
        {
            if (Math.Abs(cursor.X - vertex.X) < _dotSize / 2 + _clickAccuracy && Math.Abs(cursor.Y - vertex.Y) < _dotSize / 2 + _clickAccuracy)
                return true;
            return false;
        }

        public bool IsInsideLine(Point cursor, Point lineBeginning, Point lineEnding)
        {
            var a = (lineBeginning.Y - lineEnding.Y) / (lineBeginning.X - lineEnding.X);
            var b = lineBeginning.Y - a * lineBeginning.X; 
            if (Math.Abs(a * cursor.X + b - cursor.Y) < _lineSize / 2 + _clickAccuracy)
                return true;
            return false;
        }
    }
}
