using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSon.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebBanSon.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQlBanSonDataContext db = new dbQlBanSonDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Son(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.Sons.ToList());
            return View(db.Sons.ToList().OrderBy(n => n.MaSon).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        { return View(); }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["loi1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (admin)        

                Admin ad = db.Admins.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Themmoison()
        {
            // đưa DL vào droplist
            // lấy DS từ table loại son , sắp xếp tăng dần theo loại son, chọn giá trị mã loại son , hiển thì tên loai son
            ViewBag.MaLoaiSon = new SelectList(db.LoaiSons.ToList().OrderBy(n => n.TenLoaiSon), "MaLoaiSon", "TenLoaiSon");
            ViewBag.MaNsx = new SelectList(db.HangSanXuats.ToList().OrderBy(n => n.TenNsx), "MaNsx", "TenNsx");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoison(Son son, HttpPostedFileBase fileupload)
        {
            // drop
            ViewBag.MaLoaiSon = new SelectList(db.LoaiSons.ToList().OrderBy(n => n.TenLoaiSon), "MaLoaiSon", "TenLoaiSon");
            ViewBag.MaNsx = new SelectList(db.HangSanXuats.ToList().OrderBy(n => n.TenNsx), "MaNsx", "TenNsx");
            //test duonng dan
            if (fileupload == null)
            {
                ViewBag.Thongbao = " Vui long chon anh bia";
                return View();
            }
            // them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //lưu tên file
                    var filename = Path.GetFileName(fileupload.FileName);
                    // lưu đường dẫn
                    var path = Path.Combine(Server.MapPath("~/HinhSP"), filename);
                    //kiểm tra hình ảnh đã tồn tại chưa
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //lưu hình ảnh vào đường dẫn
                        fileupload.SaveAs(path);
                    }
                    son.AnhBia = filename;
                    db.Sons.InsertOnSubmit(son);
                    db.SubmitChanges();
                }
                return RedirectToAction("Son");
            }
        }
        public ActionResult Chitietson(int id)
        {
            // lấy son theo mã
            Son son = db.Sons.SingleOrDefault(n => n.MaSon == id);
            ViewBag.MaSon = son.MaSon;
            if ( son==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(son);
        }
        [HttpGet]
        public ActionResult Xoason(int id)
        {
            // lấy son theo mã
            Son son = db.Sons.SingleOrDefault(n => n.MaSon == id);
            ViewBag.MaSon = son.MaSon;
            if (son == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(son);
        }

        [HttpPost, ActionName("Xoason")]

        public ActionResult Xacnhanxoa(int id)
        {
            Son son = db.Sons.SingleOrDefault(n => n.MaSon == id);
            ViewBag.MaSon = son.MaSon;
            if (son == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            db.Sons.DeleteOnSubmit(son);
            db.SubmitChanges();
            return RedirectToAction("Son");
        }

        [HttpGet]
        public ActionResult Sua(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            var son = db.Sons.SingleOrDefault(x => x.MaSon == id);
            ViewBag.MaLoaiSon = new SelectList(db.Sons.ToList().OrderBy(n => n.TenSon), "MaLoaiSon", "TenSon");
            ViewBag.MaNsx = new SelectList(db.HangSanXuats.ToList().OrderBy(n => n.TenNsx), "MaNsx", "TenNsx");
            return View(son);
        }
        [HttpPost]
        [ValidateInput(false)]
        //public ActionResult Sua(QUANAO quanao, FormCollection collection)
        //public ActionResult Sua(int id, HttpPostedFileBase fileUpload)
        public ActionResult Sua(int id, FormCollection collection)
        {
            ViewBag.MaLoaiSon = new SelectList(db.LoaiSons.ToList().OrderBy(n => n.TenLoaiSon), "MaLoaiSon", "TenLoaiSon");
            ViewBag.MaNsx = new SelectList(db.HangSanXuats.ToList().OrderBy(n => n.TenNsx), "MaNsx", "TenNsx");
            try
            {
                //Lấy giá trị ở Form EditProduct
                string _TenSP = collection["txt_TenSP"];
                string _UrlHinh = collection["txt_UrlHinh"];
                decimal _GiaCu = decimal.Parse(collection["txt_GiaCu"]);
                string _MoTa = collection["txt_MoTa"];
                int _MaNSX = int.Parse(collection["MaNSX"]);
                int _MaLoaiSon = int.Parse(collection["MaLoaiSon"]);
                int _SoLuongTon = int.Parse(collection["txt_SLTon"]);
                DateTime _NgayCapNhat = Convert.ToDateTime(collection["txt_NgayCapNhat"]);

                //Lấy ra thông tin Sản phẩm từ MaSP truyền vào
                var sp = db.Sons.First(s => s.MaSon == id);

                //Gán giá trị để chỉnh sửa
                sp.TenSon = _TenSP;
                sp.AnhBia = _UrlHinh;
                sp.GiaBan = _GiaCu;
                sp.MoTa = _MoTa;
                sp.MaNsx = _MaNSX;
                sp.MaLoaiSon = _MaLoaiSon;
                sp.SoLuongTon = _SoLuongTon;
                sp.NgayCapNhat = _NgayCapNhat;
                UpdateModel(sp);
                db.SubmitChanges();
                return Content("<script>alert('Chỉnh sửa thành công!');window.location='/Admin/Son';</script>");
            }
            catch
            {
                return Content("<script>alert('Lỗi hệ thống!');window.location='/Admin/Son';</script>");
            }            
        }
        public ActionResult LoaiSon()
        {
            return View(db.LoaiSons.ToList());

        }


        public ActionResult KhachHang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.KhachHangs.ToList().OrderBy(n => n.MaKhachHang).ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult themKH()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themKH(KhachHang khachhang)
        {
            db.KhachHangs.InsertOnSubmit(khachhang);
            db.SubmitChanges();
            return RedirectToAction("KhachHang");
        }

        [HttpGet]
        public ActionResult suaKH(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            var khachhang = db.KhachHangs.SingleOrDefault(x => x.MaKhachHang == id);
            return View(khachhang);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult suaKH(int id, FormCollection collection)
        {
            try
            {
                //Lấy giá trị ở Form EditProduct
                string _TenKhachHang = collection["txt_TenKhachHang"];


                //Lấy ra thông tin Sản phẩm từ MaSP truyền vào
                var kh = db.KhachHangs.First(s => s.MaKhachHang == id);

                //Gán giá trị để chỉnh sửa
                kh.TenKhachHang = _TenKhachHang;

                UpdateModel(kh);
                db.SubmitChanges();
                return Content("<script>alert('Chỉnh sửa thành công!');window.location='/Admin/KhachHang';</script>");
            }
            catch
            {
                return Content("<script>alert('Lỗi hệ thống!');window.location='/Admin/KhachHang';</script>");
            }
        }

        public ActionResult xoaKH(int id)
        {
            KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKhachHang == id);
            ViewBag.MaKhachHang = khachhang.MaKhachHang;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(khachhang);
        }

        [HttpPost, ActionName("xoaKH")]

        public ActionResult xacnhanxoakhachhang(int id)
        {
            KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKhachHang == id);
            ViewBag.MaKhachHang = khachhang.MaKhachHang;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            db.KhachHangs.DeleteOnSubmit(khachhang);
            db.SubmitChanges();
            return RedirectToAction("KhachHang");
        }


        public ActionResult HangSanXuat()
        {
            return View(db.HangSanXuats.ToList());

        }


        [HttpGet]
        public ActionResult themloaison()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themloaison(LoaiSon loaison)
        {
            db.LoaiSons.InsertOnSubmit(loaison);
            db.SubmitChanges();
            return RedirectToAction("Loaison");
        }
        [HttpGet]
        public ActionResult sualoaison(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            var son = db.LoaiSons.SingleOrDefault(x => x.MaLoaiSon == id);
            return View(son);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult sualoaison(int id, FormCollection collection)
        {
            try
            {
                //Lấy giá trị ở Form EditProduct
                string _TenSP = collection["txt_TenSP"];


                //Lấy ra thông tin Sản phẩm từ MaSP truyền vào
                var sp = db.LoaiSons.First(s => s.MaLoaiSon == id);

                //Gán giá trị để chỉnh sửa
                sp.TenLoaiSon = _TenSP;

                UpdateModel(sp);
                db.SubmitChanges();
                return Content("<script>alert('Chỉnh sửa thành công!');window.location='/Admin/Son';</script>");
            }
            catch
            {
                return Content("<script>alert('Lỗi hệ thống!');window.location='/Admin/Son';</script>");
            }
        }
        public ActionResult xoaloaison(int id)
        {
            LoaiSon loaison = db.LoaiSons.SingleOrDefault(n => n.MaLoaiSon == id);
            ViewBag.MaLoaiSon = loaison.MaLoaiSon;
            if (loaison == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(loaison);
        }

        [HttpPost, ActionName("xoaloaison")]

        public ActionResult xacnhanxoaloaison(int id)
        {
            LoaiSon loaison = db.LoaiSons.SingleOrDefault(n => n.MaLoaiSon == id);
            ViewBag.MaLoaiSon = loaison.MaLoaiSon;
            if (loaison == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            db.LoaiSons.DeleteOnSubmit(loaison);
            db.SubmitChanges();
            return RedirectToAction("LoaiSon");
        }
        [HttpGet]
        public ActionResult themhsx()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themhsx(HangSanXuat hsx)
        {
            db.HangSanXuats.InsertOnSubmit(hsx);
            db.SubmitChanges();
            return RedirectToAction("HangSanXuat");
        }
        [HttpGet]
        public ActionResult suahsx(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            var son = db.HangSanXuats.SingleOrDefault(x => x.MaNsx == id);
            return View(son);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult suahsx(int id, FormCollection collection)
        {
            try
            {
                //Lấy giá trị ở Form EditProduct
                string _TenSP = collection["txt_TenSP"];
                string _GiaCu = collection["txt_GiaCu"];
                string _MoTa = collection["txt_MoTa"];

                //Lấy ra thông tin Sản phẩm từ MaSP truyền vào
                var sp = db.HangSanXuats.First(s => s.MaNsx == id);

                //Gán giá trị để chỉnh sửa
                sp.TenNsx = _TenSP;
                sp.DiaChi = _GiaCu;
                sp.DienThoai = _MoTa;
                UpdateModel(sp);
                db.SubmitChanges();
                return Content("<script>alert('Chỉnh sửa thành công!');window.location='/Admin/HangSanXuat';</script>");
            }
            catch
            {
                return Content("<script>alert('Lỗi hệ thống!');window.location='/Admin/HangSanXuat';</script>");
            }

        }
        public ActionResult xoahsx(int id)
        {
            HangSanXuat hsx = db.HangSanXuats.SingleOrDefault(n => n.MaNsx == id);
            ViewBag.MaNsx = hsx.MaNsx;
            if (hsx == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(hsx);
        }

        [HttpPost, ActionName("xoahsx")]

        public ActionResult xacnhanxoahsx(int id)
        {
            HangSanXuat hsx = db.HangSanXuats.SingleOrDefault(n => n.MaNsx == id);
            ViewBag.MaNsx = hsx.MaNsx;
            if (hsx == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            db.HangSanXuats.DeleteOnSubmit(hsx);
            db.SubmitChanges();
            return RedirectToAction("HangSanXuat");
        }
        public ActionResult Chitiethsx(int id)
        {
            HangSanXuat hsx = db.HangSanXuats.SingleOrDefault(n => n.MaNsx == id);
            ViewBag.MaNsx = hsx.MaNsx;
            if (hsx == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(hsx);
        }


        public ActionResult DonDatHang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.DonDatHangs.ToList().OrderBy(n => n.SoHd).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult themddh()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themddh(DonDatHang dondathang)
        {
            db.DonDatHangs.InsertOnSubmit(dondathang);
            db.SubmitChanges();
            return RedirectToAction("DonDatHang");
        }

        

        public ActionResult xoaddh(int id)
        {
            DonDatHang dondathang = db.DonDatHangs.SingleOrDefault(n => n.SoHd == id);
            ViewBag.SoHd = dondathang.SoHd;
            if (dondathang == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(dondathang);
        }

        [HttpPost, ActionName("xoaddh")]

        public ActionResult xacnhanxoadondathang(int id)
        {
            DonDatHang dondathang = db.DonDatHangs.SingleOrDefault(n => n.SoHd == id);
            ViewBag.SoHd = dondathang.SoHd;
            if (dondathang == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            db.DonDatHangs.DeleteOnSubmit(dondathang);
            db.SubmitChanges();
            return RedirectToAction("DonDatHang");
        }
    }
}
              
    