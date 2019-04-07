using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSon.Models;


namespace WebBanSon.Controllers
{
    public class GiohangController : Controller
    {
        dbQlBanSonDataContext data = new dbQlBanSonDataContext();
        public List<Giohang> laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;

            }
            return lstGiohang;
        }
        //them hang gio
        public ActionResult Themgiohang(int iMaSon, string strURL)
        {
            List<Giohang> lstGiohang = laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.iMason == iMaSon);

            if (sanpham == null)
            {
                sanpham = new Giohang(iMaSon);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);

            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int Tongsoluong()
        {
            int iTongsoluong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongsoluong = lstGiohang.Sum(n => n.iSoluong);


            }
            return iTongsoluong;
        }
        private Double Tongtien()
        {
            Double iTongtien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongtien = lstGiohang.Sum(n => n.dThanhtien);


            }
            return iTongtien;
        }
        // GET: Giohang
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SonStore");

            }
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();



            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();
            return PartialView();

        }
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMason == iMaSP);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMason == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SonStore");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMason == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }

            return RedirectToAction("GioHang");
        }
        public ActionResult XoaHetGiohang()
        {
            List<Giohang> lstGiohang = laygiohang();
            lstGiohang.Clear();

            return RedirectToAction("Index", "SonStore");


        }
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");


            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "SonStore");

            }
            List<Giohang> lstGiohang = laygiohang();
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();



            return View(lstGiohang);
        }

        public ActionResult Dathang(FormCollection collection)
        {
            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<Giohang> gh = laygiohang();
            ddh.MaKhachHang = kh.MaKhachHang;
            ddh.NgayDh = DateTime.Now;
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.SoHd = ddh.SoHd;
                ctdh.MaSon = item.iMason;
                ctdh.SoLuong = item.iSoluong;
                ctdh.DonGia = (decimal)item.dDongia;
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("xacnhandonhang", "Giohang");
        }
        public ActionResult xacnhandonhang()
        {
            return View();
        }

    }
}