using DRL_UTE.Common;
using DRL_UTE.Controllers;
using ModelEF.Dao;
using ModelEF.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using OfficeOpenXml;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace DRL_UTE.Areas.PhongCTSV.Controllers
{
    public class CNLopController : BaseController
    {
        // GET: PhongCTSV/CNLop
        DBContext dbPro = new DBContext();
        LopDao aDB = new LopDao();

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
            var sp = new LopDao();
            var model = sp.ListAllPost(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model.ToPagedList(page, pageSize));
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
        public ActionResult Create(Lop model )
        {
            DateTime now = DateTime.Now;
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            try
            {
                if (model.tenLop == null || model.maLop == null || model.khoaHoc == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                }
                else
                {
                    if (aDB.FindmaLop(model.maLop) == 0)
                    {
                        aDB.Edit(model);
                        SetAlert("Thêm mới thành công", "success");
                        return RedirectToAction("Index", "CNLop");
                    }
                    else
                    {
                        SetAlert("Lớp này đã tồn tại", "error");
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

        [HttpGet]
        public ActionResult Edit(string maLop)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var dao = new LopDao();
            var result = dao.Find(maLop);
            if (result != null)
                return View(result);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Lop model)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            try
            {
                if (model.tenLop == null || model.maLop == null || model.khoaHoc == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                }
                
                else
                {

                    aDB.Edit(model);
                    SetAlert("Chỉnh sửa thành công", "success");
                    return RedirectToAction("Index", "CNLop");
                }
            }
            catch (Exception)
            {
                SetAlert("Cập nhật thất bại", "error");
            }
            return View();
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Server.MapPath("/Doc/FileMau/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }


        public ActionResult Upload()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
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
                            sqlBulkCopy.DestinationTableName = "dbo.Lop";

                            // Map the Excel columns with that of the database table, this is optional but good if you do
                            // 
                            sqlBulkCopy.ColumnMappings.Add("maLop", "maLop");
                            sqlBulkCopy.ColumnMappings.Add("tenLop", "tenLop");
                            sqlBulkCopy.ColumnMappings.Add("khoaHoc", "khoaHoc");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Upload", "CNLop");
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
