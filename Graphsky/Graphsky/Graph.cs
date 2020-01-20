using System;


namespace Graphsky {
    class Graph {
        private Node[] nodes;
        private readonly bool[][] edges;

        public Graph(Node[] nodes_, int[][] adj_mat) {
            nodes = nodes_;
            Array.Sort(nodes, delegate (Node a, Node b) {
                return a.getId().CompareTo(b.getId());
            });

            edges = new bool[nodes.Length][];
            for (int i = 0; i < edges.Length; i++) {
                edges[i] = new bool[nodes.Length];
                for (int j = 0; j < edges[i].Length; j++) {
                    edges[i][j] = false;
                }
            }

            foreach (int[] pair in adj_mat) {
                int idx_from = Array.FindIndex(nodes, delegate (Node a) {
                    return a.getId() == pair[0];
                });

                int idx_to = Array.FindIndex(nodes, delegate (Node a) {
                    return a.getId() == pair[1];
                });

                edges[idx_from][idx_to] = true;
            }
        }
    }
}
