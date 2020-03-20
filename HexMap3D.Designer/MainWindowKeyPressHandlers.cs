using System;
using System.Windows.Input;

namespace HexMap3D.Designer {
    public partial class MainWindow {
        private void cnv_KeyDown(object sender, KeyEventArgs e) {
            Console.WriteLine($"{e.Key} pressed");
            switch (e.Key) {
                case Key.Delete:
                    Delete_KeyPressed();
                    break;
            }
        }

        /// <summary>
        /// Delete key was pressed, delete all the currently selected hexes
        /// </summary>
        private void Delete_KeyPressed() {
            foreach (var hex in _selectedHexes) {
                _hexMap.Hexes.Remove(hex.Coordinate);
            }
            
            _selectedHexes.Clear();
            
            RedrawHexes();
        }
    }
}