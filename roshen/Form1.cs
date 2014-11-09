using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;

namespace roshen
{
    public partial class Form1 : Form
    {
        Operator o; 
        public Form1()
        {
            InitializeComponent();
            o = new Operator("Database1.sdf");
            listView1.Items.AddRange(o.display());
            treeView1.BeginUpdate();
            treeView1.Nodes.Add("ParentNode");
            treeView1.EndUpdate();
        }

        private void addParentNode_Click(object sender, EventArgs e)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Add("ParentNode");
            treeView1.EndUpdate();
        }
        
        int i = 0;
        private void addChildNode_Click(object sender, EventArgs e)
        {
            TreeNode ParentNode = treeView1.SelectedNode;
            if (ParentNode != null)
            {
                ParentNode.Nodes.Add("ChildNode " + i);
                i++;
                treeView1.ExpandAll();
                treeView1.Invalidate();
            }
            else
                MessageBox.Show("Выделите ParentNode ");
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeNode ParentNode = treeView1.SelectedNode;
            if (ParentNode != null)
            {
                ParentNode.Nodes.Add("ChildNode " + i);
                i++;
                treeView1.ExpandAll();
                treeView1.Invalidate();
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (treeView1.SelectedNode != null && e.KeyCode == Keys.Delete)
                treeView1.SelectedNode.Remove();
            if(treeView1.Nodes.Count == 0)
            {
                treeView1.BeginUpdate();
                treeView1.Nodes.Add("ParentNode");
                treeView1.EndUpdate();
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            Form2 secondForm = new Form2("Database1.sdf");
            secondForm.ShowDialog();
            listView1.Items.Clear();
            listView1.Items.AddRange(o.display());
        }

        private void update()
        {
            Form2 secondForm = new Form2("Database1.sdf", new specimen(
                    Convert.ToDateTime(listView1.SelectedItems[0].SubItems[0].Text),
                    Convert.ToDecimal(listView1.SelectedItems[0].SubItems[1].Text),
                    listView1.SelectedItems[0].SubItems[2].Text,
                    Convert.ToInt32(listView1.SelectedItems[0].SubItems[3].Text)));
            secondForm.ShowDialog();
            listView1.Items.Clear();
            listView1.Items.AddRange(o.display());
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                update();
            else
                MessageBox.Show("Выделите элемент для удаления ");
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            update();
        }

        private void delete()
        {
            o.delete(new specimen(
                    Convert.ToDateTime(listView1.SelectedItems[0].SubItems[0].Text),
                    Convert.ToDecimal(listView1.SelectedItems[0].SubItems[1].Text),
                    listView1.SelectedItems[0].SubItems[2].Text,
                    Convert.ToInt32(listView1.SelectedItems[0].SubItems[3].Text)));
            listView1.Items.Clear();
            listView1.Items.AddRange(o.display());
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                delete();
            else
                MessageBox.Show("Выделите элемент для удаления ");
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && e.KeyCode == Keys.Delete)
                delete();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int processed = 0;
            bool root = true;
            saveTreeNode(treeView1.Nodes, 0, ref processed, ref root);
            string str = String.Empty;
            foreach (Trees t in l)
                str += "\t" + t.NoteID + "\t" + t.ParentNoteID + "\t" + t.NoteName + Environment.NewLine;
            MessageBox.Show(str);
            str = String.Empty;
            l.Clear();
        }
        List<Trees> l = new List<Trees>();
        private void saveTreeNode(TreeNodeCollection nodes, int NoteID, ref int processed, ref bool root)
        {
            foreach (TreeNode node in nodes)
            {
                int ParentNoteID;
                if (root)
                {
                    ParentNoteID = -1;
                    root = false;
                }
                else
                    ParentNoteID = NoteID - 1;
                l.Add(new Trees(NoteID, ParentNoteID, node.Text));
                ++processed;
                saveTreeNode(node.Nodes, NoteID + 1, ref processed, ref root);
                root = true;
                NoteID = processed;
            }
        }
        /*
        private void saveTreeNode(TreeNodeCollection nodes, int NoteID, ref int processed)
        {
            foreach (TreeNode node in nodes)
            {
                int ParentNoteID;
                if (NoteID <= 0)
                    ParentNoteID = -1;
                else
                    ParentNoteID = NoteID - 1;
                l.Add(new Trees(NoteID, ParentNoteID, node.Text));
                ++processed;
                saveTreeNode(node.Nodes, NoteID+1, ref processed);
                //ParentNoteID = -1;
                NoteID = processed;
            }
            
        }
        private void saveTreeNode(ref int NoteID, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                int ParentNoteID;
                if (NoteID == 0)
                    ParentNoteID = -1;
                else
                    ParentNoteID = NoteID - 1;
                l.Add(new Trees(NoteID, ParentNoteID, node.Text));
                //++processed;
                //int localProcseed = processed;
                saveTreeNode(ref NoteID, node.Nodes);
                NoteID++;
            }
        }
        
        List<TreeNode> list = new List<TreeNode>();
        private List<TreeNode> GetAllNodes(TreeNode Node)
        {
            
            //List<Trees> l = new List<Trees>();
            list.Add(Node);
            foreach (TreeNode n in Node.Nodes)
                list.AddRange(GetAllNodes(n));
            return list;
        }

        private void treeNode()
        {
            /*
            for (int i = 0; i < treeNode.Nodes.Count; i++)
            {
                l.Add(new Trees(c, treeNode.Text, i));
            }
            //TreeNode node = treeView1.GetNodeCount .SelectedNode;
            //node.GetNodeCount
            
            str += "1 " + treeNode.Text + Environment.NewLine;
            foreach (TreeNode childChildNode in treeNode.Nodes)
            {
                str += "2 " + childChildNode.Text + " 1" + Environment.NewLine;
                foreach (TreeNode c in childChildNode.Nodes)
                {
                    str += "3 " + c.Text + " 2" + Environment.NewLine;
                }
            }
        }
        }*/
    }
}
