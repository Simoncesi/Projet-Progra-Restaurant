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

            //Controller.Cuisine cuisine = test.InstantiateCuisine(5, 5, new int[] { 15, 15 });
            //Console.WriteLine(test.GetTableEntity(new int[] { 16, 16 }).typeSalle);

            //Controller.Client perso1 = new Controller.Client(0, 0, "Jean", "Roger", 20, new int[] { 0, 0 }, test.GetLoader());
            //Controller.Client perso2 = new Controller.Client(0, 0, "Jean", "Roger", 20, new int[] { 0, 0 }, test.GetLoader());

            test.InstantiateRestaurant(10, 10, new int[] { 0, 0 });
            test.InstantiateHall(10, 10, new int[] { 10, 0 });
            test.InstantiateComptoir(10, 0, new int[] { 0, 11 }, new int[] { 0, 0 }, new int[] { 0, 10 });

            List<List<String>> menu = new List<List<string>>()
            {
                new List<String>{
                    "entree1", "plat1", "dessert1"
                },
                new List<String>{
                    "entree2", "plat2", "dessert2"
                }
            };

            test.GetComptoir().LoadCarte(new Controller.Carte(test.GetComptoir().stockCartes, test.GetLoader(), menu));


            test.getRestaurant().GenerateTables(new int[,] { { 1, 1 }, { 2, 2 } });
            Controller.MaitreHotel maitre = new Controller.MaitreHotel("Eugene", "Baskiez", 60, new int[] { 15, 15 }, test.GetLoader());
            Controller.ChefDeRang chefDeRang = new Controller.ChefDeRang("Robert", "Baskiez", 60, new int[] { 15, 15 }, test.GetLoader(), maitre);
            Controller.ChefDeRang chefDeRang2 = new Controller.ChefDeRang("Jean", "Baskiez", 60, new int[] { 15, 15 }, test.GetLoader(), maitre);


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
