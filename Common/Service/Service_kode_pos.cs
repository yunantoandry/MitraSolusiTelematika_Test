using Common.Model;
using Common.Repository;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Common.Service
{
    public class Service_kode_pos
    {
        Rep_KodePos rep_KodePos = null;
        public void LoadGv(GridView gv, string param)
        {
            using (DBHelper db = new DBHelper(utils.GetConnectionString()))
            {
                rep_KodePos = new Rep_KodePos(db);
                List<Kode_Pos> list = rep_KodePos.GetAll(param);
                gv.DataSource = list;
                gv.DataBind();
            }
        }
        public Kode_Pos Find(string condition)
        {
            using (DBHelper db = new DBHelper(utils.GetConnectionString()))
            {
                rep_KodePos = new Rep_KodePos(db);
                return rep_KodePos.Find(condition);
            }
        }
        public void Insert(int NoKodePos, string Kelurahan, string Kecamatan, string Jenis, string Kabupaten, string Propinsi, string createdBy, string modifiedBy)
        {
            using (DBHelper db = new DBHelper(utils.GetConnectionString()))
            {
                rep_KodePos = new Rep_KodePos(db);
                rep_KodePos.Insert(NoKodePos, Kelurahan, Kecamatan, Jenis, Kabupaten, Propinsi, createdBy, modifiedBy);
            }
        }
        public void Update(int id, Kode_Pos obj)
        {
            using (DBHelper db = new DBHelper(utils.GetConnectionString()))
            {
                rep_KodePos = new Rep_KodePos(db);
                rep_KodePos.Update(id, obj);
            }
        }
        public void Delete(int id)
        {
            using (DBHelper db = new DBHelper(utils.GetConnectionString()))
            {
                rep_KodePos = new Rep_KodePos(db);
                rep_KodePos.Delete(id);
            }
        }
    }
}
