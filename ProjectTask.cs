using System;
using System.Collections.Generic;
using System.Text;
using PluginSDK;
using Microsoft.Office.Interop.MSProject;

namespace ProjectPlugins
{
    public class ProjectTask : ITask, IComparable
    {
        private Task m_task;
        private DateTime m_startDate;

        public ProjectTask(DateTime startDate)
        {
            m_startDate = startDate;
        }

        public ProjectTask(Task task)
        {
            m_task = task;
            m_startDate = DateTime.Parse(task.Start.ToString());
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() == typeof(ProjectTask))
            {
                ProjectTask temp = (ProjectTask)obj;
                return m_startDate.CompareTo(temp.StartDate);
            }
            throw new ArgumentException("object is not a ProjectTask");
        }

        public string Summary
        {
            get
            {
                if (m_task.OutlineParent != null)
                    return m_task.Name + " - " + m_task.OutlineParent.Name;
                else
                    return m_task.Name;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return m_startDate;
            }
            set
            {
                m_startDate = value;
                if (m_task != null)
                    m_task.Start = value.ToShortDateString();
            }
        }

        public DateTime DueDate
        {
            get
            {
                return DateTime.Parse(m_task.Finish.ToString());
            }
        }

        public bool Complete { get { return (int)m_task.PercentComplete == 100; } }

        public Task Task
        {
            get
            {
                return m_task;
            }
        }

        public void Check()
        {
            m_task.PercentComplete = 100;
        }
    }
}
