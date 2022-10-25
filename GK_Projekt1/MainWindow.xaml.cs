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
            _status.CurrentLine = (new Point(-1, -1), new Point(-1, -1));
            _status.RelationCount = 1;
            _polygons = new List<Polygon>();
            Canvas1.MouseLeftButtonDown += new MouseButtonEventHandler(Canvas1_MouseLeftButtonDown);
            Canvas1.Background = Brushes.Transparent;
            SetFirstScene();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Status.Mode != Modes.AddingInProgress)
            {
                _status.Mode = Modes.Adding;
                btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
                btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnConstLength.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Status.Mode == Modes.AddingInProgress)
            {
                CancelInserting(e);
            }
            _status.Mode = Modes.Deletion;
            btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
            btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnConstLength.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Status.Mode == Modes.AddingInProgress)
                CancelInserting(e);
            _status.Mode = Modes.AddingVertex;
            btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
            btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnConstLength.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
        }


        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            if (Status.Mode == Modes.AddingInProgress)
                CancelInserting(e);
            _status.Mode = Modes.Idle;
            btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
            btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnConstLength.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
        }


        private void btnPerpendicular_Click(object sender, RoutedEventArgs e)
        {
            if (Status.Mode == Modes.AddingInProgress)
                CancelInserting(e);
            _status.Mode = Modes.AddingPerpendicularRelation;
            btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
            btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnConstLength.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
        }

        private void btnParallel_Click(object sender, RoutedEventArgs e)
        {
            if (Status.Mode == Modes.AddingInProgress)
                CancelInserting(e);
            _status.Mode = Modes.AddingParallelRelation;
            btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
            btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnConstLength.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
        }

        private void btnConstLength_Click(object sender, RoutedEventArgs e)
        {
            if (Status.Mode == Modes.AddingInProgress)
                CancelInserting(e);
            _status.Mode = Modes.SettingConstLength;
            btnConstLength.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
            btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
        }

        private void Canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(Canvas1);
            if (Status.Mode == Modes.Adding)
            {
                _status.CurrentPolygon = new Polygon(Canvas1);
                _status.CurrentPolygon.Color = PolyColors.colors[_status.PolyCount++ % PolyColors.colors.Count()];
                DrawPoint(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), Colors.Red, Status.CurrentPolygon._dotSize);
                Status.CurrentPolygon.Vertices.Add(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y));
                _status.Mode = Modes.AddingInProgress;
                btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");;
            }
            else if (Status.Mode == Modes.AddingInProgress)
            {
                if (Status.CurrentPolygon.IsInsideFirstVertex(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y)))
                {
                    _polygons.Add(Status.CurrentPolygon);
                    _polygons.Last()._finished = true;
                    _status.CurrentPolygon = null;
                    _status.Mode = Modes.Adding;
                    btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
                    RedrawCanvas(Canvas1);
                }
                else
                {
                    DrawLine(Status.CurrentPolygon.Vertices.Last(), new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), PolyColors.colors[(_status.PolyCount - 1) % PolyColors.colors.Count()], Status.CurrentPolygon._lineSize);
                    DrawPoint(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), Colors.Red, Status.CurrentPolygon._dotSize);
                    Status.CurrentPolygon.Vertices.Add(new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y));
                }
            }
            else if (Status.Mode == Modes.Deletion)
            {
                if (e.Source.GetType() == typeof(TextBlock))
                {
                    for (int i = 0; i < Polygons.Count; i++)
                        for (int j = 0; j < Polygons[i].Relations.Count; j++)
                        {
                            var temp = Polygons[i].Relations[j].FirstEdge;
                            if (Math.Sqrt(Math.Pow(point.X - (temp.Item1.X + temp.Item2.X) / 2, 2) + Math.Pow(point.Y - (temp.Item1.Y + temp.Item2.Y) / 2, 2)) < 40)
                            {
                                Polygons[i].Relations.Remove(Polygons[i].Relations[j]);
                                for (int k = 0; k < Polygons.Count; k++)
                                    for (int l = 0; l < Polygons[k].Relations.Count; l++)
                                    {
                                        if(temp == Polygons[k].Relations[l].SecondEdge)
                                            Polygons[k].Relations.Remove(Polygons[k].Relations[l]);
                                    }
                                break;
                            }
                        }
                }
                else
                {
                    for (var i = 0; i < Polygons.Count; i++)
                    {
                        var p = Polygons[i].IsInsideAnyVertex(point);
                        if (p != new Point(-1, -1) && Polygons[i].Vertices.Count > 3)
                        {
                            Polygons[i].Vertices.RemoveAt(Polygons[i].Vertices.IndexOf(p));
                            var temp = new List<Relation>(Polygons[i].Relations);
                            foreach (var r in Polygons[i].Relations)
                            {
                                if (r.FirstEdge.Item1 == p)
                                {
                                    foreach (var s in Polygons[i].Relations)
                                        if (s.SecondEdge.Item1 == r.FirstEdge.Item1 || s.SecondEdge.Item2 == r.FirstEdge.Item1)
                                            temp.Remove(s);
                                    temp.Remove(r);
                                    continue;
                                }
                                if (r.FirstEdge.Item2 == p)
                                {
                                    foreach (var s in Polygons[i].Relations)
                                        if (s.SecondEdge.Item1 == r.FirstEdge.Item2 || s.SecondEdge.Item2 == r.FirstEdge.Item2)
                                            temp.Remove(s);
                                    temp.Remove(r);
                                }
                            }
                            Polygons[i].Relations = temp;
                            break;
                        }
                        else
                        {
                            var solver = new InsideOutsideLib();
                            var poly = solver.isInside(Polygons[i], point);
                            if (poly != null)
                            {
                                Polygons.Remove(poly);
                            }
                        }
                    }
                }
                RedrawCanvas(Canvas1);
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
                    var solver = new InsideOutsideLib();
                    var poly = solver.isInside(Polygons[i], point); 
                    if( poly != null)
                    {
                        _status.Mode = Modes.Moving;
                        _status.CurrentPolygon = Polygons[i];
                        break;
                    }
                }
            }
            else if (Status.Mode == Modes.AddingVertex)
            {
                for (var i = 0; i < Polygons.Count; i++)
                {
                    var pair = Polygons[i].IsInsideAnyLine(point);
                    if (pair.Item1 != new Point(-1, -1))
                    {
                        DrawPoint(Mouse.GetPosition(Canvas1), Polygons[i]._vertexColor, Polygons[i]._dotSize);
                        Polygons[i].Vertices.Insert(Polygons[i].Vertices.IndexOf(pair.Item1) + 1, Mouse.GetPosition(Canvas1));
                        break;
                    }
                }
            }
            else if (Status.Mode == Modes.AddingPerpendicularRelation || Status.Mode == Modes.AddingParallelRelation)
            {
                for (var i = 0; i < Polygons.Count; i++)
                {
                    var pair = Polygons[i].IsInsideAnyLine(point);
                    if (pair.Item1 != new Point(-1, -1))
                    {
                        if(Status.CurrentLine == (new Point(-1, -1), new Point(-1, -1)))
                        {
                            _status.CurrentLine = pair;
                            Polygons[i].AddRelation(pair.Item1, pair.Item2, null, new Point(-1, -1), new Point(-1, -1), Status.Mode == Modes.AddingPerpendicularRelation ? RelationTypes.Perpendicular : RelationTypes.Parallel, Status.RelationCount);
                            _status.CurrentRelation = Polygons[i].Relations.Last();
                            _status.CurrentPolygon = Polygons[i];
                            _status.RelationCount++;
                            RedrawCanvas(Canvas1);
                        }
                        else
                        {
                            var temp1 = new List<(int, int, int)>();
                            var temp2 = new List<(int, int, int)>();
                            foreach (var p in Polygons)
                                foreach (var r in p.Relations)
                                {
                                    if (r.FirstEdge.Item1 == pair.Item1)
                                        temp1.Add((Polygons.IndexOf(p), p.Relations.IndexOf(r), 1));
                                    else if(r.FirstEdge.Item2 == pair.Item1)
                                        temp1.Add((Polygons.IndexOf(p), p.Relations.IndexOf(r), 2));
                                    else if(r.FirstEdge.Item1 == pair.Item2)
                                        temp2.Add((Polygons.IndexOf(p), p.Relations.IndexOf(r), 1));
                                    else if(r.FirstEdge.Item2 == pair.Item2)
                                        temp2.Add((Polygons.IndexOf(p), p.Relations.IndexOf(r), 2));
                                }
                            var a = (Status.CurrentRelation.FirstEdge.Item1.Y - Status.CurrentRelation.FirstEdge.Item2.Y) / (Status.CurrentRelation.FirstEdge.Item1.X - Status.CurrentRelation.FirstEdge.Item2.X);
                            var p1 = Polygons[i].Vertices.FindIndex((x) => x == pair.Item1);
                            var p2 = Polygons[i].Vertices.FindIndex((x) => x == pair.Item2);
                            if (Status.Mode == Modes.AddingPerpendicularRelation)
                            {
                                (Polygons[i].Vertices[p1], Polygons[i].Vertices[p2]) = RotateEdge((pair.Item1, pair.Item2), -1 / a);
                            }
                            else
                            {
                                (Polygons[i].Vertices[p1], Polygons[i].Vertices[p2]) = RotateEdge((pair.Item1, pair.Item2), a);
                            }
                            Polygons[i].AddRelation(Polygons[i].Vertices[p1], Polygons[i].Vertices[p2], Status.CurrentPolygon, Status.CurrentRelation.FirstEdge.Item1, Status.CurrentRelation.FirstEdge.Item2, Status.CurrentRelation.Type, Status.CurrentRelation.ID);
                            _status.CurrentRelation.SecondEdge = (Polygons[i].Vertices[p1], Polygons[i].Vertices[p2]);
                            _status.CurrentRelation.RelatedPolygon = Polygons[i];
                            foreach (var t in temp1)
                            {
                                if (t.Item3 == 1)
                                    Polygons[t.Item1].Relations[t.Item2].FirstEdge.Item1 = Polygons[i].Vertices[p1];
                                else if(t.Item3 == 2)
                                    Polygons[t.Item1].Relations[t.Item2].FirstEdge.Item2 = Polygons[i].Vertices[p1];
                            }
                            foreach (var t in temp2)
                            {
                                if (t.Item3 == 1)
                                    Polygons[t.Item1].Relations[t.Item2].FirstEdge.Item1 = Polygons[i].Vertices[p2];
                                else if (t.Item3 == 2)
                                    Polygons[t.Item1].Relations[t.Item2].FirstEdge.Item2 = Polygons[i].Vertices[p2];
                            }

                            _status.Mode = Modes.Idle;
                            _status.CurrentLine = (new Point(-1, -1), new Point(-1, -1));
                            _status.CurrentRelation = null;
                            _status.CurrentPolygon = null;
                            btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
                            btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                            btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                            btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                            btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                            btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                            btnConstLength.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                            RedrawCanvas(Canvas1);
                        }
                    }
                }
            }
            else if(Status.Mode == Modes.SettingConstLength)
            {
                for (var i = 0; i < Polygons.Count; i++)
                {
                    var pair = Polygons[i].IsInsideAnyLine(point);
                    if (pair.Item1 != new Point(-1, -1))
                    {
                        Polygons[i].AddRelation(pair.Item1, pair.Item2, null, new Point(-1, -1), new Point(-1, -1), RelationTypes.Const, Status.RelationCount);
                        _status.RelationCount++;
                        break;
                    }
                }
                btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
                btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnConstLength.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                _status.Mode = Modes.Idle;
                RedrawCanvas(Canvas1);
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
                    _status.Ray = DrawLine(Status.CurrentPolygon.Vertices.Last(), new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), PolyColors.colors[(_status.PolyCount - 1) % PolyColors.colors.Count()], Status.CurrentPolygon._lineSize);
                else
                    _status.BresenhofDump = DrawLineBresenhof(Status.CurrentPolygon.Vertices.Last(), new Point(e.GetPosition(Canvas1).X, e.GetPosition(Canvas1).Y), PolyColors.colors[(_status.PolyCount - 1) % PolyColors.colors.Count()]);
                Status.CurrentPolygon.Draw(Canvas1);
                foreach (var i in Canvas1.Children)
                    if (i.GetType() == typeof(Line))
                    {
                        ((Line)i).MouseLeftButtonDown += Canvas1_MouseLeftButtonDown;
                        ((Line)i).MouseRightButtonDown += Canvas1_MouseRightButtonDown;
                        ((Line)i).MouseLeftButtonUp += Canvas1_MouseLeftButtonUp;
                    }
                    else if (i.GetType() == typeof(Ellipse))
                    {
                        ((Ellipse)i).MouseLeftButtonDown += Canvas1_MouseLeftButtonDown;
                        ((Ellipse)i).MouseRightButtonDown += Canvas1_MouseRightButtonDown;
                        ((Ellipse)i).MouseLeftButtonUp += Canvas1_MouseLeftButtonUp;
                    }
                    else if (i.GetType() == typeof(TextBlock))
                    {
                        ((TextBlock)i).MouseLeftButtonDown += Canvas1_MouseLeftButtonDown;
                        ((TextBlock)i).MouseRightButtonDown += Canvas1_MouseRightButtonDown;
                        ((TextBlock)i).MouseLeftButtonUp += Canvas1_MouseLeftButtonUp;
                    }
            }
            else if(Status.Mode == Modes.MovingVertex)
            {
                var isConst = false;
                Relation rel = new Relation();
                int count = 0;
                foreach (var i in Status.CurrentPolygon.Relations)
                    if ((i.FirstEdge.Item1 == _status.CurrentPolygon.Vertices[Status.VertexInMove] || i.FirstEdge.Item2 == _status.CurrentPolygon.Vertices[Status.VertexInMove]) && i.Type == RelationTypes.Const)
                    {
                        isConst = true;
                        rel = i;
                        count++;
                    }
                if (isConst == true)
                {
                    if (count < 2)
                    {
                        var line = rel.FirstEdge.Item1 == Status.CurrentPolygon.Vertices[Status.VertexInMove] ?
                            (new Point(rel.FirstEdge.Item2.X - (rel.FirstEdge.Item1.X - rel.FirstEdge.Item2.X), rel.FirstEdge.Item2.Y - (rel.FirstEdge.Item1.Y - rel.FirstEdge.Item2.Y)), rel.FirstEdge.Item1) :
                            (new Point(rel.FirstEdge.Item1.X - (rel.FirstEdge.Item2.X - rel.FirstEdge.Item1.X), rel.FirstEdge.Item1.Y - (rel.FirstEdge.Item2.Y - rel.FirstEdge.Item1.Y)), rel.FirstEdge.Item2);
                        var ret = RotateEdge(line, (point.Y - (rel.FirstEdge.Item1.Y + rel.FirstEdge.Item2.Y) / 2) / (point.X - (rel.FirstEdge.Item1.X + rel.FirstEdge.Item2.X) / 2));
                        if (rel.FirstEdge.Item1 == Status.CurrentPolygon.Vertices[Status.VertexInMove])
                            rel.FirstEdge.Item1 = ret.Item2;
                        else
                            rel.FirstEdge.Item2 = ret.Item2;
                        _status.CurrentPolygon.Vertices[Status.VertexInMove] = ret.Item2;
                    }
                    else
                    {
                        _status.Mode = Modes.Moving;
                    }
                }
                else
                {
                    var p0 = _status.CurrentPolygon.Vertices[Status.VertexInMove];
                    _status.CurrentPolygon.Vertices[Status.VertexInMove] = point;
                    for (int i = 0; i < Status.CurrentPolygon.Relations.Count; i++)
                    {
                        if (p0 == Status.CurrentPolygon.Relations[i].FirstEdge.Item1)
                            _status.CurrentPolygon.Relations[i].FirstEdge.Item1 = point;
                        if (p0 == Status.CurrentPolygon.Relations[i].FirstEdge.Item2)
                            _status.CurrentPolygon.Relations[i].FirstEdge.Item2 = point;
                    }
                    var a = (Status.CurrentPolygon.Vertices[Status.VertexInMove].Y - Status.CurrentPolygon.Vertices[Mod(Status.VertexInMove - 1, Status.CurrentPolygon.Vertices.Count)].Y) / (Status.CurrentPolygon.Vertices[Status.VertexInMove].X - Status.CurrentPolygon.Vertices[Mod((Status.VertexInMove - 1), Status.CurrentPolygon.Vertices.Count)].X);
                    for (var i = 0; i < Status.CurrentPolygon.Relations.Count; i++)
                    {
                        if (Status.CurrentPolygon.Relations[i].FirstEdge.Item1 == p0)
                        {
                            a = (Status.CurrentPolygon.Vertices[Status.VertexInMove].Y - Status.CurrentPolygon.Vertices[Mod(Status.VertexInMove + 1, Status.CurrentPolygon.Vertices.Count)].Y) / (Status.CurrentPolygon.Vertices[Status.VertexInMove].X - Status.CurrentPolygon.Vertices[Mod((Status.VertexInMove + 1), Status.CurrentPolygon.Vertices.Count)].X);
                            Status.CurrentPolygon.Relations[i].Visited = true;
                            MoveRelations(a, Status.CurrentPolygon.Relations[i].RelatedPolygon, Status.CurrentPolygon.Relations[i].ID, Status.CurrentPolygon.Relations[i]);
                        }
                        else if(Status.CurrentPolygon.Relations[i].FirstEdge.Item2 == p0)
                        {
                            Status.CurrentPolygon.Relations[i].Visited = true;
                            MoveRelations(a, Status.CurrentPolygon.Relations[i].RelatedPolygon, Status.CurrentPolygon.Relations[i].ID, Status.CurrentPolygon.Relations[i]);
                        }
                    }
                //    a = (Status.CurrentPolygon.Vertices[Status.VertexInMove].Y - Status.CurrentPolygon.Vertices[Mod(Status.VertexInMove + 1, Status.CurrentPolygon.Vertices.Count)].Y) / (Status.CurrentPolygon.Vertices[Status.VertexInMove].X - Status.CurrentPolygon.Vertices[Mod((Status.VertexInMove + 1), Status.CurrentPolygon.Vertices.Count)].X);
                //    MoveRelations(a, _status.CurrentPolygon, p0, Status.CurrentPolygon.Vertices[Mod(Status.VertexInMove + 1, Status.CurrentPolygon.Vertices.Count)], null);
                }
                RedrawCanvas(Canvas1);
                for (var i = 0; i < Polygons.Count; i++)
                    for (var j = 0; j < Polygons[i].Relations.Count; j++)
                        Polygons[i].Relations[j].Visited = false;
            }
            else if(Status.Mode == Modes.MovingEdge)
            {
                // Keeping relations while moving edges to be implemented...
                for (int i = 0; i < Status.CurrentPolygon.Relations.Count; i++)
                {
                    if (Status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1] == Status.CurrentPolygon.Relations[i].FirstEdge.Item1)
                        _status.CurrentPolygon.Relations[i].FirstEdge.Item1 = new Point(_status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1].X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1].Y - (_lastMousePosition.Y - point.Y));
                    if (Status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1] == Status.CurrentPolygon.Relations[i].FirstEdge.Item2)
                        _status.CurrentPolygon.Relations[i].FirstEdge.Item2 = new Point(_status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1].X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1].Y - (_lastMousePosition.Y - point.Y));
                    if (Status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2] == Status.CurrentPolygon.Relations[i].FirstEdge.Item1)
                        _status.CurrentPolygon.Relations[i].FirstEdge.Item1 = new Point(_status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2].X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2].Y - (_lastMousePosition.Y - point.Y));
                    if (Status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2] == Status.CurrentPolygon.Relations[i].FirstEdge.Item2)
                        _status.CurrentPolygon.Relations[i].FirstEdge.Item2 = new Point(_status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2].X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2].Y - (_lastMousePosition.Y - point.Y));
                }
                var ret = new Point(_status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1].X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1].Y - (_lastMousePosition.Y - point.Y));
                _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item1] = ret;
                ret = new Point(_status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2].X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2].Y - (_lastMousePosition.Y - point.Y));
                _status.CurrentPolygon.Vertices[Status.EdgeInMove.Item2] = ret;
                RedrawCanvas(Canvas1);
            }
            else if(Status.Mode == Modes.Moving)
            {
                for (int i = 0; i < Status.CurrentPolygon.Relations.Count; i++)
                {
                    _status.CurrentPolygon.Relations[i].FirstEdge.Item1 = new Point(_status.CurrentPolygon.Relations[i].FirstEdge.Item1.X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Relations[i].FirstEdge.Item1.Y - (_lastMousePosition.Y - point.Y));
                    _status.CurrentPolygon.Relations[i].FirstEdge.Item2 = new Point(_status.CurrentPolygon.Relations[i].FirstEdge.Item2.X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Relations[i].FirstEdge.Item2.Y - (_lastMousePosition.Y - point.Y));
                    _status.CurrentPolygon.Relations[i].SecondEdge.Item1 = new Point(_status.CurrentPolygon.Relations[i].SecondEdge.Item1.X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Relations[i].SecondEdge.Item1.Y - (_lastMousePosition.Y - point.Y));
                    _status.CurrentPolygon.Relations[i].SecondEdge.Item2 = new Point(_status.CurrentPolygon.Relations[i].SecondEdge.Item2.X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Relations[i].SecondEdge.Item2.Y - (_lastMousePosition.Y - point.Y));

                }
                for (var i = 0; i < _status.CurrentPolygon.Vertices.Count; i++)
                {
                    var ret = new Point(_status.CurrentPolygon.Vertices[i].X - (_lastMousePosition.X - point.X), _status.CurrentPolygon.Vertices[i].Y - (_lastMousePosition.Y - point.Y));
                    _status.CurrentPolygon.Vertices[i] = ret;
                }
                RedrawCanvas(Canvas1);
            }
            _lastMousePosition = point;
            e.Handled = true;
        }

        private void Canvas1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var temp = Status.Mode;
            CancelInserting(e);
            if (temp == Modes.AddingInProgress)
            {
                _status.Mode = Modes.Adding;
                btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
                btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
            }
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
            if (Status.Mode == Modes.AddingInProgress)
                _status.PolyCount--;
            if (Status.Mode != Modes.Adding)
            {
                _status.BresenhofDump = null;
                _status.CurrentPolygon = null;
                RedrawCanvas(Canvas1);
                _status.Mode = Modes.Idle;
                btnMove.Background = (Brush)new BrushConverter().ConvertFrom("#bee6fd");
                btnNewVertex.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnNewPoly.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnDelete.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnPerpendicular.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
                btnParallel.Background = (Brush)new BrushConverter().ConvertFrom("#dddddd");
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
                else if (i.GetType() == typeof(Ellipse))
                {
                    ((Ellipse)i).MouseLeftButtonDown += Canvas1_MouseLeftButtonDown;
                    ((Ellipse)i).MouseRightButtonDown += Canvas1_MouseRightButtonDown;
                    ((Ellipse)i).MouseLeftButtonUp += Canvas1_MouseLeftButtonUp;
                }
                else if (i.GetType() == typeof(TextBlock))
                {
                    ((TextBlock)i).MouseLeftButtonDown += Canvas1_MouseLeftButtonDown;
                    ((TextBlock)i).MouseRightButtonDown += Canvas1_MouseRightButtonDown;
                    ((TextBlock)i).MouseLeftButtonUp += Canvas1_MouseLeftButtonUp;
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

        private (Point, Point) RotateEdge((Point, Point) edge, double a)
        {
            if (a == double.NaN)
                a = 50;
            var middle = new Point((edge.Item1.X + edge.Item2.X) / 2, (edge.Item1.Y + edge.Item2.Y) / 2);
            var length = Math.Sqrt(Math.Pow(edge.Item1.X - edge.Item2.X, 2) + Math.Pow(edge.Item1.Y - edge.Item2.Y, 2));
            var b = middle.Y - a * middle.X;
            var delta = Math.Pow(2 * middle.X + 2 * (middle.Y - b) * a, 2) - 4 * (1 + Math.Pow(a, 2)) * (Math.Pow(middle.X, 2) + Math.Pow(middle.Y - b, 2) - Math.Pow(length / 2, 2));
            var eqa = 1 + Math.Pow(a, 2);
            var eqb = -(2 * middle.X + 2 * (middle.Y - b) * a);
            var eqc = Math.Pow(middle.X, 2) + Math.Pow(middle.Y - b, 2) - Math.Pow(length / 2, 2);
            var x1 = (-eqb - Math.Sqrt(delta)) / (2 * eqa);
            var x2 = (-eqb + Math.Sqrt(delta)) / (2 * eqa);
            var y1 = a * x1 + b;
            var y2 = a * x2 + b;
            var dist1 = Math.Sqrt(Math.Pow(edge.Item1.X - x1, 2) + Math.Pow(edge.Item1.Y - y1, 2));
            var dist2 = Math.Sqrt(Math.Pow(edge.Item1.X - x2, 2) + Math.Pow(edge.Item1.Y - y2, 2));
            if (dist2 > dist1)
            {
                edge.Item1.X = x1;
                edge.Item1.Y = y1;
                edge.Item2.X = x2;
                edge.Item2.Y = y2;
            }
            else
            {
                edge.Item1.X = x2;
                edge.Item1.Y = y2;
                edge.Item2.X = x1;
                edge.Item2.Y = y1;
            }
            return edge;
        }

        public void MoveRelations(double a, Polygon poly, int ID, Relation prevRel)
        {
            (Point, Point) temp = default;
            (Point, Point) res = default;
            for (var i = 0; i < poly.Relations.Count; i++)
            {
                if (poly.Relations[i].ID == ID && poly.Relations[i].Visited != true)
                {
                    try
                    {
                        temp = poly.Relations[i].FirstEdge;
                        var twinRelation = poly.Relations[i];
                        var twinPoints = (poly.Vertices.FindIndex((x) => new Point(x.X, x.Y) == twinRelation.FirstEdge.Item1), poly.Vertices.FindIndex((x) => new Point(x.X, x.Y) == twinRelation.FirstEdge.Item2));
                        if (poly.Relations[i].Type == RelationTypes.Parallel)
                        {
                            (poly.Vertices[twinPoints.Item1], poly.Vertices[twinPoints.Item2]) = RotateEdge(twinRelation.FirstEdge, a);
                            twinRelation.FirstEdge = (poly.Vertices[twinPoints.Item1], poly.Vertices[twinPoints.Item2]);
                        }
                        else if (poly.Relations[i].Type == RelationTypes.Perpendicular)
                        {
                            (poly.Vertices[twinPoints.Item1], poly.Vertices[twinPoints.Item2]) = RotateEdge(twinRelation.FirstEdge, -1 / a);
                            twinRelation.FirstEdge = (poly.Vertices[twinPoints.Item1], poly.Vertices[twinPoints.Item2]);
                        }
                        res = twinRelation.FirstEdge;
                        //if (prevRel != null)
                        //    prevRel.SecondEdge = twinRelation.FirstEdge;
                        //poly.Relations[i].SecondEdge = twinRelation.FirstEdge;
                    }
                    catch (ArgumentOutOfRangeException e)
                    {

                    }
                    // Recursive Moving further edge dependancies to be added
                    poly.Relations[i].Visited = true;
                }
            }
            for (var i = 0; i < poly.Relations.Count; i++)
                if (poly.Relations[i].Visited != true && ((poly.Relations[i].FirstEdge.Item1, poly.Relations[i].FirstEdge.Item2) == temp || (poly.Relations[i].FirstEdge.Item2, poly.Relations[i].FirstEdge.Item1) == temp))
                {
                    MoveRelations(a, poly.Relations[i].RelatedPolygon, poly.Relations[i].ID, poly.Relations[i]);
                    poly.Relations[i].FirstEdge = res;
                }
        }

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

        int Mod(int x, int m)
        {
            return (x % m + m) % m;
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

        private void SetFirstScene()
        {
            var tempPoly1 = new Polygon(Canvas1);
            var tempVertexes1 = new List<Point>();
            tempVertexes1.Add(new Point(70, 100));
            tempVertexes1.Add(new Point(100, 200));
            tempVertexes1.Add(new Point(200, 200));
            tempVertexes1.Add(new Point(170, 100));
            tempPoly1.Vertices = tempVertexes1;
            tempPoly1._finished = true;
            Polygons.Add(tempPoly1);

            var tempPoly2 = new Polygon(Canvas1);
            var tempVertexes2 = new List<Point>();
            tempVertexes2.Add(new Point(350, 100));
            tempVertexes2.Add(new Point(310, 400));
            tempVertexes2.Add(new Point(450, 400));
            tempVertexes2.Add(new Point(550, 100));
            tempPoly2.Vertices = tempVertexes2;
            tempPoly2._finished = true;
            Polygons.Add(tempPoly2);

            var tempPoly3 = new Polygon(Canvas1);
            var tempVertexes3 = new List<Point>();
            tempVertexes3.Add(new Point(80, 300));
            tempVertexes3.Add(new Point(130, 500));
            tempVertexes3.Add(new Point(250, 400));
            tempPoly3.Vertices = tempVertexes3;
            tempPoly3._finished = true;
            Polygons.Add(tempPoly3);

            var tempPoly4 = new Polygon(Canvas1);
            var tempVertexes4 = new List<Point>();
            tempVertexes4.Add(new Point(800, 100));
            tempVertexes4.Add(new Point(600, 300));
            tempVertexes4.Add(new Point(700, 500));
            tempVertexes4.Add(new Point(900, 500));
            tempVertexes4.Add(new Point(1000, 300));
            tempPoly4.Vertices = tempVertexes4;
            tempPoly4._finished = true;
            Polygons.Add(tempPoly4);

            var tempRel1 = new Relation();
            tempRel1.FirstEdge = (tempPoly1.Vertices[2], tempPoly1.Vertices[3]);
            tempRel1.RelatedPolygon = tempPoly2;
            tempRel1.ID = 1;
            tempRel1.Type = RelationTypes.Parallel;
            tempPoly1.Relations.Add(tempRel1);
            var tempRel2 = new Relation();
            tempRel2.FirstEdge = (tempPoly2.Vertices[0], tempPoly2.Vertices[1]);
            tempRel2.RelatedPolygon = tempPoly1;
            tempRel2.ID = 1;
            tempRel2.Type = RelationTypes.Parallel;
            tempPoly2.Relations.Add(tempRel2);

            var tempRel3 = new Relation();
            tempRel3.FirstEdge = (tempPoly3.Vertices[0], tempPoly3.Vertices[1]);
            tempRel3.RelatedPolygon = null;
            tempRel3.ID = 2;
            tempRel3.Type = RelationTypes.Const;
            tempPoly3.Relations.Add(tempRel3);

            var tempRel4 = new Relation();
            tempRel4.FirstEdge = (tempPoly3.Vertices[1], tempPoly3.Vertices[2]);
            tempRel4.RelatedPolygon = null;
            tempRel4.ID = 3;
            tempRel4.Type = RelationTypes.Const;
            tempPoly3.Relations.Add(tempRel4);

            var tempRel5 = new Relation();
            tempRel5.FirstEdge = (tempPoly2.Vertices[2], tempPoly2.Vertices[3]);
            tempRel5.RelatedPolygon = tempPoly4;
            tempRel5.ID = 2;
            tempRel5.Type = RelationTypes.Perpendicular;
            tempPoly2.Relations.Add(tempRel5);
            var tempRel6 = new Relation();
            tempRel6.FirstEdge = (tempPoly4.Vertices[2], tempPoly4.Vertices[3]);
            tempRel6.RelatedPolygon = tempPoly2;
            tempRel6.ID = 2;
            tempRel6.Type = RelationTypes.Perpendicular;
            tempPoly4.Relations.Add(tempRel6);

            _status.RelationCount = 3;
            RedrawCanvas(Canvas1);
        }

        private void Canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }
    }

    public struct Statuses
    {
        public bool BresenhofEnabled;
        public Polygon CurrentPolygon;
        public (Point, Point) CurrentLine;
        public Modes Mode;
        public Line Ray;
        public List<Ellipse> BresenhofDump;
        public int VertexInMove;
        public (int, int) EdgeInMove;
        public int PolyCount;
        public int RelationCount;
        public Relation CurrentRelation;
    }

    public enum Modes
    {
        Adding,
        AddingInProgress,
        AddingVertex,
        Moving,
        MovingVertex,
        MovingEdge,
        Deletion,
        AddingPerpendicularRelation,
        AddingParallelRelation,
        SettingConstLength,
        Idle
    }

    public static class PolyColors
    {
        public static Color[] colors = { Colors.Black, Colors.Blue, Colors.Green, Colors.Orange, Colors.Brown, Colors.Violet, Colors.Pink };
    }
}
