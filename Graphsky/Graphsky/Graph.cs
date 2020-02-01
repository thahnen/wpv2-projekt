using System;
using System.Collections.Generic;


namespace Graphsky {
    /// Output structure for Graph (resembles input)
    public struct OGraph {
        public readonly int Width;
        public readonly int Height;

        public readonly object[] Nodes;
        public readonly bool[,] Adjacency;

        public OGraph(ref Graph given) {
            given.GetExtent().Unpack(out Width, out Height);

            Nodes = new object[given.Nodes.Length];
            for (int i = 0; i < Nodes.Length; i++) {
                int ux, uy;
                given.Nodes[i].GetPosition().Unpack(out ux, out uy);

                Nodes[i] = new {
                    Id = given.Nodes[i].Id,
                    Coords = new {
                        uX = ux,
                        uY = uy
                    }
                };
            }

            Adjacency = given.Edges;
        }
    }


    /// Stores information about a given graph
    public class Graph {
        // List of all graph nodes, sorted by Id
        public Node[] Nodes { get; private set; }
        public Node First { get; private set; }
        public Node Last { get; private set; }

        // First index equals node where arrow points from
        // Second index equals node where arrow points to
        public readonly bool[,] Edges;

        private int? width;
        private int? height;


        /**
         *  Constructor, parameters are called "Nodes" and "Edges" based on the JSON-field!
         */
        public Graph(Node[] Nodes, int[][] Edges) {
            this.Nodes = Nodes;
            Array.Sort(this.Nodes, delegate (Node a, Node b) {
                return a.Id.CompareTo(b.Id);
            });

            this.Edges = new bool[this.Nodes.Length, this.Nodes.Length];

            foreach (int[] pair in Edges) {
                int idx_from = Array.FindIndex(this.Nodes, delegate (Node a) {
                    return a.Id == pair[0];
                });

                int idx_to = Array.FindIndex(this.Nodes, delegate (Node a) {
                    return a.Id == pair[1];
                });

                this.Edges[idx_from, idx_to] = true;
            }
        }


        /**
         *  Returns the extent of the graph
         *  - width -> number of horizontal nodes
         *  - height -> number of vertical nodes
         *  
         *  @return             tuple containing width and height in that order
         */
        public Tuple<int, int> GetExtent() {
            return new Tuple<int, int>((int)width, (int)height);
        }



        /**
         *  Calculates uniforma coordinates for the given nodes
         *  
         *  @return             true, if there was no problem with the given nodes
         */
        public bool CalculateUniformCoordinates() {
            // find entry node (the one nobody points to)
            int? idx_first = GetFirstEmptyColumn();
            if (!idx_first.HasValue) {
                return false;
            }

            First = Nodes[(int)idx_first];

            // find ending node (the one pointing to nobody)
            int? idx_last = GetFirstEmptyRow();
            if (!idx_last.HasValue) {
                return false;
            }

            Last = Nodes[(int)idx_last];

            int idx = 0;

            // Get slices of graph
            List<SortedSet<int>> slices = new List<SortedSet<int>>();
            slices.Add(new SortedSet<int>(new int[] { (int)idx_first }));

            do {
                SortedSet<int> following = GetFollowingNodes(slices[idx]);
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

            width = slices.Count;
            height = GetMaxParallelNodes(slices);

            for (idx = 0; idx < slices.Count; idx++) {
                // TODO: take a look at straight / odd numbers
                int y, step_size;
                if (slices[idx].Count % 2 == 0) {
                    y = -(slices[idx].Count - 1);
                    step_size = 2;
                } else {
                    y = (int) -Math.Floor((double)slices[idx].Count / 2);
                    step_size = 1;
                }

                foreach (int node in slices[idx]) {
                    Nodes[node].SetPosition(idx, y);
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
        private int? GetFirstEmptyColumn() {
            int? idx = null;
            int len = Edges.GetLength(0);

            for (int to = 0; to < len; to++) {
                bool empty = true;

                for (int from = 0; from < len; from++) {
                    empty &= !Edges[from, to];
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
        private int? GetFirstEmptyRow() {
            int? idx = null;
            int len = Edges.GetLength(0);

            for (int from = 0; from < len; from++) {
                bool empty = true;

                for (int to = 0; to < len; to++) {
                    empty &= !Edges[from, to];
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
        private int GetMaxParallelNodes(List<SortedSet<int>> slices) {
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
        private SortedSet<int> GetFollowingNodes(SortedSet<int> indizes) {
            SortedSet<int> found = new SortedSet<int>();

            foreach (int idx in indizes) {
                for (int to = 0; to < Edges.GetLength(0); to++) {
                    if (Edges[idx, to] && !Nodes[to].CheckPosition()) {
                        found.Add(to);
                    }
                }
            }

            return found;
        }
    }
}
