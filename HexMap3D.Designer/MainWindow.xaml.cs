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

        private Canvas _canvas;

        private HexMap _hexMap;
        
        public MainWindow(HexMap hexMap) {
            _hexMap = hexMap;
            InitializeComponent();

            _canvas = (Canvas)this.FindName("canvas");
        }

        private void cnv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DrawHexagon((Canvas) e.Source, e.GetPosition((Canvas)e.Source));
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
            Console.WriteLine($"Offset is {_offset}");
                
            foreach (UIElement child in _canvas.Children) {
                child.UpdateLayout();
            }
        }
        
        #endregion
        
        private void DrawHexagon(Canvas canvas, Point centre) {
            
            Console.Out.WriteLine($"Clicked on {centre}");

            var points = new PointCollection(new List<Point> {
                new Point(-50 + centre.X + _offset.X, -50 + centre.Y + _offset.Y),
                new Point(+50 + centre.X + _offset.X, -50 + centre.Y + _offset.Y),
                new Point(-50 + centre.X + _offset.X, +50 + centre.Y + _offset.Y),
                new Point(+50 + centre.X + _offset.X, +50 + centre.Y + _offset.Y)
            });
            
            var hex = new Polygon() {
                Points = points,
                Stroke = new SolidColorBrush() {Color = Colors.Black},
                StrokeThickness = 2,
            };

            canvas.Children.Add(hex);
        }
    }
}