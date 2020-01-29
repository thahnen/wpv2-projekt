using System;


namespace Graphsky {
    /// Stores information about a given node
    public class Node {
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
         *  Checks if uniform position is already set
         *  
         *  @return             true if position set, false otherwise
         */
        public bool checkPosition() {
            return (x.HasValue & y.HasValue);
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
