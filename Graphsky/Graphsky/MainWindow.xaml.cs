using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
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
            string path;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true) {
                path = dialog.FileName;

                if (JSONHandler.loadFromFile(path, ref graph)) {
                    // Show message box that file was loaded correctly!
                    MessageBox.Show(
                        $"File {path} loaded!",
                        "Loading successfull",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    // Enable button only if loading was successfull!
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

                // Disable all other buttons on failure
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
            if (graph.calculateUniformCoordinates()) {
                // Show message box that uniform coordinates where calculated!
                MessageBox.Show(
                    "Uniform coordinates where calculated!",
                    "Uniform coordinates",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Paint graph to canvas -> add to another function!
                int width = (int) cvsWhiteboard.ActualWidth;
                int height = (int)cvsWhiteboard.ActualHeight;

                int gwidth, gheight;
                graph.getExtent().Unpack(out gwidth, out gheight);

                int steps_width = width / (gwidth + 2);
                int steps_height = height / (gheight + 2);

                foreach (Node n in graph.nodes) {
                    Ellipse el = new Ellipse();
                    el.Stroke = System.Windows.Media.Brushes.Black;
                    el.Fill = System.Windows.Media.Brushes.Black;
                    el.Width = height / 10;
                    el.Height = height / 10;
                    cvsWhiteboard.Children.Add(el);
                    Canvas.SetLeft(el, n.getPosition().Item1 * steps_width);
                    Canvas.SetTop(el, height / 2 + n.getPosition().Item2 * steps_height);
                }

                // Enable button only if calculation was successfull!
                btnSaveGraph.IsEnabled = true;
                return;
            }

            // Show message box that uniform coordinates calculation failed!
            MessageBox.Show(
                "Uniform coordinates could not be calculated!",
                "Uniform coordinates",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.OK,
                MessageBoxOptions.ServiceNotification
            );
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
