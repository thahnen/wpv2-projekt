using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using Microsoft.Win32;


namespace Graphsky {
    /// Interaktionslogik für MainWindow.xaml
    public partial class MainWindow : Window {
        string path;            // path to loaded file
        private Graph graph;    // graph created from loaded file
        int? step_x, step_y;    // step size of points, calculated using graph/ canvas size
        bool calculated;        // indicates that step size was calculated (otherwise problems with recalculating)


        public MainWindow() {
            InitializeComponent();

            // Set default values
            graph = null;
            calculated = false;
        }


        #region Button event handler
        /**
         *  Button handler for loading a graph from a file
         */
        private void loadGraphFromFile(object sender, RoutedEventArgs e) {
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
                    // Disable save button
                    btnSaveGraph.IsEnabled = false;

                    // Set uniform coordinates calculated to false
                    calculated = false;

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
         */
        private void calculateGraphLayout(object sender, RoutedEventArgs e) {
            // Check if already calculated
            if (calculated) {
                return;
            }

            if (graph.calculateUniformCoordinates()) {
                // Show message box that uniform coordinates where calculated!
                MessageBox.Show(
                    "Uniform coordinates where calculated!",
                    "Uniform coordinates",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Calculate step size based on given graph/ canvas size
                calculateStepSize(out this.step_x, out this.step_y);

                // Clear canvas
                cvsWhiteboard.Children.Clear();
                // Draw nodes
                drawNodes(graph.nodes);
                // Draw edges (including arrow to first, from last to end)
                drawEdges(graph.nodes, graph.edges);

                // Enable button only if calculation was successfull!
                btnSaveGraph.IsEnabled = true;

                // Set uniform coordinates calculated to true
                calculated = true;

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

            // Disable all other buttons on failure
            btnSaveGraph.IsEnabled = false;
        }


        /**
         *  Button handler for saving the current graph to a file
         */
        private void saveGraphToFile(object sender, RoutedEventArgs e) {
            string[] parts = path.Split('.');
            string output_path = parts[0];

            for (int i = 1; i < parts.Length; i++) {
                if (i == parts.Length-1) {
                    output_path += ".output";
                }

                output_path += "." + parts[i];
            }

            if (JSONHandler.saveGraphToFile(output_path, ref this.graph)) {
                // Show message box that file was saved correctly!
                MessageBox.Show(
                    $"File {path} saved to {output_path}!",
                    "Saving successfull",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                return;
            }

            // Show message box that saving file failed!
            MessageBox.Show(
                $"File {path} could not be saved to {output_path}!",
                "Saving failed",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.OK,
                MessageBoxOptions.ServiceNotification
            );
        }
        #endregion


        #region Window event handler
        /**
         *  Handler for when window is resized, redraws the graph
         */
        private void onWindowResize(object sender, RoutedEventArgs e) {
            // Only run if graph given and uniform coordinates already calculated
            if (graph == null || !calculated) {
                return;
            }

            // Recalculate node positioning
            calculateStepSize(out step_x, out step_y);

            // Clear canvas
            cvsWhiteboard.Children.Clear();
            // Draw nodes
            drawNodes(graph.nodes);
            // Draw edges (including arrow to first, from last to end)
            drawEdges(graph.nodes, graph.edges);
        }
        #endregion


        /**
         *  Calculates step size using graph and canvas size
         *  
         *  @param step_x       where to store 
         */
        private void calculateStepSize(out int? step_x, out int? step_y) {
            int graph_width, graph_height;
            graph.getExtent().Unpack(out graph_width, out graph_height);

            step_x = (int)cvsWhiteboard.ActualWidth / graph_width;
            step_y = (int)cvsWhiteboard.ActualHeight / (graph_height + 2);
        }


        #region Drawing functions
        /**
         *  Draws all nodes using the graph width + height to calculate the exact position
         *  
         *  @param nodes        list of graph nodes
         *  @param size         element size, defaults to 20
         */
        private void drawNodes(Node[] nodes, int size = 20) {
            int cvs_height = (int)cvsWhiteboard.ActualHeight;

            foreach (Node n in nodes) {
                int x, y;
                n.getPosition().Unpack(out x, out y);

                Rectangle e = new Rectangle {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Black,
                    Width = size,
                    Height = size
                };

                int offset_width = (int)(e.Width * 0.5);
                int offset_height = (int)(e.Height * 0.5);

                cvsWhiteboard.Children.Add(e);
                Canvas.SetLeft(e, (int)step_x / 2       // distance from left, equals y axis
                                    + x * (int)step_x   // distance from y axis
                                    - offset_width);    // offset due to ellipse offsets x coordinate by width

                Canvas.SetTop(e, cvs_height / 2         // distance from top, equals x axis
                                    + y * (int)step_y   // distance from x axis
                                    - offset_height);   // offset due to ellipse offsets y coordinate by height


                TextBlock t = new TextBlock {
                    FontSize = size - 2,
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.White,
                    Background = Brushes.Black,
                    Text = n.id.ToString(),
                    Width = size,
                    Height = size
                };

                offset_width = (int)(t.Width * 0.5);
                offset_height = (int)(t.Height * 0.5);

                cvsWhiteboard.Children.Add(t);
                Canvas.SetLeft(t, (int)step_x / 2
                                    + x * (int)step_x
                                    - offset_width);

                Canvas.SetTop(t, cvs_height / 2
                                    + y * (int)step_y
                                    - offset_height);
            }
        }


        /**
         *  Draws all edges from given adjacency matrix
         *  
         *  @param nodes        list of nodes, necessary for edge coordinates
         *  @param adjacency    the adjacency matrix for the edges
         *  @param size         element size, defaults to 20
         */
        private void drawEdges(Node[] nodes, bool[,] adjacency, int size = 20) {
            int cvs_height = (int)cvsWhiteboard.ActualHeight;

            for (int i = 0; i < adjacency.GetLength(0); i++) {
                int x1, y1;
                nodes[i].getPosition().Unpack(out x1, out y1);

                for (int j = 0; j < adjacency.GetLength(1); j++) {
                    if (adjacency[i, j]) {
                        int x2, y2;
                        nodes[j].getPosition().Unpack(out x2, out y2);

                        Line l = new Line {
                            Stroke = Brushes.Black,
                            StrokeThickness = 2,
                            X1 = (int)step_x / 2    // distance from left, equals y axis
                                + x1 * (int)step_x  // distance from y axis
                                + size / 2,         // equals node based offset
                            Y1 = cvs_height / 2     // distance from top, equals x axis
                                + y1 * (int)step_y, // distance from x axis

                            X2 =(int) step_x / 2    // distance from left, equals y axis
                                + x2 * (int)step_x  // distance from y axis
                                - size / 2,         // equals node based offset
                            Y2 = cvs_height / 2     // distance from top, equals x axis
                                + y2 * (int)step_y  // distance from x axis
                        };

                        cvsWhiteboard.Children.Add(l);
                    }
                }
            }

            // Draw start (to first node)
            int x, y;
            graph.first.getPosition().Unpack(out x, out y);

            cvsWhiteboard.Children.Add(new Line {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                X1 = (int)step_x / 4,
                Y1 = cvs_height / 2,
                X2 = (int)step_x / 2
                        + x * (int)step_x
                        - size / 2,
                Y2 = cvs_height / 2
            });

            // Draw ending (from last node)
            graph.last.getPosition().Unpack(out x, out y);
            cvsWhiteboard.Children.Add(new Line {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                X1 = (int)step_x / 2
                        + x * (int)step_x
                        + size / 2,
                Y1 = cvs_height / 2,
                X2 = 3 * (int)step_x / 4
                        + x * (int)step_x,
                Y2 = cvs_height / 2
            });
        }
        #endregion
    }
}
