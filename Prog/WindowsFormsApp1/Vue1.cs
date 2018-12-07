using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Vue1 : Form
    {
        public Vue1()
        {
            InitializeComponent();

            Controller.World test = new Controller.World(20, 20);

            Controller.Cuisine cuisine = test.InstantiateCuisine(5, 5, new int[] { 15, 15 });
            Console.WriteLine(test.GetTableEntity(new int[] { 16, 16 }).typeSalle);

            Controller.Loader loader = new Controller.Loader();

            dataGridView1.Dock = DockStyle.Fill;

            dataGridView1.AutoGenerateColumns = true;

            FillDataGridView(test.GenerateGridView());
        }

        private void FillDataGridView(string[,] table)
        {
            dataGridView1.ColumnCount = table.GetLength(0);

            for (int y = 0; y < table.GetLength(1); y++)
            {
                string[] row = new string[table.GetLength(0)];

                for (int x = 0; x < table.GetLength(0); x++)
                {
                    row[x] = table[x, y];
                }
                dataGridView1.Rows.Add(row);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
