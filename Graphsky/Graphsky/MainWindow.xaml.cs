using System.Windows;
using Microsoft.Win32;


namespace Graphsky {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        /**
         *  Button handler for loading a graph from a file
         *  
         *  @paran sender       ..
         *  @param e            ..
         */
        private void loadGraphFromFile(object sender, RoutedEventArgs e) {
            //
            string path;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true) {
                path = dialog.FileName;
            }

            btnCalcGraph.IsEnabled = true;
        }

        /**
         *  Button handler for calculating a appropriate layout for loaded graph
         *  
         *  @paran sender       ..
         *  @param e            ..
         */
        private void calculateGraphLayout(object sender, RoutedEventArgs e) {
            //
            btnSaveGraph.IsEnabled = true;
        }

        /**
         *  Button handler for saving the current graph to a file
         *  
         *  @paran sender       ..
         *  @param e            ..
         */
        private void saveGraphToFile(object sender, RoutedEventArgs e) {
            //
        }
    }
}
