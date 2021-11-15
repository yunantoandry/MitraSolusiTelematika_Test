using Common.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Common.Repository
{
    public class Rep_KodePos : Repository, IDisposable
    {
        public Rep_KodePos()
        {

        }
        public Rep_KodePos(DBHelper db)
        {
            this.DBHelper = db;
        }


       
        public List<Kode_Pos> GetAll()
        {
            List<Kode_Pos> list = GetAll(string.Empty);
            return list;
        }

        public List<Kode_Pos> GetAll(string condition)
        {
            List<Kode_Pos> list = null;
            if (string.IsNullOrEmpty(condition))
                list = DBHelper.Connection.Query<Kode_Pos>("Select * From Kode_Pos").ToList();
            else
                list = DBHelper.Connection.Query<Kode_Pos>("Select * From Kode_Pos  " + condition).ToList();

            return list;
        }
        public void LoadGv(GridView gv, string param)
        {
            List<Kode_Pos> list = GetAll(param);
            gv.DataSource = list;
            gv.DataBind();
        }

        public Kode_Pos Find(string condition)
        {
            Kode_Pos _PARAM_LIST_BANK_VERSION = null;
            _PARAM_LIST_BANK_VERSION = DBHelper.Select<Kode_Pos>(condition).SingleOrDefault();
            return _PARAM_LIST_BANK_VERSION;
        }
        public void Insert(int NoKodePos, string Kelurahan, string Kecamatan, string Jenis,string Kabupaten, string Propinsi, string createdBy, string modifiedBy)
        {
            Kode_Pos o = new Kode_Pos();
            //o.ID = ID;
            o.NO_KODE_POS = NoKodePos;
            o.KELURAHAN = Kelurahan;
            o.KECAMATAN = Kecamatan;
            o.JENIS = Jenis;
            o.KABUPATEN = Kabupaten;
            o.PROPINSI = Propinsi;
            o.CREATED_BY = createdBy;
            o.CREATED_DATE = DateTime.Now;
            o.MODIFIED_BY = modifiedBy;
            o.MODIFIED_DATE = DateTime.Now;
            int i = DBHelper.Insert<Kode_Pos>(o);
        }
        public void Update(int id, Kode_Pos obj)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("ID", id);
            int i = DBHelper.Update<Kode_Pos>(obj, args);
        }
        public void Delete(int id)
        {
            int i = DBHelper.Delete<Kode_Pos>(new Kode_Pos { ID = id });
        }
    }
}
