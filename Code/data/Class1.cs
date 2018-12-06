using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace data
{


    public class CLcad
    {

        private string cnx;
        private string rq_sql;
        private SqlDataAdapter sqlAdapter;
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private DataSet dataset;

        public CLcad()
        {
            Console.WriteLine("epepkepekpekpepepke");
            this.cnx = @"Data Source=PC-DAV;Initial Catalog=DB_A2_WS2;User ID=sa;Password=password1";
            this.sqlCommand = new SqlCommand();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.Connection = sqlConnection;

            try
            {
                this.sqlConnection = new SqlConnection(cnx);
                this.sqlConnection.Open();
                Console.WriteLine("rgrgrgr");
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine("IOException source: {0}", e.Source);
            }


        }

        public void actionRows(string rq_sql)
        {

        }

        public DataSet getRows(string rq_sql, string dataTableName)
        {
            DataSet dataset = new DataSet();

            //sqlCommand = new SqlCommand(rq_sql, sqlConnection);
            sqlCommand.CommandText = rq_sql;
            sqlCommand.Connection = sqlConnection;

            sqlConnection = new SqlConnection(cnx);
            sqlConnection.Open();

            SqlDataAdapter test = new SqlDataAdapter(sqlCommand);
            test.Fill(dataset, "test");

            return dataset;
        }

    }
}
