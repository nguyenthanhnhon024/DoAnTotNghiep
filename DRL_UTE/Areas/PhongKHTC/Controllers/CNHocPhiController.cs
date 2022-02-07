using DRL_UTE.Common;
using DRL_UTE.Controllers;
using ModelEF.Dao;
using ModelEF.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.PhongKHTC.Controllers
{
    public class CNHocPhiController : BaseController
    {
        // GET: PhongKHTC/CNHocPhi
        DBContext dbPro = new DBContext();
        DRLDao aDB = new DRLDao();

        [HandleError]
        [HttpGet]
        public ActionResult Index(string error, string name)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
                ViewBag.ProError = error;
            var model = aDB.ListAll();
            return View(model);
        }

        [HandleError]
        [HttpGet]
        public ActionResult Details(int hocKy)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            Session.Add(SinhVienXN.XN_SESSION, hocKy);

            var sp = new HocPhiDao();
            var model = sp.ListAllHocKy(hocKy);
            return View(model);
        }

        public ActionResult Delete(int hocKy, string maSV)
        {

            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var dao = new HocPhiDao().Delete(hocKy, maSV);
            SetAlert("Xóa thành công", "success");
            return Redirect("/PhongKHTC/CNHocPhi/Details?hocKy=" + hocKy);
        }

        [HttpGet]
        public ActionResult AddSV()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            var hocKy = Session[SinhVienXN.XN_SESSION];
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            ViewBag.hocKy = hocKy;
            return View();
        }

        [HttpPost]
        public ActionResult AddSV(HocPhi model)
        {
            DateTime now = DateTime.Now;
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var ss = (int)Session[SinhVienXN.XN_SESSION];
            model.hocKy = ss;
            ViewBag.hocKy = model.hocKy;
            try
            {
                if (model.hocKy == null || model.maSV == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                }
                else
                {
                    var dao = new HocPhiDao();
                    if (dao.Find(model.hocKy, model.maSV.Trim()) == null)
                    {
                        dao.Edit(model);
                        SetAlert("Thêm mới thành công", "success");
                        return View();
                    }
                    else
                    {
                        SetAlert("Sinh viên này đã tồn tại", "error");
                        return View();
                    }

                }
            }
            catch (Exception)
            {
                SetAlert("Thêm mới thất bại", "error");
            }
            return View();
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Server.MapPath("/Doc/FileMau/" + fileName);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
        public ActionResult UploadExcel()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var ss = (int)Session[SinhVienXN.XN_SESSION];

            ViewBag.hocKy = ss;
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            var ss = (int)Session[SinhVienXN.XN_SESSION];
            ViewBag.hocKy = ss;

            string filePath = string.Empty;
            if (file != null)
            {
                try
                {
                    string path = Server.MapPath("~/Doc/FileUpload/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    file.SaveAs(filePath);

                    string conString = string.Empty;

                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();


                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";

                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }

                    conString = @"data source=.\SQLEXPRESS;initial catalog=DRL_UTE;Integrated Security=True";
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "dbo.HocPhi";

                            // Map the Excel columns with that of the database table, this is optional but good if you do
                            // 
                            sqlBulkCopy.ColumnMappings.Add("hocKy", "hocKy");
                            sqlBulkCopy.ColumnMappings.Add("maSV", "maSV");

                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("UploadExcel", "CNHocPhi");
                }
                catch (Exception)
                {
                    SetAlert("Cập nhật thất bại vui lòng kiểm tra lại", "error");
                }
            }
            //if the code reach here means everthing goes fine and excel data is imported into database
            return View();
        }
    }
}