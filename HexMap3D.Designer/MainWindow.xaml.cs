using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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

namespace HexMap3D.Designer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly Canvas _canvas;

        private readonly HexMap _hexMap;

        private Hex _selectedHex = null;

        #region Modes

        private bool _addHexOnClick = true;

        #endregion
        
        public MainWindow() {
            _hexMap = new HexMap(Orientation.FlatTop, 100);
            InitializeComponent();

            _canvas = (Canvas)this.FindName("canvas");
        }

        private void cnv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {

            if (_addHexOnClick) {
                //Add a new hex under the mouse if one does not already exist - if one exists do nothing
                var cubicCoords = HexUtils.CartesianToCubic(Utils.PointToPoint(e.GetPosition(_canvas)), _hexMap.Orientation, _hexMap.Width);
                try {
                    _hexMap.AddHex(cubicCoords);
                }
                catch (ArgumentException) {
                    //This means a hex already exists at that location
                    Console.WriteLine($"Not adding hex at {cubicCoords} as it already exists");
                }

                Console.WriteLine($"Adding hex at {cubicCoords}");
            }
            else {
                //Select the hex under the mouse if it exists
            }
            
            RedrawHexes();
            
            //DEBUG - draw circle where clicked
            var circleCenter = e.GetPosition(_canvas);
            DrawCircle(circleCenter.X, circleCenter.Y);
        }

        #region Click and Drag

        private Point _offset = new Point(0,0);
        private Point _lastMousePosition;
        private bool _mouseDown = false;
        
        /// <summary>
        /// Right mouse button is clicked - start click and drag.
        /// </summary>
        private void cnv_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            _lastMousePosition = e.GetPosition((Canvas)e.Source);
            _mouseDown = true;
        }
        
        /// <summary>
        /// Right mouse button is lifted - stop click and drag
        /// </summary>
        private void cnv_MouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            _mouseDown = false;
        }

        /// <summary>
        /// Mouse has moved.
        /// If right mouse button is down, then move the offset and redraw canvas
        /// </summary>
        private void cnv_MouseMove(object sender, MouseEventArgs e) {
            if (!_mouseDown) return;
            
            var clicked = (UIElement) e.Source;

            _offset = Point.Add(_offset, _lastMousePosition - e.GetPosition(clicked));
            
            //TODO is there a more efficent way of doing this 
            foreach (UIElement canvasChild in _canvas.Children) {
                Canvas.SetLeft(canvasChild, _offset.X);
                Canvas.SetTop(canvasChild, _offset.Y);
            }
            _canvas.InvalidateVisual(); 
            
            Console.WriteLine($"Offset is {_offset}");
                
//            foreach (UIElement child in _canvas.Children) {
//                child.UpdateLayout();
//            }
        }
        
        #endregion
        
        /// <summary>
        /// Redraws the canvas
        /// TODO: in the future should use canvas.Invalidate() which will redraw everything
        /// </summary>
        private void RedrawHexes() {
            _canvas.Children.Clear();

            foreach (var hex in _hexMap.Hexes.Values) {
                var pointList = new List<Point>();
                var center = hex.CartesianCoordinate;

                if (_hexMap.Orientation == Orientation.FlatTop) {
                    double w = hex.Width; //Width of hex from center
                    double h = Math.Sqrt(3) * hex.Width / 2f; //Height of hex from center
                    
                    pointList.Add(new Point(center.X - w, center.Y));
                    pointList.Add(new Point(center.X - w/2f, center.Y - h));
                    pointList.Add(new Point(center.X + w/2f, center.Y - h));
                    pointList.Add(new Point(center.X + w, center.Y));
                    pointList.Add(new Point(center.X + w/2f, center.Y + h));
                    pointList.Add(new Point(center.X - w/2f, center.Y + h));
                }
                else {
                    double h = hex.Width; //Width of hex from center
                    double w = Math.Sqrt(3) * hex.Width / 2f; //Height of hex from center
                    
                    //Side points
                    //Top points
                    //Bottom points
                    
                    pointList.Add(new Point(center.X, center.Y - h));
                    pointList.Add(new Point(center.X + w, center.Y - h/2f));
                    pointList.Add(new Point(center.X + w, center.Y + h/2f));
                    pointList.Add(new Point(center.X, center.Y + h));
                    pointList.Add(new Point(center.X - w, center.Y + h/2f));
                    pointList.Add(new Point(center.X - w, center.Y - h/2f));
                }

                _canvas.Children.Add(new Polygon() {
                    Points = new PointCollection(pointList),
                    Stroke = new SolidColorBrush() {Color = Colors.Black},
                    StrokeThickness = 2,
                });

//                _canvas.Children.Add(new Ellipse() {
//                    
//                });
            }
        }
        
        
        private void DrawCircle(double x, double y, double width=1, double height=1)
        {
            Console.WriteLine($"Drawing circle at {x}, {y}");
//            Ellipse circle = new Ellipse()
//            {
//                Width = width,
//                Height = height,
//                Stroke = Brushes.Red,
//                StrokeThickness = 1
//            };
//
//            _canvas.Children.Add(circle);
//
//            circle.SetValue(Canvas.LeftProperty, x);
//            circle.SetValue(Canvas.TopProperty, y);
        }
    }
}