using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace notepad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string filePath = "";
        bool editing = false;
        string originalText = "";

        #region File Menu

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editing)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes to?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    saveAsToolStripMenuItem.PerformClick();
                }
                else if (result == DialogResult.Cancel)
                {
                    textBoxEditor.DeselectAll();
                    return;
                }
            }

            textBoxEditor.Clear();
            filePath = "";
            originalText = "";
            this.Text = "Notepad - New File";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editing)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    saveAsToolStripMenuItem.PerformClick();
                }
                else if (result == DialogResult.Cancel)
                {
                    textBoxEditor.DeselectAll();
                    return;
                }
            }

            DialogResult openFile = openFileDialog1.ShowDialog();
            if (openFile == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFileDialog1.FileName);
                textBoxEditor.Text = reader.ReadToEnd();
                reader.Close();
                filePath = openFileDialog1.FileName;
                originalText = textBoxEditor.Text;
                this.Text = "Notepad - " + filePath;
            }else if (openFile == DialogResult.Cancel)
            {
                return;
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filePath == "")
            {
                saveAsToolStripMenuItem.PerformClick();
            }
            else
            {
                StreamWriter writer = new StreamWriter(filePath, false);
                writer.Write(textBoxEditor.Text);
                writer.Close();
                originalText = textBoxEditor.Text;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult saveFile = saveFileDialog1.ShowDialog();
            if (saveFile == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false);
                writer.Write(textBoxEditor.Text);
                writer.Close();
                filePath = saveFileDialog1.FileName;
                originalText = textBoxEditor.Text;
                this.Text = "Notepad - " + filePath;
            }
            else if (saveFile == DialogResult.Cancel)
            {
                return;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void textBoxEditor_TextChanged(object sender, EventArgs e)
        {
            if (textBoxEditor.Text == originalText)
            {
                editing = false;
            }
            else
            {
                editing = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (editing)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    saveAsToolStripMenuItem.PerformClick();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
