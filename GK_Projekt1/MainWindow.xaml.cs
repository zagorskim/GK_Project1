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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Polygon> _polygons;
        //private List<Line> _lines;
        private Statuses _status;
        private WriteableBitmap _wb;
        public const int dpiX = 96;
        public const int dpiY = 96;
        private Point _lastMousePosition;

        public List<Polygon> Polygons
        {
            get
            {
                return _polygons;
            }
            set
            {
                _polygons = value;
            }
        }
        //public List<Line> Lines
        //{
        //    get
        //    {
        //        return _lines;
        //    }
        //    set
        //    {
        //        _lines = value;
        //    }
        //}
        public Statuses Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public WriteableBitmap Picture
        {
            get
            {
                return _wb;
            }
            set
            {
                _wb = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            //RenderOptions.SetBitmapScalingMode(Image1, BitmapScalingMode.NearestNeighbor);
            //RenderOptions.SetEdgeMode(Image1, EdgeMode.Aliased);
            _status.Mode = Modes.Adding;
            _status.BresenhofEnabled = false;
            _polygons = new List<Polygon>();
            Canvas1.MouseLeftButtonDown += new MouseButtonEventHandler(Canvas1_MouseLeftButtonDown);
            Canvas1.Background = Brushes.Transparent;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Status.Mode != Modes.AddingInProgress)
                _status.Mode = Modes.Adding;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(Status.Mode != Modes.AddingInProgress)
                _status.Mode = Modes.Deletion;
        }
        
        private void Canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(Canvas1);
            if (Status.Mode == Modes.Adding)
            {
                _status.CurrentPolygon = new Polygon(Canvas1);
                _status.CurrentPolygon.Color = PolyColors.colors[_status.PolyCount++ % PolyColors.count];
                DrawPoint(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), Colors.Red, Status.CurrentPolygon._dotSize);
                Status.CurrentPolygon.Vertices.Add(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y));
                _status.Mode = Modes.AddingInProgress;
            }
            else if (Status.Mode == Modes.AddingInProgress)
            {
                if (Status.CurrentPolygon.IsInsideFirstVertex(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y)))
                {
                    _polygons.Add(Status.CurrentPolygon);
                    _polygons.Last()._finished = true;
                    _status.Mode = Modes.Idle;
                    RedrawCanvas(Canvas1);
                }
                else
                {
                    DrawLine(Status.CurrentPolygon.Vertices.Last(), new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), Colors.Black, Status.CurrentPolygon._lineSize);
                    DrawPoint(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), Colors.Red, Status.CurrentPolygon._dotSize);
                    Status.CurrentPolygon.Vertices.Add(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y));
                }
            }
            else if (Status.Mode == Modes.Deletion)
            {

            }
            else if (Status.Mode == Modes.Idle)
            {
                for (var i = 0; i < Polygons.Count; i++)
                {

                    var p = Polygons[i].IsInsideAnyVertex(point);
                    if (p != new Point(-1, -1))
                    {
                        _status.Mode = Modes.MovingVertex;
                        _status.CurrentPolygon = Polygons[i];
                        _status.VertexInMove = Status.CurrentPolygon.Vertices.IndexOf(p);
                        break;
                    }
                    var pair = Polygons[i].IsInsideAnyLine(point);
                    if (pair.Item1 != new Point(-1, -1))
                    {
                        _status.Mode = Modes.MovingEdge;
                        _status.CurrentPolygon = Polygons[i];
                        _status.EdgeInMove = (Status.CurrentPolygon.Vertices.IndexOf(pair.Item1), Status.CurrentPolygon.Vertices.IndexOf(pair.Item2));
                        break;
                    }
                }
            }
            _lastMousePosition = point;
            e.Handled = true;
        }

        private void Canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(Canvas1);
            if (Status.Mode == Modes.AddingInProgress)
            {
                Canvas1.Children.Clear();
                foreach (var i in _polygons)
                    i.Draw(Canvas1);

                if (Status.BresenhofEnabled == false)
                    _status.Ray = DrawLine(Status.CurrentPolygon.Vertices.Last(), new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), PolyColors.colors[(_status.PolyCount - 1) % PolyColors.count], Status.CurrentPolygon._lineSize);
                else
                    _status.BresenhofDump = DrawLineBresenhof(Status.CurrentPolygon.Vertices.Last(), new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), PolyColors.colors[(_status.PolyCount - 1) % PolyColors.count]);
                Status.CurrentPolygon.Draw(Canvas1);
                foreach (var i in Canvas1.Children)
                    if (i.GetType() == typeof(Line))
                    {
                        ((Line)i).MouseLeftButtonDown += Canvas1_MouseLeftButtonDown;
                        ((Line)i).MouseRightButtonDown += Canvas1_MouseRightButtonDown;
                    }
                    else
                    {
                        ((Ellipse)i).MouseLeftButtonDown += Canvas1_MouseLeftButtonDown;
                        ((Ellipse)i).MouseRightButtonDown += Canvas1_MouseRightButtonDown;
                    }
            }
            else if(Status.Mode == Modes.MovingVertex)
            {
                _status.CurrentPolygon.Vertices[Status.VertexInMove] = point;
                RedrawCanvas(Canvas1);
            }
            else if(Status.Mode == Modes.MovingEdge)
            {
                // Moving one polygon's edge sometimes grab random polygon's edge :/
                var ret = new Point(_status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1].X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1].Y - (_lastMousePosition.Y - point.Y));
                _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1] = ret;
                ret = new Point(_status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2].X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2].Y - (_lastMousePosition.Y - point.Y));
                _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2] = ret;
                RedrawCanvas(Canvas1);
            }
            _lastMousePosition = point;
            e.Handled = true;
        }

        private void Canvas1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            CancelInserting(e);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CancelInserting(e);
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            _status.BresenhofEnabled = true;
        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            _status.BresenhofEnabled = false;
        }

        private void Canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Status.Mode == Modes.MovingVertex || Status.Mode == Modes.MovingEdge || Status.Mode == Modes.Moving)
            {
                _status.Mode = Modes.Idle;
                _status.CurrentPolygon = null;
                _status.VertexInMove = -1;
                _status.EdgeInMove = (-1, -1);
            }
        }

        public void CancelInserting(RoutedEventArgs e)
        {
            if (Status.Mode != Modes.Adding)
            {
                _status.BresenhofDump = null;
                _status.CurrentPolygon = null;
                _status.PolyCount--;
                RedrawCanvas(Canvas1);
                _status.Mode = Modes.Idle;
            }
            e.Handled = true;
        }

        public void RedrawCanvas(Canvas canvas)
        {
            canvas.Children.Clear();
            foreach (var i in _polygons)
                i.Draw(canvas);
            if(Status.CurrentPolygon != null)
                Status.CurrentPolygon.Draw(canvas);
            foreach (var i in Canvas1.Children)
                if (i.GetType() == typeof(Line))
                {
                    ((Line)i).MouseLeftButtonDown += Canvas1_MouseLeftButtonDown;
                    ((Line)i).MouseRightButtonDown += Canvas1_MouseRightButtonDown;
                    ((Line)i).MouseLeftButtonUp += Canvas1_MouseLeftButtonUp;
                }
                else
                {
                    ((Ellipse)i).MouseLeftButtonDown += Canvas1_MouseLeftButtonDown;
                    ((Ellipse)i).MouseRightButtonDown += Canvas1_MouseRightButtonDown;
                    ((Ellipse)i).MouseLeftButtonUp += Canvas1_MouseLeftButtonUp;
                }
        }

        public Line DrawLine(Point p1, Point p2, Color c, int size)
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
            Canvas1.Children.Add(temp);
            Canvas1.UpdateLayout();
            return temp;
        }

        public List<Ellipse> DrawLineBresenhof(Point p1, Point p2, Color c)
        {
            var ret = new List<Ellipse>();
            var orientedSet = PointOrientation(p1, p2);
            var x1 = orientedSet.p1o.X;
            var y1 = orientedSet.p1o.Y;
            var x2 = orientedSet.p2o.X;
            var y2 = orientedSet.p2o.Y;
            var dx = x2 - x1;
            var dy = y2 - y1;
            var incrE = 2 * dy;
            var incrNE = 2 * (dy - dx);
            var d = 2 * dy - dx;
            var xf = x1;
            var yf = y1;
            var xb = x2;
            var yb = y2;

            ret.Add(SetPixel(new Point(xf, yf), c));
            ret.Add(SetPixel(new Point(xb, yb), c));
            while(xf < xb)
            {
                xf++;
                xb--;
                if (d < 0)
                    d += incrE;
                else
                {
                    d += incrNE;
                    yf++;
                    yb--;
                }
                ret.Add(SetPixel(new Point(xf, yf), c));
                ret.Add(SetPixel(new Point(xb, yb), c));
            }
            for (var i = 0; i < ret.Count; i++)
            {
                var x = Canvas.GetLeft(ret[i]);
                var y = Canvas.GetTop(ret[i]);
                switch (orientedSet.quartersToRotate)
                {
                    case 0:
                        Canvas.SetTop(ret[i], y1 - (Canvas.GetTop(ret[i]) - y1));
                        break;
                    case 1:
                        Canvas.SetTop(ret[i], y1 + x1 - x);
                        Canvas.SetLeft(ret[i], x1 - y1 + y);
                        break;
                    case 2:
                        Canvas.SetTop(ret[i], y2 - x2 + x);
                        Canvas.SetLeft(ret[i], x2 - y2 + y);
                        break;
                    case 4:
                        Canvas.SetTop(ret[i], y2 + (y2 - Canvas.GetTop(ret[i])));
                        break;
                    case 5:
                        Canvas.SetTop(ret[i], y2 + x2 - x);
                        Canvas.SetLeft(ret[i], x2 - y2 + y);
                        break;
                    case 6:
                        Canvas.SetTop(ret[i], y1 - x1 + x);
                        Canvas.SetLeft(ret[i], x1 - y1 + y);
                        break;
                }
            }
            return ret;
        }

        public void DrawPoint(Point p, Color c, int size)
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
            Canvas1.Children.Add(temp);
            
        }
        //private void Image1_Loaded(object sender, RoutedEventArgs e)
        //{
        //    _wb = new WriteableBitmap((int)DockPanel1.ActualWidth, (int)DockPanel1.ActualHeight, dpiX, dpiY, PixelFormats.Bgr32, null);
        //    var test = new int[Picture.PixelWidth * Picture.PixelHeight];
            
        //    Image1.Source = _wb;
        //}

        private Ellipse SetPixel(Point p, Color c)
        {
            var temp = new Ellipse();
            var b = new SolidColorBrush();
            b.Color = c;
            Canvas.SetLeft(temp, p.X);
            Canvas.SetTop(temp, p.Y);
            temp.StrokeThickness = 1;
            temp.Stroke = b;
            temp.SnapsToDevicePixels = true;
            temp.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            Canvas1.Children.Add(temp);
            return temp;
        }

        private void SetPixel(int x, int y, Color c)
        {
            int column = x;
            int row = y;

            try
            {
                _wb.Lock();

                unsafe
                {
                    IntPtr pBackBuffer = _wb.BackBuffer;

                    pBackBuffer += row * _wb.BackBufferStride;
                    pBackBuffer += column * 4;

                    int color_data = 255 << 16; // R
                    color_data |= 128 << 8;   // G
                    color_data |= 255 << 0;   // B

                    *((int*)pBackBuffer) = RGBToInt(c.R, c.G, c.B);
                }

                _wb.AddDirtyRect(new Int32Rect(column, row, 1, 1));
            }
            finally
            {
                _wb.Unlock();
            }
        }


        public int RGBToInt(int r, int g, int b)
        {
            return (r << 0) | (g << 8) | (b << 16);
        }

        public (Point p1o, Point p2o, int quartersToRotate) PointOrientation(Point p1d, Point p2d)
        {
            (Point, Point, int) ret;
            if (p1d.X <= p2d.X)
            {
                if (p1d.Y >= p2d.Y)
                {
                    var a = (p1d.Y - p2d.Y) / (p2d.X - p1d.X);
                    if (a <= 1)
                        ret = (p1d, new Point(p2d.X, p2d.Y - 2 * (p2d.Y - p1d.Y)), 0);
                    else
                        ret = (p1d, new Point(p1d.X + p1d.Y - p2d.Y, p1d.Y + p2d.X - p1d.X), 1);
                }
                else
                {
                    var a = (p2d.Y - p1d.Y) / (p2d.X - p1d.X);
                    if (a <= 1)
                        ret = (p1d, p2d, 7);
                    else
                        ret = (p1d, new Point(p1d.X - p1d.Y + p2d.Y, p1d.Y + p2d.X - p1d.X), 6);
                }
            }
            else
            {
                var p1 = p2d;
                var p2 = p1d;
                if (p1.Y >= p2.Y)
                {
                    var a = (p1.Y - p2.Y) / (p2.X - p1.X);
                    if (a <= 1)
                        ret = (new Point(p1.X, p1.Y - 2 * (p1.Y - p2.Y)), p2, 4);
                    else
                        ret = (new Point(p2.X + p2.Y - p1.Y, p2.Y + p1.X - p2.X), p2, 5); 
                }
                else
                {
                    var a = (p2.Y - p1.Y) / (p2.X - p1.X);
                    if (a <= 1)
                        ret = (p1, p2, 3);
                    else
                        ret = (new Point(p2.X - p2.Y + p1.Y, p2.Y + p1.X - p2.X), p2, 2);
                }
            }
            return ret;
        }

        private void Canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var temp = PointOrientation(Status.CurrentPolygon.Vertices.Last(), e.GetPosition(Canvas1));
            SetPixel(temp.p2o, Colors.Black);
        }
    }

    public struct Statuses
    {
        public bool BresenhofEnabled;
        public Polygon CurrentPolygon;
        public Modes Mode;
        public Line Ray;
        public List<Ellipse> BresenhofDump;
        public int VertexInMove;
        public (int, int) EdgeInMove;
        public int PolyCount;
    }

    public enum Modes
    {
        Adding,
        AddingInProgress,
        Moving,
        MovingVertex,
        MovingEdge,
        Deletion,
        Idle
    }

    public static class PolyColors
    {
        public static Color[] colors = { Colors.Black, Colors.Blue, Colors.Green, Colors.Yellow };
        public static int count = 4;
    }
}
