using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
        private List<Relation> _relations;
        public bool IsConstLength { get; set; }

        public Polygon(Canvas canvas)
        {
            _vertices = new List<System.Windows.Point>();
            _canvas = canvas;
            _relations = new List<Relation>();
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

        public List<Relation> Relations
        {
            get
            {
                return _relations;
            }
            set
            {
                _relations = value;
            }
        }

        public void Draw(System.Windows.Controls.Canvas canvas)
        {
            for (int i = 0; i < _vertices.Count - 1; i++)
                DrawLine(_vertices[i], _vertices[i + 1], _color, _lineSize, canvas);
            if (_finished)
                DrawLine(_vertices[0], _vertices[_vertices.Count - 1], _color, _lineSize, canvas);
            foreach (var i in Vertices)
                DrawPoint(i, _vertexColor, _dotSize, canvas);
            foreach (var i in Relations)
                DrawRelation(i);
        }

        public Line DrawLine(Point p1, Point p2, Color c, int size, Canvas canvas)
        {
            var temp = new Line();
            var b = new SolidColorBrush();
            b.Color = c;
            try
            {
                temp.X1 = p1.X;
                temp.X2 = p2.X;
                temp.Y1 = p1.Y;
                temp.Y2 = p2.Y;
            }catch(ArgumentException e)
            {
                
            }
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

        public void DrawRelation(Relation rel)
        {
            var ret = new TextBlock();
            ret.FontSize = 15;
            ret.Foreground = Brushes.Black;
            if (rel.Type == RelationTypes.Perpendicular)
                ret.Text = ((char)0x27C2).ToString() + " " + rel.ID.ToString();
            else if (rel.Type == RelationTypes.Parallel)
                ret.Text = ((char)0x2225).ToString() + " " + rel.ID.ToString();
            else if (rel.Type == RelationTypes.Const)
                ret.Text = "const";

            if (Math.Abs(rel.FirstEdge.Item1.X - rel.FirstEdge.Item2.X) >= Math.Abs(rel.FirstEdge.Item1.Y - rel.FirstEdge.Item2.Y))
            {
                Canvas.SetLeft(ret, Math.Min(rel.FirstEdge.Item1.X, rel.FirstEdge.Item2.X) + Math.Abs(rel.FirstEdge.Item1.X - rel.FirstEdge.Item2.X) / 2);
                Canvas.SetTop(ret, Math.Min(rel.FirstEdge.Item1.Y, rel.FirstEdge.Item2.Y) + Math.Abs(rel.FirstEdge.Item1.Y - rel.FirstEdge.Item2.Y) / 2 - 20);
            }
            else
            {
                Canvas.SetLeft(ret, Math.Min(rel.FirstEdge.Item1.X, rel.FirstEdge.Item2.X) + Math.Abs(rel.FirstEdge.Item1.X - rel.FirstEdge.Item2.X) / 2 + 10);
                Canvas.SetTop(ret, Math.Min(rel.FirstEdge.Item1.Y, rel.FirstEdge.Item2.Y) + Math.Abs(rel.FirstEdge.Item1.Y - rel.FirstEdge.Item2.Y) / 2);
            }
            _canvas.Children.Add(ret);
        }

        public bool IsInsideFirstVertex(Point p)
        {
            if (IsInsideVertex(p, Vertices[0]) && Vertices.Count > 2)
                return true;
            return false;
        }

        public (Point, Point) IsInsideAnyLine(Point p)
        {
            for (var i = 0; i < _vertices.Count; i++)
                if (IsInsideLine(p, _vertices[i], _vertices[(i + 1) % _vertices.Count]))
                    return (_vertices[i], _vertices[(i + 1) % _vertices.Count]);
            return (new Point(-1, -1), new Point(-1, -1));
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
            if (Math.Abs(a) > 20 && Math.Abs(cursor.X - (lineBeginning.X + lineEnding.X) / 2) < _lineSize + _clickAccuracy
                && cursor.Y < Math.Max(lineBeginning.Y, lineEnding.Y)
                && cursor.Y > Math.Min(lineBeginning.Y, lineEnding.Y))
                return true;
            if (Math.Abs(a * cursor.X + b - cursor.Y) <= _lineSize / 2 + _clickAccuracy && 
                cursor.X >= Math.Min(lineBeginning.X, lineEnding.X) && cursor.X <= Math.Max(lineBeginning.X, lineEnding.X) && 
                cursor.Y >= Math.Min(lineBeginning.Y, lineEnding.Y) && cursor.Y <= Math.Max(lineBeginning.Y, lineEnding.Y))
                return true;
            return false;
        }

        public void AddRelation(Point p1, Point p2, Polygon poly, Point p3, Point p4, RelationTypes type, int id)
        {
            var ret = new Relation();
            ret.FirstEdge = (p1, p2);
            ret.RelatedPolygon = poly;
            ret.SecondEdge = (p3, p4);
            ret.Type = type;
            ret.ID = id;
            Relations.Add(ret);
        }
    }
    public class Relation
    {
        public (Point, Point) FirstEdge;
        public Polygon RelatedPolygon;
        public (Point, Point) SecondEdge;
        public RelationTypes Type;
        public int ID;
        public bool Visited = false;
    }

    public enum RelationTypes
    {
        Perpendicular,
        Parallel,
        Const
    }
    // https://www.geeksforgeeks.org/how-to-check-if-a-given-point-lies-inside-a-polygon/#:~:text=Draw%20a%20horizontal%20line%20to,on%20an%20edge%20of%20polygon.
    class InsideOutsideLib
    {
        static bool onSegment(Point p, Point q, Point r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) && q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
            {
                return true;
            }
            return false;
        }

        static double orientation(Point p, Point q, Point r)
        {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0)
            {
                return 0;
            }
            return (val > 0) ? 1 : 2;
        }

        static bool doIntersect(Point p1, Point q1, Point p2, Point q2)
        {
            double o1 = orientation(p1, q1, p2);
            double o2 = orientation(p1, q1, q2);
            double o3 = orientation(p2, q2, p1);
            double o4 = orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
            {
                return true;
            }
            if (o1 == 0 && onSegment(p1, p2, q1))
            {
                return true;
            }
            if (o2 == 0 && onSegment(p1, q2, q1))
            {
                return true;
            }
            if (o3 == 0 && onSegment(p2, p1, q2))
            {
                return true;
            }
            if (o4 == 0 && onSegment(p2, q1, q2))
            {
                return true;
            }
            return false;
        }


        public Polygon isInside(Polygon poly, Point p)
        {
            var polygon = poly.Vertices.ToArray();
            var n = poly.Vertices.Count();
            if (n < 3)
            {
                return null;
            }
            Point extreme = new Point(double.MaxValue, p.Y);
            int count = 0, i = 0;
            do
            {
                int next = (i + 1) % n;
                if (doIntersect(polygon[i],
                                polygon[next], p, extreme))
                {
                    if (orientation(polygon[i], p, polygon[next]) == 0)
                    {
                        if (onSegment(polygon[i], p, polygon[next]))
                            return poly;
                        else
                            return null;
                    }
                    count++;
                }
                i = next;
            } while (i != 0);

            if (count % 2 == 1)
                return poly;
            else
                return null;
        }
    }
}
