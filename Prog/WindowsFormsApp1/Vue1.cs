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
            Console.WriteLine(cuisine.GetType());

            Controller.Loader loader = new Controller.Loader();

            Controller.Entity test2 = new Controller.Entity(loader);
            Controller.Entity test3 = new Controller.Entity(loader);

            Console.WriteLine(test2.id);
            Console.WriteLine(test3.id);

        }
    }
}
