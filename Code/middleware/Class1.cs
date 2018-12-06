using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace middleware
{
    public class CLmapTB_A2_WS2
    {
        private string rq_sql;
        private int id;
        private string nom;
        private string prenom;

        public string selectAll()
        {
            return "SELECT * FROM [DB_A2_WS2].[dbo].[TB_A2_WS2]";
        }

        public string selectByName(string nom)
        {
            return "SELECT * FROM [DB_A2_WS2].[dbo].[TB_A2_WS2] WHERE nom='" + nom + "'";
        }

        public string delete(string id)
        {
            return "DELETE FROM [DB_A2_WS2].[dbo].[TB_A2_WS2] WHERE id='" + id + "'";
        }
    }

    public class CLprocessus
    {
        private DataSet oDs;
        private CLmapTB_A2_WS2 oMap;
        private data.CLcad oCad;
        private string rq_sql;

        public CLprocessus()
        {
            this.oMap = new CLmapTB_A2_WS2();
            this.oCad = new data.CLcad();
            this.oDs = new DataSet();
        }

        public DataSet afficher(string dataTableName, string nom)
        {
            oDs.Clear();

            if (nom == "*" || nom == "")
            {
                oDs = oCad.getRows(oMap.selectAll(), dataTableName);
            }
            else
            {
                oDs = oCad.getRows(oMap.selectByName(nom), dataTableName);
            }
            return oDs;
        }
    }
}
