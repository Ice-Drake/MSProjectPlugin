using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectPlugins
{
    public class AVLDateListElement<T> : IComparable
    {
        #region Constructors

        public AVLDateListElement(DateTime date)
        {
            this.date = date;
            this.list = new List<T>();
        }

        #endregion

        public int CompareTo(object obj)
        {
            if (obj.GetType() == typeof(AVLDateListElement<T>))
            {
                AVLDateListElement<T> temp = (AVLDateListElement<T>)obj;
                return date.CompareTo(temp.date);
            }
            throw new ArgumentException("object is not a AVLDateListElement<T>");
        }

        public DateTime date; // The data in the node
        public List<T> list; // Height
    }
}
