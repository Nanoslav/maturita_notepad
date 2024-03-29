﻿using System;
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
        bool cancelPrevent = false;

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

            if (!cancelPrevent)
            {
                textBoxEditor.Clear();
                filePath = "";
                originalText = "";
                this.Text = "Notepad - New File";
            }
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
                cancelPrevent = true;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Edit Menu

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Undo();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.SelectAll();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxEditor.Paste();
        }

        private void dateAndTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            int index = textBoxEditor.SelectionStart;
            textBoxEditor.Text = textBoxEditor.Text.Insert(index, date.ToString());
            textBoxEditor.SelectionStart = index + date.ToString().Length;
        }

        private void darkAndLightThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!darkAndLightThemeToolStripMenuItem.Checked)
            {
                textBoxEditor.BackColor = Color.White;
                textBoxEditor.ForeColor = Color.Black;
            }
            else
            {
                textBoxEditor.BackColor = Color.Black;
                textBoxEditor.ForeColor = Color.White;
            }
        }
        #endregion

        #region Format Menu

        private void lineBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lineBreakToolStripMenuItem.Checked)
            {
                textBoxEditor.WordWrap = true;
            }
            else
            {
                textBoxEditor.WordWrap = false;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult fontDialog = fontDialog1.ShowDialog();
            if (fontDialog == DialogResult.OK)
            {
                textBoxEditor.Font = fontDialog1.Font;
            }
        }

        #endregion

        #region About Menu

        private void authorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Name & surname: xxxx yyyy\nClass: zzz", "Author", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void infoAboutMZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Maturita exam 2023/2024\nText editor", "MZ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private void textBoxEditor_Click(object sender, EventArgs e)
        {
            int row = textBoxEditor.GetLineFromCharIndex(textBoxEditor.SelectionStart);
            int col = textBoxEditor.SelectionStart - textBoxEditor.GetFirstCharIndexFromLine(row);

            RowColumn.Text = "Row " + (row + 1) + ", Col " + (col + 1);
        }

        private void textBoxEditor_KeyDown(object sender, KeyEventArgs e)
        {
            int row = textBoxEditor.GetLineFromCharIndex(textBoxEditor.SelectionStart);
            int col = textBoxEditor.SelectionStart - textBoxEditor.GetFirstCharIndexFromLine(row);

            RowColumn.Text = "Row " + (row + 1) + ", Col " + (col + 1);
        }

        private void textBoxEditor_KeyUp(object sender, KeyEventArgs e)
        {
            int row = textBoxEditor.GetLineFromCharIndex(textBoxEditor.SelectionStart);
            int col = textBoxEditor.SelectionStart - textBoxEditor.GetFirstCharIndexFromLine(row);

            RowColumn.Text = "Row " + (row + 1) + ", Col " + (col + 1);
        }

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

            if (textBoxEditor.Lines.Count() > 1)
            {
                CharCount.Text = "Char count: " + (1 + textBoxEditor.Text.Length - textBoxEditor.Lines.Count());
            }
            else
            {
                CharCount.Text = "Char count: " + textBoxEditor.Text.Length;
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
            if (cancelPrevent)
            {
                e.Cancel = true;
            }

            cancelPrevent = false;
        }
    }
}
