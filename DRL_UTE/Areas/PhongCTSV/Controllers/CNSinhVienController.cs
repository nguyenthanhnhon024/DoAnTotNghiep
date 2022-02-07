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

using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.PhongCTSV.Controllers
{
    public class CNSinhVienController : BaseController
    {
        // GET: PhongCTSV/CNSinhVien
        DBContext dbPro = new DBContext();
        SinhVienDao aDB = new SinhVienDao();

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
            var sp = new SinhVienDao();
            var model = sp.ListAllPost(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var result = new SinhVienDao().ChangeStatus(id.Trim());
            return Json(new { status = result });
        }

        [HandleError]
        [HttpGet]
        public ActionResult Details(string maSV)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var dao = new SinhVienDao();
                var result = dao.Find(maSV);
                if (result != null)
                    return View(result);
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(string maSV)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            Session.Add(SinhVienXN.XN_SESSION, maSV);
            //SetViewBag();
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                tblSinhVien sv = dbPro.tblSinhViens.Find(maSV);
                var dao = new SinhVienDao();
                var result = dao.Find(maSV);
                ViewBag.maLop = new SelectList(dbPro.Lops, "maLop", "maLop", sv.maLop);
                if (result != null)
                    return View(result);
                return View(result);
            }
        }

        private static bool IsNumber(string val)
        {
            if (val != "")
                return Regex.IsMatch(val, @"^[0-9]\d*\.?[0]*$");
            else return true;
        }

        [HttpPost]
        public ActionResult Edit(tblSinhVien editPro)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            //SetViewBag();
            //TaiKhoanPB taikhoan = dbPro.TaiKhoanPBs.Find(maTK);

            var ma = Session[SinhVienXN.XN_SESSION];

            ViewBag.maLop = new SelectList(dbPro.Lops, "maLop", "maLop");
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var sv = aDB.Find(editPro.maSV);

                if (editPro.SDT != null)
                {
                    editPro.SDT = editPro.SDT.Trim();
                    if (IsNumber(editPro.SDT) != true)
                    {
                        SetAlert("Vui lòng nhập số có 10 hoặc 11 kí tự", "error");
                        //return RedirectToAction("Index", "CNTaiKhoan");
                        return Redirect("/PhongCTSV/CNSinhVien/Edit?maSV=" + ma);
                    }
                    if (editPro.SDT.Length == 10 || editPro.SDT.Length == 11)
                    {
                        try
                        {
                            editPro.avatar = sv.avatar;
                            aDB.EditSV(editPro);
                            SetAlert("Cập nhật thành công ", "success");
                            return RedirectToAction("Index", "CNSinhVien");
                            //return Redirect("/PhongCTSV/CNSinhVien/Edit?maSV=" + ma);
                        }
                        catch (Exception)
                        {
                            SetAlert("Cập nhật thất bại", "error");
                            return Redirect("/PhongCTSV/CNSinhVien/Edit?maSV=" + ma);
                        }
                    }
                    else
                    {
                        SetAlert("Vui lòng nhập số có 10 hoặc 11 kí tự", "error");
                        //return RedirectToAction("Index", "CNTaiKhoan");
                        return Redirect("/PhongCTSV/CNSinhVien/Edit?maSV=" + ma);
                    }
                }
                else
                {
                    try
                    {
                        editPro.avatar = sv.avatar;
                        aDB.EditSV(editPro);
                        SetAlert("Cập nhật thành công ", "success");
                        return RedirectToAction("Index", "CNSinhVien");
                        //return Redirect("/PhongCTSV/CNSinhVien/Edit?maSV=" + ma);
                    }
                    catch (Exception)
                    {
                        SetAlert("Cập nhật thất bại", "error");
                        return Redirect("/PhongCTSV/CNSinhVien/Edit?maSV=" + ma);
                    }
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            ViewBag.maLop = new SelectList(dbPro.Lops, "maLop", "maLop");
            return View();
        }

        [HttpPost]
        public ActionResult Create(tblSinhVien model)
        {
            DateTime now = DateTime.Now;
            var session = (UserLogin)Session[Constants.USER_SESSION];
            ViewBag.maLop = new SelectList(dbPro.Lops, "maLop", "maLop");

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            try
            {
                if (model.maSV == null || model.tenSV == null || model.maLop == null || model.chucVu == "Other" || model.gioiTinh == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                }
                else
                {
                    if (aDB.FindmaSV(model.maSV) == 0)
                    {
                        model.ttHoc = 1;
                        model.matKhau = "123456";
                        aDB.EditSV(model);
                        SetAlert("Thêm mới thành công", "success");
                        return RedirectToAction("Index", "CNSinhVien");
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
                            sqlBulkCopy.DestinationTableName = "dbo.tblSinhVien";

                            // Map the Excel columns with that of the database table, this is optional but good if you do
                            // 
                            sqlBulkCopy.ColumnMappings.Add("maSV", "maSV");
                            sqlBulkCopy.ColumnMappings.Add("tenSV", "tenSV");
                            sqlBulkCopy.ColumnMappings.Add("maLop", "maLop");
                            sqlBulkCopy.ColumnMappings.Add("gioiTinh", "gioiTinh");
                            
                            sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                            sqlBulkCopy.ColumnMappings.Add("SDT", "SDT");

                            sqlBulkCopy.ColumnMappings.Add("ngaySinh", "ngaySinh");
                            sqlBulkCopy.ColumnMappings.Add("diaChi", "diaChi");
                            sqlBulkCopy.ColumnMappings.Add("chucVu", "chucVu");
                            sqlBulkCopy.ColumnMappings.Add("ttHoc", "ttHoc");
                            sqlBulkCopy.ColumnMappings.Add("matKhau", "matKhau");
                            sqlBulkCopy.ColumnMappings.Add("avatar", "avatar");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("UploadExcel", "CNSinhVien");
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