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

        private List<Hex> _selectedHexes = new List<Hex>();

        #region Radio Buttons - Mode

        private readonly RadioButton _addButton;
        private readonly RadioButton _selectButton;
        private readonly RadioButton _deleteButton;

        #endregion

        public MainWindow() {
            _hexMap = new HexMap(Orientation.PointyTop, 100);
            InitializeComponent();

            _canvas = (Canvas)this.FindName("canvas");

            _addButton = (RadioButton) this.FindName("addButton");
            _selectButton = (RadioButton) this.FindName("selectButton");
            _deleteButton = (RadioButton) this.FindName("deleteButton");
        }

        /// <summary>
        /// Mouse has been clicked on the canvas. Depending on the radio button that is active either
        ///     - Add a new hex
        ///      - Select (or unselect a hex). If shift is held select multiple
        ///     - Delete a hex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cnv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            //TODO add some sort of undo feature here - deleting hexes and not being able to recover them may be very bad down the line
            
            //Unselect the hex before adding or deleting
            if (!_selectButton.IsChecked ?? true) {
                _selectedHexes = new List<Hex>();
            }
            
            // Determine which point was clicked after calculating offset
            var clickedPoint = new Point(e.GetPosition(_canvas).X - +_offset.X, e.GetPosition(_canvas).Y - _offset.Y);
            
            if (_addButton.IsChecked ?? false) {
                //Add a new hex under the mouse if one does not already exist - if one exists do nothing
                var cubicCoords = HexUtils.CartesianToCubic(Utils.PointToPoint(clickedPoint), _hexMap.Orientation, _hexMap.Width);
                try {
                    _hexMap.AddHex(cubicCoords);
                }
                catch (ArgumentException) {
                    //This means a hex already exists at that location
                    Console.WriteLine($"Not adding hex at {cubicCoords} as it already exists");
                }

                Console.WriteLine($"Adding hex at {cubicCoords}");
            }
            else if (_selectButton.IsChecked ?? false) {
                //Select the hex under the mouse if it exists
                var key = HexUtils.CartesianToCubic(Utils.PointToPoint(clickedPoint), _hexMap.Orientation, _hexMap.Width);

                if (_hexMap.Hexes.ContainsKey(key)) {
                        var clickedHex = _selectedHexes.FirstOrDefault(hex => hex.Coordinate.Equals(key));
                        if (clickedHex != null) {
                            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) {
                                //CLicked on a hex that is already selected - unselect it
                                _selectedHexes.Remove(clickedHex);
                            }
                            else {
                                if (_selectedHexes.Count > 1) {
                                    //Multiple hexes are selected so deselect all of them except for the one clicked
                                    _selectedHexes = new List<Hex>() {_hexMap.Hexes[key]};
                                }
                                else {
                                    _selectedHexes.Clear();
                                }
                            }
                        }
                        else {
                            //Check if selecting multiple hexes
                            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) {
                                _selectedHexes.Add(_hexMap.Hexes[key]);
                            }
                            else {
                                _selectedHexes = new List<Hex>() {_hexMap.Hexes[key]};
                            }
                        }
                    
                        RedrawHexes();
                }
            }
            else if (_deleteButton.IsChecked ?? false) {
                //Delete the hex that was clicked
                var key = HexUtils.CartesianToCubic(Utils.PointToPoint(clickedPoint), _hexMap.Orientation, _hexMap.Width);

                if (_hexMap.Hexes.ContainsKey(key)) {
                    _hexMap.Hexes.Remove(key);
                    RedrawHexes();
                }
            }
            else {
                Console.WriteLine("Error - no radio buttons are selected?? or all of them are null somehow");
            }
            
            RedrawHexes();
        }

        #region Click and Drag
        //TODO if you right click and drag on a hex it gets the coord of the get and not the canvas? Either way weird things happen

        private Point _offset = new Point(0,0);
        private Point _lastMousePosition;
        private bool _mouseDown = false;
        
        /// <summary>
        /// Right mouse button is clicked - start click and drag.
        /// </summary>
        private void cnv_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            _lastMousePosition = e.GetPosition(_canvas);
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

            _offset = Point.Subtract(_offset, _lastMousePosition - e.GetPosition(clicked));
            _lastMousePosition = e.GetPosition(_canvas);
            
            RedrawHexes();
            
//            Console.WriteLine($"Offset is {_offset}");
        }
        
        #endregion
        
        /// <summary>
        /// Redraws the canvas
        /// TODO: in the future should use canvas.Invalidate() which will redraw everything
        /// </summary>
        private void RedrawHexes() {
            _canvas.Children.Clear();

            //Draw normal hexes
            foreach (var hex in _hexMap.Hexes.Values) {
                List<Point> pointList = GetPointListForHex(hex);

                _canvas.Children.Add(new Polygon() {
                    Points = new PointCollection(pointList),
                    Stroke = new SolidColorBrush() {Color = Colors.Black},
                    Fill = new SolidColorBrush() {Color = Colors.Azure},
                    StrokeThickness = 2,
                });
            }
            
            //Draw selected hex
            if (_selectedHexes != null) {
                foreach (var hex in _selectedHexes) {
                    var selectedPoints = GetPointListForHex(hex);

                    _canvas.Children.Add(new Polygon() {
                        Points = new PointCollection(selectedPoints),
                        StrokeThickness = 2,
                        Stroke = new SolidColorBrush() {Color = Colors.OrangeRed},
                        Fill = new SolidColorBrush() {Color = Colors.LightPink}
                    });
                }
            }

            //TODO is there a more efficent way of doing this 
            foreach (UIElement canvasChild in _canvas.Children) {
                Canvas.SetLeft(canvasChild, _offset.X);
                Canvas.SetTop(canvasChild, _offset.Y);
            }
        }

        /// <summary>
        /// Converts a hex into a list of points so that the hex can be rendered
        /// </summary>
        /// 
        /// <param name="hex">The hex whose points are being calculated</param>
        /// 
        /// <returns>A list of points forming a polygon. The points are in order</returns>
        private List<Point> GetPointListForHex(Hex hex) {
            var pointList = new List<Point>();
            var center = hex.CartesianCoordinate;

            if (_hexMap.Orientation == Orientation.FlatTop) {
                double w = hex.Width; //Width of hex from center
                double h = Math.Sqrt(3) * hex.Width / 2f; //Height of hex from center

                pointList.Add(new Point(center.X - w, center.Y));
                pointList.Add(new Point(center.X - w / 2f, center.Y - h));
                pointList.Add(new Point(center.X + w / 2f, center.Y - h));
                pointList.Add(new Point(center.X + w, center.Y));
                pointList.Add(new Point(center.X + w / 2f, center.Y + h));
                pointList.Add(new Point(center.X - w / 2f, center.Y + h));
            }
            else {
                double h = hex.Width; //Width of hex from center
                double w = Math.Sqrt(3) * hex.Width / 2f; //Height of hex from center

                pointList.Add(new Point(center.X, center.Y - h));
                pointList.Add(new Point(center.X + w, center.Y - h / 2f));
                pointList.Add(new Point(center.X + w, center.Y + h / 2f));
                pointList.Add(new Point(center.X, center.Y + h));
                pointList.Add(new Point(center.X - w, center.Y + h / 2f));
                pointList.Add(new Point(center.X - w, center.Y - h / 2f));
            }

            return pointList;
        }
    }
}