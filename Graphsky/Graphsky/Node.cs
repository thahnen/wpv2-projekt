using System;


namespace Graphsky {
    /// Stores information about a given node
    public class Node {
        public readonly int Id;
        private int? x, y;


        /**
         *  Constructor, parameter is called "id_" based on the JSON-field!
         */
        public Node(int id_) {
            Id = id_;
        }


        /**
         *  Sets the uniform coordinates
         *  
         *  @param x            value for x
         *  @param y            value for y
         */
        public void SetPosition(int x, int y) {
            this.x = x;
            this.y = y;
        }


        /**
         *  Checks if uniform position is already set
         *  
         *  @return             true if position set, false otherwise
         */
        public bool CheckPosition() {
            return (x.HasValue & y.HasValue);
        }


        /**
         *  Returns the uniform coordinates of the node
         *  
         *  @return             tuple containing x and y in that order
         */
        public Tuple<int, int> GetPosition() {
            return new Tuple<int, int>((int)x, (int)y);
        }
    }
}
