using System;

namespace Graphsky {
    /**
     *  Stores information about a given node
     */
    class Node {
        public readonly int id;
        private int? x, y;


        public Node(int id_) {
            id = id_;
            x = null;
            y = null;
        }


        /**
         *  Sets the uniform coordinates
         *  
         *  @param x_           value for x
         *  @param y_           value for y
         */
        public void setPosition(int x_, int y_) {
            x = x_;
            y = y_;
        }


        /**
         *  Returns the uniform coordinates of the node
         *  
         *  @return             tuple containing x and y in that order
         */
        public Tuple<int, int> getPosition() {
            return new Tuple<int, int>((int)x, (int)y);
        }
    }
}
