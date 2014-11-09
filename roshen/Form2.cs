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

namespace roshen
{
    public partial class Form2 : Form
    {
        Operator o;
        bool check;
        int id, identifier;

        public Form2(string o)
        {
            InitializeComponent();
            check = true;
            this.o = new Operator(o);
            this.id = this.o.id + 1;
            identifier = 0;
        }

        public Form2(string o, specimen changeFrom)
        {
            InitializeComponent();
            check = true;
            this.o = new Operator(o);
            dateTimePicker1.Value = changeFrom.date;
            numericUpDown1.Value = changeFrom.digit;
            textBox2.Text = changeFrom.text;
            this.id = changeFrom.id;
            identifier = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (check)
            {
                o.write(identifier, new specimen(dateTimePicker1.Value, numericUpDown1.Value, textBox2.Text, id));
                check = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (check)
                o.write(identifier, new specimen(dateTimePicker1.Value, numericUpDown1.Value, textBox2.Text, id));
            this.Close();
        }
    }
}
