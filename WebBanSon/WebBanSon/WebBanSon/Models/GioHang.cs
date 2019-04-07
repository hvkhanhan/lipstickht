using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanSon.Models
{
    public class Giohang
    {
        dbQlBanSonDataContext data = new dbQlBanSonDataContext();
        public int iMason { set; get; }
        public string sTenson { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        //khởi tạo giỏ hàng với số lượng mặc định truyền vào là 1
        public Giohang(int MaSon)
        {
            iMason = MaSon;
            Son son = data.Sons.Single(n => n.MaSon == iMason);
            sTenson = son.TenSon;
            sAnhbia = son.AnhBia;
            dDongia = double.Parse(son.GiaBan.ToString());
            iSoluong = 1;
        }
        
    }
}