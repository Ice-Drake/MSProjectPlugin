using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectPlugins
{
    /**
     * Implements an AVL tree.
     * Note that all "matching" is based on the CompareTo method.
     * @author Mark Allen Weiss
     */
    public class AVLTree<T> where T : IComparable
    {
        /**
         * Construct the tree.
         */
        public AVLTree()
        {
            root = null;
        }

        /**
         * Insert into the tree; duplicates are ignored.
         * @param x the item to insert.
         */
        public void insert(T x)
        {
            root = insert(x, root);
        }

        /**
         * Remove from the tree. Nothing is done if x is not found.
         * @param x the item to remove.
         * @return true if removed successfully, false otherwise.
         */
        public bool remove(T x)
        {
            return remove(x, ref root) != null;
        }

        /**
         * Find the smallest item in the tree.
         * @return smallest item or null if empty.
         */
        public T findMin()
        {
            return elementAt(findMin(root));
        }

        /**
         * Find the largest item in the tree.
         * @return the largest item of null if empty.
         */
        public T findMax()
        {
            return elementAt(findMax(root));
        }

        /**
         * Find an item in the tree.
         * @param x the item to search for.
         * @return the matching item or null if not found.
         */
        public T find(T x)
        {
            return elementAt(find(x, root));
        }

        /**
         * Make the tree logically empty.
         */
        public void makeEmpty()
        {
            root = null;
        }

        /**
         * Test if the tree is logically empty.
         * @return true if empty, false otherwise.
         */
        public bool isEmpty()
        {
            return root == null;
        }

        /**
         * Print the tree contents in sorted order.
         */
        public void printTree()
        {
            if( isEmpty( ) )
                System.Console.WriteLine( "Empty tree" );
            else
                printTree( root );
        }

        /**
         * Internal method to get element field.
         * @param t the node.
         * @return the element field or null if t is null.
         */
        private T elementAt(AVLNode<T> t)
        {
            return t == null ? default(T) : t.element;
        }

        /**
         * Internal method to insert into a subtree.
         * @param x the item to insert.
         * @param t the node that roots the tree.
         * @return the new root.
         */
        private AVLNode<T> insert(T x, AVLNode<T> t)
        {
            if (t == null)
                t = new AVLNode<T>(x, null, null);
            else if (x.CompareTo(t.element) < 0)
            {
                t.left = insert(x, t.left);
                if (height(t.left) - height(t.right) == 2)
                    //Left-Left case
                    if (x.CompareTo(t.left.element) < 0)
                        t = rightRotation(t);
                    //Left-Right case
                    else
                    {
                        t.left = leftRotation(t.left);
                        t = rightRotation(t);
                    }
            }
            else if (x.CompareTo(t.element) > 0)
            {
                t.right = insert(x, t.right);
                if (height(t.right) - height(t.left) == 2)
                    //Right-Right case
                    if (x.CompareTo(t.right.element) > 0)
                        t = leftRotation(t);
                    //Right-Left case
                    else
                    {
                        t.right = rightRotation(t.right);
                        t = leftRotation(t);
                    }
            }
            t.height = max(height(t.left), height(t.right)) + 1;
            return t;
        }

        /**
        * Internal method to remove from a subtree.
        * @param x the item to remove.
        * @param t the node that roots the tree.
        * @return the removed item.
        */
        private AVLNode<T> remove(T x, ref AVLNode<T> t)
        {
            AVLNode<T> found;

            // See if the tree is empty
            if (t == null)
                return null;
            else if (x.CompareTo(t.element) < 0)
            {
                found = remove(x, ref t.left);
                if (height(t.right) - height(t.left) == 2)
                    //Right-Right case
                    if (x.CompareTo(t.right.element) < 0)
                        t = leftRotation(t);
                    //Right-Left case
                    else
                    {
                        t.right = rightRotation(t.right);
                        t = leftRotation(t);
                    }
            }
            else if (x.CompareTo(t.element) > 0)
            {
                found = remove(x, ref t.right);
                if (height(t.left) - height(t.right) == 2)
                    //Left-Left case
                    if (x.CompareTo(t.left.element) > 0)
                        t = rightRotation(t);
                    //Left-Right case
                    else
                    {
                        t.left = leftRotation(t.left);
                        t = rightRotation(t);
                    }
            }
            else
            {
                found = t;

                if(t.left == null && t.right == null)
                // We have a leaf -- remove it
                {
                    t = null;
                    return found;
                }
                else if(t.right == null)
                // We have only left child -- the largest successor of the left child becomes
                // the new replacement and the original successor is removed afterward.
                {
                    AVLNode<T> successor = t.left;
                    while (successor.right != null)
                    {
                        successor = successor.right;
                    }
                    
                    t.element = successor.element;
                    remove(successor.element, ref t.left);
                }
                else
                // We have only right child or two children -- the smallest successor of the right child becomes
                // the new replacement and the original successor is removed afterward.
                {
                    AVLNode<T> successor = t.right;
                    while (successor.left != null)
                    {
                        successor = successor.left;
                    }
                    
                    t.element = successor.element;
                    remove(successor.element, ref t.right);
                }
            }
            t.height = max(height(t.left), height(t.right)) + 1;
            return found;
        }


        /**
         * Internal method to find the smallest item in a subtree.
         * @param t the node that roots the tree.
         * @return node containing the smallest item.
         */
        private AVLNode<T> findMin(AVLNode<T> t)
        {
            if (t == null)
                return t;

            while (t.left != null)
                t = t.left;
            return t;
        }

        /**
         * Internal method to find the largest item in a subtree.
         * @param t the node that roots the tree.
         * @return node containing the largest item.
         */
        private AVLNode<T> findMax(AVLNode<T> t)
        {
            if (t == null)
                return t;

            while (t.right != null)
                t = t.right;
            return t;
        }

        /**
         * Internal method to find an item in a subtree.
         * @param x is item to search for.
         * @param t the node that roots the tree.
         * @return node containing the matched item.
         */
        private AVLNode<T> find(T x, AVLNode<T> t)
        {
            while (t != null)
                if (x.CompareTo(t.element) < 0)
                    t = t.left;
                else if (x.CompareTo(t.element) > 0)
                    t = t.right;
                else
                    return t;    // Match

            return null;   // No match
        }

        /**
         * Internal method to print a subtree in sorted order.
         * @param t the node that roots the tree.
         */
        private void printTree(AVLNode<T> t)
        {
            if( t != null )
            {
                printTree( t.left );
                System.Console.WriteLine( t.element );
                printTree( t.right );
            }
        }

        /**
         * Return the height of node t, or -1, if null.
         */
        private static int height(AVLNode<T> t)
        {
            return t == null ? 0 : t.height;
        }

        /**
         * Return maximum of lhs and rhs.
         */
        private static int max(int lhs, int rhs)
        {
            return lhs > rhs ? lhs : rhs;
        }

        /**
         * Rotate binary tree node with left child.
         * For AVL trees, this is a single rotation for case 1.
         * Update heights, then return new root.
         */
        private static AVLNode<T> rightRotation(AVLNode<T> k2)
        {
            AVLNode<T> k1 = k2.left;
            k2.left = k1.right;
            k1.right = k2;
            k2.height = max(height(k2.left), height(k2.right)) + 1;
            k1.height = max(height(k1.left), k2.height) + 1;
            return k1;
        }

        /**
         * Rotate binary tree node with right child.
         * For AVL trees, this is a single rotation for case 4.
         * Update heights, then return new root.
         */
        private static AVLNode<T> leftRotation(AVLNode<T> k1)
        {
            AVLNode<T> k2 = k1.right;
            k1.right = k2.left;
            k2.left = k1;
            k1.height = max(height(k1.left), height(k1.right)) + 1;
            k2.height = max(height(k2.right), k1.height) + 1;
            return k2;
        }

        /** The tree root. */
        protected AVLNode<T> root;
    }
}
