using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
//using System.Web;

namespace roshen
{
    public class Trees
    {
        public Trees()
        {
        }
        public Trees(int NoteID, int ParentNoteID, string NoteName)
        {
            this.NoteID = NoteID;
            this.ParentNoteID = ParentNoteID;
            this.NoteName = NoteName;
        }
        public int NoteID { get; private set; }
        public int ParentNoteID { get; private set; }
        public string NoteName { get; private set; }
    }

    public class specimen
    {
        public specimen()
        {
        }
        public specimen(DateTime date, decimal digit, string text, int id)
        {
            this.date = date;
            this.digit = digit;
            this.text = text;
            this.id = id;
        }
        public DateTime date { get; private set; }
        public decimal digit { get; private set; }
        public string text { get; private set; }
        public int id { get; private set; }
    }

    class Operator
    {
        public int id { get; private set; }
        private SqlCeConnection connection;
        private SqlCeCommand command;

        public Operator()
        {
        }

        public Operator(string data)
        {
            this.connection = new SqlCeConnection(Convert.ToString("Data Source = " + data));
            getId();
        }

        private void getId()
        {
            string sql = "select max(id) from template1";
            using (command = new SqlCeCommand(sql, connection))
            {
                try
                {
                    connection.Open();
                    id = (int)command.ExecuteScalar();
                }
                catch //(Exception e)
                {
                    id = 0;
                    //throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool writeTree(List<Trees> Items)
        {
            string sql = "INSERT INTO tree (NoteID, NoteName, ParentNoteID) Values(@NoteID,@NoteName,@ParentNoteID)";    //"usp_GetEmployees"
            using (command = new SqlCeCommand(sql, connection))
            {
                try
                {
                    connection.Open();
                    foreach (Trees tree in Items)
                    {
                        command.Parameters.AddWithValue("@NoteID", tree.NoteID);
                        command.Parameters.AddWithValue("@NoteName", tree.NoteName);
                        command.Parameters.AddWithValue("@ParentNoteID", tree.ParentNoteID);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public TreeNode[] readTree()
        {
            List<TreeNode> trees = new List<TreeNode>();
            string sql = "SELECT * FROM tree";
            using (command = new SqlCeCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    List<Trees> temp = new List<Trees>();
                    SqlCeDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        temp.Add(new Trees(
                            int.Parse(dataReader["NoteID"].ToString()),
                            int.Parse(dataReader["ParentNoteID"].ToString()),
                            dataReader["NoteName"].ToString()
                        ));
                    }
                    TreeNode temp1 = new TreeNode();
                    foreach (Trees tree in temp)
                    {
                        Trees parentEmp = temp.Find(o => o.NoteID == tree.ParentNoteID);
                        if (parentEmp != null)
                            temp1.Nodes.Find(parentEmp.ParentNoteID.ToString(), true)[0].Nodes.Add(tree.ParentNoteID.ToString(), tree.NoteName);
                        else
                            temp1.Nodes.Add(tree.ParentNoteID.ToString(), tree.NoteName);
                        trees.Add(temp1);
                    }
                    return trees.ToArray();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public ListViewItem[] display()
        {
            string sql = "SELECT * FROM template1 ORDER BY date";
            using (command = new SqlCeCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    List<ListViewItem> temp = new List<ListViewItem>();
                    SqlCeDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        string[] st = new string[dataReader.FieldCount];
                        for (int i = 0; i < dataReader.FieldCount; i++)
                            st[i] = dataReader.GetValue(i).ToString();
                        temp.Add(new ListViewItem(st));
                    }
                    return temp.ToArray();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool delete(specimen Items)
        {
            string sql = "DELETE FROM template1 WHERE id = " + Items.id;
            using (command = new SqlCeCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch(Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool write(int identifier, specimen Items)
        {
            if (Items.date != null && Items.digit > 0 && Items.text != String.Empty)
            {
                string sql;
                switch (identifier)
                {
                    case 0:
                        sql = "INSERT INTO template1 (date, digit, text, id) Values(@date,@digit,@text,@id)";
                        break;
                    case 1:
                        sql = "UPDATE template1 SET date = @date, digit = @digit, text = @text, id = @id WHERE ID = @id";
                        break;
                    default:
                        return false;
                }
                using (command = new SqlCeCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@date", Items.date);
                        command.Parameters.AddWithValue("@digit", Items.digit);
                        command.Parameters.AddWithValue("@text", Items.text);
                        command.Parameters.AddWithValue("@id", Items.id);
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        connection.Close();
                        getId();
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}
