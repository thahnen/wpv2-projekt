using System;
using System.Linq;
using System.Collections.Generic;


namespace Graphsky {
    class Graph {
        // List of all graph nodes, sorted by Id
        private Node[] nodes;

        // First index equals node from which arrow points
        // Second index equals node where arrow points
        private readonly bool[,] edges;

        public Graph(Node[] nodes_, int[][] adj_mat) {
            nodes = nodes_;
            Array.Sort(nodes, delegate (Node a, Node b) {
                return a.getId().CompareTo(b.getId());
            });

            edges = new bool[nodes.Length, nodes.Length];

            foreach (int[] pair in adj_mat) {
                int idx_from = Array.FindIndex(nodes, delegate (Node a) {
                    return a.getId() == pair[0];
                });

                int idx_to = Array.FindIndex(nodes, delegate (Node a) {
                    return a.getId() == pair[1];
                });

                edges[idx_from, idx_to] = true;
            }
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

            nodes[(int)idx_first].setPosition(1, 0);

            // find ending node (the one pointing to nobody)
            int? idx_last = getFirstEmptyRow();
            if (!idx_last.HasValue) {
                return false;
            }

            int width = 1;
            int height = getMaxParallelNodes(ref width, (int)idx_first);

            int x = 2;
            int[] indizes = getFollowingNodes((int)idx_first);
            int len = indizes.Length;
            while (len != 0) {
                int y = (int) -Math.Floor((double)len / 2);
                foreach (int idx in indizes) {
                    nodes[idx].setPosition(x, y++);
                }
                x++;
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
         *  @param len          the length of the graph
         *  @param ids          the list of ids to check on their successors
         *  @return             the maximum number of parallel nodes
         */
        private int getMaxParallelNodes(ref int len, params int[] indizes) {
            int[] found = getFollowingNodes(indizes);

            int max = indizes.Length;
            if (found.Length != 0) {
                max = Math.Max(max, getMaxParallelNodes(ref len, found));
                len += 1;
            }

            return max;
        }

        /**
         *  Return the indizes of the nodes following the given nodes indizes
         *  
         *  @param indizes      the current nodes to get the successors from
         *  @return             a list (converted set) of successor indizes
         */
        private int[] getFollowingNodes(params int[] indizes) {
            SortedSet<int> found = new SortedSet<int>();

            foreach (int idx in indizes) {
                for (int to = 0; to < edges.GetLength(0); to++) {
                    if (edges[idx, to]) {
                        found.Add(to);
                    }
                }
            }

            return found.ToArray<int>();
        }
    }
}
