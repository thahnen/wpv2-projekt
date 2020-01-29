using System;
using System.Collections.Generic;


namespace Graphsky {
    /// Output structure for Graph (resembles input)
    public struct OGraph {
        public int width;
        public int height;

        public object[] nodes;
        public bool[,] adjacency;

        public OGraph(ref Graph given) {
            given.getExtent().Unpack(out this.width, out this.height);

            this.nodes = new object[given.nodes.Length];
            for (int i = 0; i < this.nodes.Length; i++) {
                int ux, uy;
                given.nodes[i].getPosition().Unpack(out ux, out uy);

                this.nodes[i] = new {
                    id = given.nodes[i].id,
                    coords = new {
                        ux = ux,
                        uy = uy
                    }
                };
            }

            this.adjacency = given.edges;
        }
    }


    /// Stores information about a given graph
    public class Graph {
        // List of all graph nodes, sorted by Id
        public Node[] nodes { get; private set; }
        public Node first { get; private set; }
        public Node last { get; private set; }

        // First index equals node where arrow points from
        // Second index equals node where arrow points to
        public readonly bool[,] edges;

        private int? width;
        private int? height;


        public Graph(Node[] Nodes, int[][] Edges) {
            nodes = Nodes;
            Array.Sort(nodes, delegate (Node a, Node b) {
                return a.id.CompareTo(b.id);
            });

            edges = new bool[nodes.Length, nodes.Length];

            foreach (int[] pair in Edges) {
                int idx_from = Array.FindIndex(nodes, delegate (Node a) {
                    return a.id == pair[0];
                });

                int idx_to = Array.FindIndex(nodes, delegate (Node a) {
                    return a.id == pair[1];
                });

                edges[idx_from, idx_to] = true;
            }
        }


        /**
         *  Returns the extent of the graph
         *  - width -> number of horizontal nodes
         *  - height -> number of vertical nodes
         *  
         *  @return             tuple containing width and height in that order
         */
        public Tuple<int, int> getExtent() {
            return new Tuple<int, int>((int)width, (int)height);
        }



        /**
         *  Calculates uniforma coordinates for the given nodes
         *  
         *  @return             true, if there was no problem with the given nodes
         */
        public bool calculateUniformCoordinates() {
            // find entry node (the one nobody points to)
            int? idx_first = getFirstEmptyColumn();
            if (!idx_first.HasValue) {
                return false;
            }

            first = nodes[(int)idx_first];

            // find ending node (the one pointing to nobody)
            int? idx_last = getFirstEmptyRow();
            if (!idx_last.HasValue) {
                return false;
            }

            last = nodes[(int)idx_last];

            int idx = 0;

            // Get slices of graph
            List<SortedSet<int>> slices = new List<SortedSet<int>>();
            slices.Add(new SortedSet<int>(new int[] { (int)idx_first }));

            do {
                SortedSet<int> following = getFollowingNodes(slices[idx]);
                if (following.Count == 0) {
                    break;
                }

                slices.Add(following);
                idx++;
            } while (true);

            // Remove nodes in multiple slices (only keeping the last occuring)
            do {
                for (int i = idx-1; i > 0; i--) {
                    slices[i].ExceptWith(slices[idx]);
                }

                idx--;
            } while (idx > 0);

            this.width = slices.Count;
            this.height = getMaxParallelNodes(slices);

            for (idx = 0; idx < slices.Count; idx++) {
                // TODO: take a look at straight / odd numbers
                int y, step_size;
                if (slices[idx].Count % 2 == 0) {
                    y = (int) -(slices[idx].Count - 1);
                    step_size = 2;
                } else {
                    y = (int) -Math.Floor((double)slices[idx].Count / 2);
                    step_size = 1;
                }

                foreach (int node in slices[idx]) {
                    nodes[node].setPosition(idx, y);
                    y += step_size;
                }
            }

            return true;
        }


        /**
         *  Get the index of the first empty column ~ the index nobody points to
         *  
         *  @return             null if no or more than row column is empty, an index otherwise
         */
        private int? getFirstEmptyColumn() {
            int? idx = null;
            int len = edges.GetLength(0);

            for (int to = 0; to < len; to++) {
                bool empty = true;

                for (int from = 0; from < len; from++) {
                    empty &= !edges[from, to];
                }

                if (empty) {
                    if (idx.HasValue) {
                        return null;
                    }

                    idx = to;
                }
            }

            return idx;
        }


        /**
         *  Get the index of the first empty row ~ the index pointing to nobody
         *  
         *  @return             null if no ore more than one row is empty, an index otherwise
         */
        private int? getFirstEmptyRow() {
            int? idx = null;
            int len = edges.GetLength(0);

            for (int from = 0; from < len; from++) {
                bool empty = true;

                for (int to = 0; to < len; to++) {
                    empty &= !edges[from, to];
                }

                if (empty) {
                    if (idx.HasValue) {
                        return null;
                    }

                    idx = from;
                }
            }

            return idx;
        }


        /**
         *  Gets the maximum number of parallel nodes
         *  
         *  @param slices       a list of all slices (each slice contains parallel nodes)
         *  @return             the maximum number of parallel nodes
         */
        private int getMaxParallelNodes(List<SortedSet<int>> slices) {
            int h = 1;

            foreach (var slice in slices) {
                if (slice.Count > h) {
                    h = slice.Count;
                }
            }

            return h;
        }


        /**
         *  Return the indizes of the nodes following the given node indizes
         *  
         *  @param indizes      the current nodes to get the successors from
         *  @return             a set of successor indizes
         */
        private SortedSet<int> getFollowingNodes(SortedSet<int> indizes) {
            SortedSet<int> found = new SortedSet<int>();

            foreach (int idx in indizes) {
                for (int to = 0; to < edges.GetLength(0); to++) {
                    if (edges[idx, to] && !nodes[to].checkPosition()) {
                        found.Add(to);
                    }
                }
            }

            return found;
        }
    }
}
