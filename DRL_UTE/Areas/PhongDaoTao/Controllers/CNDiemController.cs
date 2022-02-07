using DRL_UTE.Common;
using DRL_UTE.Controllers;
using ModelEF.Dao;
using ModelEF.Model;
using PagedList;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.PhongDaoTao.Controllers
{
    public class CNDiemController : BaseController
    {
        // GET: PhongDoan/CNDiem
        DBContext dbPro = new DBContext();
        DRLDao aDB = new DRLDao();
        int hocKyT = 0;

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
            
            var edit = new DiemEdit();
            edit.hocKy = hocKy;
            Session.Add(SinhVienXN.XN_SESSION, edit);

            var sp = new SinhVienDao();
            var model = sp.ListAllDiem(hocKy);
            return View(model);
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
            var ed = (DiemEdit)Session[SinhVienXN.XN_SESSION];
            ViewBag.hocKy = ed.hocKy;
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

            var ed = (DiemEdit)Session[SinhVienXN.XN_SESSION];
            ViewBag.hocKy = ed.hocKy;

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
                            sqlBulkCopy.DestinationTableName = "dbo.diemTBHocKy";

                            // Map the Excel columns with that of the database table, this is optional but good if you do
                            // 
                            sqlBulkCopy.ColumnMappings.Add("hocKy", "hocKy");
                            sqlBulkCopy.ColumnMappings.Add("maSV", "maSV");
                            sqlBulkCopy.ColumnMappings.Add("diemTB", "diemTB");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("UploadExcel", "CNDiem");
                }
                catch (Exception)
                {
                    SetAlert("Cập nhật thất bại vui lòng kiểm tra lại", "error");
                }
            }
            //if the code reach here means everthing goes fine and excel data is imported into database
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string maSV, int hocKy)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            //Session.Add(SinhVienXN.XN_SESSION, maSV);
            var edit = new DiemEdit();
            edit.maSV = maSV;
            edit.hocKy = hocKy;
            Session.Add(SinhVienXN.XN_SESSION, edit);
            ViewBag.hocKy = hocKy;
            //SetViewBag();
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var dao = new SinhVienDao();
                var result = dao.FindDiem(maSV,hocKy);
                if (result != null)
                    return View(result);
                return View(result);
            }
        }

        
        [HttpPost]
        public ActionResult Edit(diemTBHocKy editPro)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            //SetViewBag();
            //TaiKhoanPB taikhoan = dbPro.TaiKhoanPBs.Find(maTK);

            var ed = (DiemEdit)Session[SinhVienXN.XN_SESSION];
            var maSV = ed.maSV;
            var hocKy = ed.hocKy;
            ViewBag.hocKy = hocKy;

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var dao = new SinhVienDao();
                var sv = dao.FindDiem(editPro.maSV,editPro.hocKy);

                if (editPro.diemTB != null)
                {
                    if (editPro.diemTB >= 0 && editPro.diemTB <= 4)
                    {
                        try
                        {
                            dao.EditDiem(editPro);
                            SetAlert("Cập nhật thành công ", "success");
                            //return RedirectToAction("Index", "CNTaiKhoan");
                            return Redirect("/PhongDaoTao/CNDiem/Details?hocKy=" + hocKy);
                        }
                        catch (Exception)
                        {
                            SetAlert("Cập nhật thất bại", "error");
                            return Redirect("/PhongDaoTao/CNDiem/Edit?maSV=" + maSV + "&hocKy=" + hocKy);
                        }
                        
                    }
                    else
                    {
                        SetAlert("Điểm phải từ 0 tới 4", "error");
                        //return RedirectToAction("Index", "CNTaiKhoan");
                        //return Redirect("/PhongCTSV/CNSinhVien/Edit?maSV=" + ma);
                        return View();
                    }
                } 
                return View();
            }
        }
    }
}