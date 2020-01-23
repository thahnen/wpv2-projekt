using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
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
                    // Clear canvas
                    cvsWhiteboard.Children.Clear();

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

                Tuple<int, int> g_size = graph.getExtent();
                Tuple<int, int> c_size = new Tuple<int, int>(
                    (int) cvsWhiteboard.ActualWidth,
                    (int) cvsWhiteboard.ActualHeight
                );

                // Clear canvas
                cvsWhiteboard.Children.Clear();
                // Draw nodes
                drawNodes(graph.nodes, ref g_size, ref c_size);
                // Draw edges (including arrow to first, from last to end)
                drawEdges(graph.nodes, graph.edges, ref g_size, ref c_size);

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


        /**
         * 
         */
        private void onWindowResize(object sender, RoutedEventArgs e) {
            //
        }


        /**
         *  Draws all nodes using the graph width + height to calculate the exact position
         *  
         *  @param nodes        list of graph nodes
         *  @param g_size       size of the graph (in nodes)
         *  @param c_size       size of the canvas (in pixel)
         */
        private void drawNodes(Node[] nodes, ref Tuple<int, int> g_size, ref Tuple<int, int> c_size) {
            int step_x = c_size.Item1 / g_size.Item1;
            int step_y = c_size.Item2 / (g_size.Item2 + 2);

            foreach (Node n in nodes) {
                int x, y;
                n.getPosition().Unpack(out x, out y);

                Ellipse e = new Ellipse {
                    Stroke = System.Windows.Media.Brushes.Black,
                    Fill = System.Windows.Media.Brushes.Black,
                    Width = 10,
                    Height = 10
                };

                int offset_width = (int) (e.Width * 0.5);
                int offset_height = (int) (e.Height * 0.5);

                cvsWhiteboard.Children.Add(e);
                Canvas.SetLeft(e, (x-0.5) * step_x - offset_width);
                Canvas.SetTop(e, c_size.Item2 / 2 + y * step_y - offset_height);
            }
        }


        /**
         *  Draws all edges from given adjacency matrix
         *  
         *  @param nodes        list of nodes, necessary for edge coordinates
         *  @param adjacency    the adjacency matrix for the edges
         *  @param g_size       size of the graph (in nodes)
         *  @param c_size       size of the canvas (in pixel)
         */
        private void drawEdges(Node[] nodes, bool[,] adjacency, ref Tuple<int, int> g_size,
                                ref Tuple<int, int> c_size) {
            int step_x = c_size.Item1 / g_size.Item1;
            int step_y = c_size.Item2 / (g_size.Item2 + 2);

            for (int i = 0; i < adjacency.GetLength(0); i++) {
                int x1, y1;
                nodes[i].getPosition().Unpack(out x1, out y1);

                for (int j = 0; j < adjacency.GetLength(1); j++) {
                    if (adjacency[i, j]) {
                        int x2, y2;
                        nodes[j].getPosition().Unpack(out x2, out y2);

                        Line l = new Line {
                            Stroke = System.Windows.Media.Brushes.Black,
                            StrokeThickness = 2,
                            X1 = (x1-0.5) * step_x,
                            Y1 = y1 * step_y + c_size.Item2 / 2,
                            X2 = (x2-0.5) * step_x,
                            Y2 = y2 * step_y + c_size.Item2 / 2
                        };

                        cvsWhiteboard.Children.Add(l);
                    }
                }
            }
        }
    }
}
