using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSon.Models;
using PagedList;
using PagedList.Mvc;

namespace WebBanSon.Controllers
{
    public class SonStoreController : Controller
    {
        //tao doi tuong
        dbQlBanSonDataContext data = new dbQlBanSonDataContext();
        // GET: ShopSon

        private List<Son> laysonmoi(int count)
        {

            return data.Sons.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList(); //lấy danh sách son
        }
        public ActionResult Index(int? page)
        {
            int pageSize = 5; //tạo biến quy định số sp trên mỗi trang
            int pageNum = (page ?? 1);// tạo biến số trang
            var sonmoi = laysonmoi(15);//update 5 son mới cập nhật
            return View(sonmoi.ToPagedList(pageNum, pageSize));
        }

       
         public ActionResult LoaiSon()
        {
            var loaison = from cd in data.LoaiSons 
                          select cd;
            return PartialView(loaison);
        }
        public ActionResult HangSanXuat()
        {
            var hangsx = from hsx in data.HangSanXuats 
                         select hsx;
            return PartialView(hangsx);
        }
        public ActionResult SPTheoLoai(int id)
        {
            var son = from g in data.Sons 
                      where g.MaLoaiSon == id 
                      select g;
            return View(son);

        }
        public ActionResult SPTheoHsx(int id)
        {
            var son = from g in data.Sons 
                      where g.MaNsx == id 
                      select g;
            return View(son);

        }
        public ActionResult Details(int id)
        {
            var son = from g in data.Sons 
                      where g.MaSon == id 
                      select g;
            return View(son.Single());
        }
        public ActionResult Contact()
        {
            return View();
        }
       
    }
}
