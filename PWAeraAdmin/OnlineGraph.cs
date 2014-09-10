using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWAeraAdmin
{
    class OnlineGraph : IDisposable
    {
        public int total = 0;
        public List<Aera.Data> DBRoles = new List<Aera.Data>();

        public OnlineGraph()
        {
            this.DBRoles = new List<Aera.Data>();
        }
        public OnlineGraph(List<Aera.Data> DBRoles)
        {
            this.DBRoles = DBRoles;
        }
        public override string ToString()
        {
            string temp = "";
            foreach (Aera.Data _data in this.DBRoles)
            {
                total++;
                temp += "█";
                Program.f.send(Program.Form.mTotalOnline, ": " + total.ToString());
            }
            return DateTime.Now.ToString() + "\t" + total + " users\t" + temp;
        }

        //Disposing the existing data
        public void DisposeAll()
        {
            DBRoles.Clear();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                DisposeAll();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
