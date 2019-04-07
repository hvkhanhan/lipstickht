using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSon.Models;


namespace WebApplication1.Controllers
{
    public class NguoiDungController : Controller
    {
        dbQlBanSonDataContext db = new dbQlBanSonDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Gioithieu()
        {
            return View();
        }
        public ActionResult tintuc()
        {
            return View();
        }
        public ActionResult album()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKi(FormCollection collection, KhachHang kh)
        {
            var ten = collection["Tenkhachhang"];
            var tendn = collection["Tendangnhap"];
            var mk = collection["MatKhau"];
            var nlmk = collection["nhaplaimatkhau"];
            var em = collection["email"];
            var dt = collection["sodienthoai"];
            var dc = collection["diachi"];
            var ns = String.Format("{0:MM/dd/yyyy }", collection["Ngaysinh"]);
            if (string.IsNullOrEmpty(ten))
            {
                ViewData["Loi1"] = "Không Được Bỏ Trống Mục Này";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Không Được Bỏ Trống Mục Này";
            }
            else if (string.IsNullOrEmpty(mk))
            {
                ViewData["Loi3"] = "Không Được Bỏ Trống Mục Này";
            }
            else if (string.IsNullOrEmpty(nlmk))
            {
                ViewData["Loi4"] = "Không Được Bỏ Trống Mục Này";
            }
            if (string.IsNullOrEmpty(em))
            {
                ViewData["Loi5"] = "Không Được Bỏ Trống Mục Này";
            }
            if (string.IsNullOrEmpty(dt))
            {
                ViewData["Loi6"] = "Không Được Bỏ Trống Mục Này";
            }
            if (string.IsNullOrEmpty(dc))
            {
                ViewData["Loi7"] = "Không Được Bỏ Trống Mục Này";
            }
            else
            {
                kh.TenKhachHang = ten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = mk;
                kh.Email = em;
                kh.DiaChi = dc;
                kh.DT = dt;
                kh.NgaySinh = DateTime.Parse(ns);

                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");


            }
            return this.DangKi();
        }
         [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap (FormCollection collection, KhachHang kh)
        {

            var tendn = collection["Tendn"];
            var mk = collection["MatKhau"];


            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Không Được Bỏ Trống Mục Này";
            }
            else if (string.IsNullOrEmpty(mk))
            {
                ViewData["Loi2"] = "Không Được Bỏ Trống Mục Này";
            }

            else
            {

                kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == mk);

                if (kh != null)
                {
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Giohang", "Giohang");
                }
                else
                    ViewBag.Thongbao = "Tài Khoản hoặc Mật Khẩu Sai";

            }
            return View();
        }
        
    }
}


