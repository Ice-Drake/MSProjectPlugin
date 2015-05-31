using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PluginSDK;
using Microsoft.Office.Interop.MSProject;

namespace ProjectPlugins
{
    public class MSProjectPanel : Form, IProjDatabase
    {
        public string AuthorName
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string PluginName
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string PluginVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        public event FormClosingEventHandler PanelClosing;
        public event ITaskHandler DatabaseChanged;

        private ProjectTaskTree taskTree;
        private string projectAbsPath;
        private ApplicationClass application;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;        

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MSProjectPanel
            // 
            this.ClientSize = new System.Drawing.Size(335, 288);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MSProjectPanel";
            this.Text = "MSProject Panel";
            this.ResumeLayout(false);

        }

        #endregion

        public MSProjectPanel()
        {
            InitializeComponent();
            taskTree = new ProjectTaskTree();
            application = new ApplicationClass();
            projectAbsPath = System.IO.Path.GetFullPath("Projects");
            this.FormClosing += new FormClosingEventHandler(MSProjectPanel_FormClosing);
        }

        private void MSProjectPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PanelClosing != null)
                PanelClosing(this, e);
        }

        public void loadDatabase()
        {
            bool readOnly = false;
            object missing = System.Reflection.Missing.Value;
            bool ignoreReadOnlyRecommended = true;

            foreach (string file in System.IO.Directory.GetFiles(projectAbsPath, "*.mpp"))
            {
                Project project;

                try
                {
                    application.FileOpen(file, readOnly, PjMergeType.pjDoNotMerge, missing, missing, missing, missing, missing, missing, missing, missing, PjPoolOpen.pjDoNotOpenPool, missing, missing, ignoreReadOnlyRecommended, missing);
                    
                    project = application.ActiveProject;
                    foreach (Task task in project.Tasks)
                    {
                        if (task.Status != PjStatusType.pjComplete && task.OutlineChildren.Count == 0)
                        {
                            taskTree.insert(new ProjectTask(task));
                        }
                    }
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    MessageBox.Show("Please install or reinstall Microsoft Project.");
                }
            }

            application.Visible = false;
        }

        public void closeDatabase()
        {
            application.FileCloseAll(PjSaveType.pjDoNotSave);
        }

        public void LoadCategory(List<string> list)
        {

        }
        
        public void showPanel()
        {
            Show();
        }

        public void hidePanel()
        {
            Hide();
        }
    }
}