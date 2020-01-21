using System.Windows;
using Microsoft.Win32;


namespace Graphsky {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Graph graph;
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

                if (JSONHandler.loadFromFile(path, ref graph)) {
                    // Show message box that file was loaded correctly!
                    MessageBox.Show(
                        $"File {path} loaded!",
                        "File loaded",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    // Only enable button if loading was successfull!
                    btnCalcGraph.IsEnabled = true;
                    return;
                }

                // Show message box that loading file failed!
                MessageBox.Show(
                    $"File {path} could not be loaded!",
                    "Loading failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK,
                    MessageBoxOptions.ServiceNotification
                );

                // Disable all other buttons if failed
                btnCalcGraph.IsEnabled = false;
                btnSaveGraph.IsEnabled = false;
            }
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
