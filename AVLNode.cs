using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectPlugins
{
    public class AVLNode<T>
    {
        #region Constructors

        public AVLNode(T element) : this(element, null, null) { }

        public AVLNode(T element, AVLNode<T> left, AVLNode<T> right)
        {
            this.element = element;
            this.left = left;
            this.right = right;
            this.height = 0;
        }

        #endregion

        public T element; // The data in the node
        public AVLNode<T> left; // Left child
        public AVLNode<T> right; // Right child
        public int height; // Height
    }
}
