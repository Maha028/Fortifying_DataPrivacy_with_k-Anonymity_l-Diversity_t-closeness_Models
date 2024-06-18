using System.Collections.Generic;
using System.Data;
namespace DataArmor
{
    internal class De_identify
    {
        public List<string> EID;
        public DataTable data=new DataTable();
        public De_identify(DataTable dt,List<string> eID) { 
            EID=eID;
            this.data=dt;
            foreach (var e in EID) {this.data.Columns.Remove(e);}
            ClassificationPage.EID.Clear();
        }

    }
}
