using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSon.Models;
using PagedList.Mvc;
using PagedList;

namespace WebBanSon.Controllers
{
    public class TimKiemController : Controller
    {
        dbQlBanSonDataContext db = new dbQlBanSonDataContext();

        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            List<Son> lstKQTK = db.Sons.Where(n => n.TenSon.Contains(sTuKhoa)).ToList();
             //phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if(lstKQTK.Count==0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào";
                return View(db.Sons.OrderBy(n => n.TenSon).ToPagedList(pageNumber,pageSize));
            }
            ViewBag.Thongbao = "Đã tìm thấy" + lstKQTK.Count + "kết quả! ";

            return View(lstKQTK.OrderBy(n => n.TenSon).ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        public ActionResult KetQuaTimKiem(string sTuKhoa, int? page)
        {
            
            ViewBag.Tukhoa = sTuKhoa;
            List<Son> lstKQTK = db.Sons.Where(n => n.TenSon.Contains(sTuKhoa)).ToList();
            //phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (lstKQTK.Count == 0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào";
                return View(db.Sons.OrderBy(n => n.TenSon).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.Thongbao = "Đã tìm thấy" + lstKQTK.Count + "kết quả! ";

            return View(lstKQTK.OrderBy(n => n.TenSon).ToPagedList(pageNumber, pageSize));
        }
    }
}