using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        DataSet test;
        middleware.CLprocessus middleware;

        public Form1()
        {
            InitializeComponent();

            test = new DataSet();
            middleware = new middleware.CLprocessus();

            this.button1.Click += new System.EventHandler(this.button1_Click);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("click !");
            test = middleware.afficher("[dbo].[TB_A2_WS2]", textBox1.Text);
            dataGridView1.DataSource = test;
            dataGridView1.DataMember = "test";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
