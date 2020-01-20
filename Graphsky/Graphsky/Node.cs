using System;

namespace Graphsky {
    /**
     *  Stores information about a given node
     */
    class Node {
        private int? x, y;
        private readonly int id;

        public Node(int id_) {
            id = id_;
        }

        public int getId() {
            return id;
        }

        public void setPosition(int x_, int y_) {
            x = x_;
            y = y_;
        }
    }
}
