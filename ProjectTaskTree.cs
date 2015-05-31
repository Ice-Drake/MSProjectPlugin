using System;
using System.Collections.Generic;
using System.Text;
using PluginSDK;
using Microsoft.Office.Interop.MSProject;

namespace ProjectPlugins
{
    public class ProjectTaskTree : AVLTree<ProjectTask>
    {
        public ProjectTaskTree()
        {
        }

        /**
        * Find an item in the tree.
        * @param x the item to search for.
        * @return the matching item or null if not found.
        */
        public ProjectTask find(DateTime date)
        {
            ProjectTask search = new ProjectTask(date);
            return base.find(search);
        }

        public List<ITask> GetDateInfo(DateTime date)
        {
            List<ITask> tasks = new List<ITask>();
            ProjectTask search = new ProjectTask(date);
            getTreeContent(root, new AVLNode<ProjectTask>(search), tasks);
            return tasks;
        }

        /**
        * Internal method to print a subtree in sorted order.
        * @param t the node that roots the tree.
        */
        private void getTreeContent(AVLNode<ProjectTask> t, AVLNode<ProjectTask> x, List<ITask> list)
        {
            if (t != null)
            {
                getTreeContent(t.left, x, list);
                if (x.element.CompareTo(t.element) >= 0 && t.element.Task.Status != PjStatusType.pjComplete && CompletePrerequisite(t.element.Task))
                    list.Add(t.element);
                getTreeContent(t.right, x, list);
            }
        }

        private bool CompletePrerequisite(Microsoft.Office.Interop.MSProject.Task task)
        {
            bool satisfied = true;
            if (task.OutlineLevel > 1)
            {
                satisfied = CompletePrerequisite(task.OutlineParent);
            }
            foreach (Microsoft.Office.Interop.MSProject.Task pretask in task.PredecessorTasks)
            {
                if (pretask.Status != PjStatusType.pjComplete)
                    return false;
            }
            return satisfied;
        }
    }
}
