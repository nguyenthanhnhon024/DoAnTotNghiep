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

namespace DRL_UTE.Areas.PhongDoan.Controllers
{
    public class CNHoatDongController : BaseController
    {
        // GET: PhongDoan/CNHoatDong
        DBContext dbPro = new DBContext();
        HoatDongDao aDB = new HoatDongDao();

        [HandleError]
        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = 6)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var model = aDB.ListAll();
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 6)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            var model = aDB.ListAllPost(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpGet]
        public ActionResult Edit(int idHD)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            var dao = new HoatDongDao();
            var result = dao.Find(idHD);
            if (result != null)
                return View(result);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(HoatDong model)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            try
            {
                if (model.tenHD == null || model.hocKy == null || model.noiDungHD == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                }

                else
                {
                    var dao = new HoatDongDao();
                    dao.Edit(model);
                    SetAlert("Chỉnh sửa thành công", "success");
                    return RedirectToAction("Index", "CNHoatDong");
                }
            }
            catch (Exception)
            {
                SetAlert("Cập nhật thất bại", "error");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(HoatDong model)
        {
            DateTime now = DateTime.Now;
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            try
            {
                if (model.tenHD == null || model.hocKy == null || model.noiDungHD == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                }
                else
                {
                        aDB.Edit(model);
                        SetAlert("Thêm mới thành công", "success");
                        return RedirectToAction("Index", "CNHoatDong");
                }
            }
            catch (Exception)
            {
                SetAlert("Thêm mới thất bại vui lòng kiểm tra lại mã sinh vien", "error");
            }
            return View();
        }

        [HandleError]
        [HttpGet]
        public ActionResult Details(int idHD)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            Session.Add(SinhVienXN.XN_SESSION, idHD);

            var sp = new HoatDongDao();
            var model = sp.ListAllDiem(idHD);
            return View(model);
        }

        public ActionResult Delete(int idHD, string maSV)
        {

            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var dao = new HoatDongDao().Delete(idHD,maSV);
            SetAlert("Xóa thành công", "success");
            return Redirect("/PhongDoan/CNHoatDong/Details?idHD=" + idHD);
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

            var idHD = Session[SinhVienXN.XN_SESSION];
            ViewBag.idHD = idHD;
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

            var idHD = Session[SinhVienXN.XN_SESSION];
            ViewBag.idHD = idHD;

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
                            sqlBulkCopy.DestinationTableName = "dbo.SV_HD";

                            // Map the Excel columns with that of the database table, this is optional but good if you do
                            // 
                            sqlBulkCopy.ColumnMappings.Add("idHD", "idHD");
                            sqlBulkCopy.ColumnMappings.Add("maSV", "maSV");
                            
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("UploadExcel", "CNHoatDong");
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
        public ActionResult AddSV()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            var idHD =Session[SinhVienXN.XN_SESSION];
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            ViewBag.idHD = idHD;
            return View();
        }

        [HttpPost]
        public ActionResult AddSV(SV_HD model)
        {
            DateTime now = DateTime.Now;
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var ss = (int)Session[SinhVienXN.XN_SESSION];
            model.idHD = ss;
            ViewBag.idHD = model.idHD;
            try
            {
                if (model.idHD == null || model.maSV == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                }
                else
                {
                    if (aDB.FindSV_HD(model.idHD,model.maSV.Trim()) == null)
                    {
                        aDB.Edit_SVHD(model);
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
    }
}