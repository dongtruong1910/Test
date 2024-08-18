using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        { 

            //MonHoc MonHoc1 = new MonHoc();

            //MonHoc1.Nhap();
            //MonHoc1.Xuat();


            DSMH DS1 = new DSMH();
            DS1.NhapDSMH();
            DS1.XuatDSMH();
            Console.ReadKey();//dung man hinh de xem ket qua
        }

        private static void MonHoc(string v1, string v2, int v3)
        {
            throw new NotImplementedException();
        }
    }


}
class MonHoc
{
    private
        string MaMon;
        string TenMon;
        int STC;

    public MonHoc() { }
    public MonHoc(string MaMon, string TenMon, int STC)
    {
        this.MaMon = MaMon;
        this.TenMon = TenMon;
        this.STC = STC;
    }
    public void Nhap()
    {
        Console.WriteLine("Nhap ten mon hoc: ");
        this.TenMon = Console.ReadLine();
        Console.WriteLine("Nhap ma mon hoc: ");
        this.MaMon = Console.ReadLine();
        Console.WriteLine("Nhap so tin chi: ");
        this.STC = int.Parse(Console.ReadLine());

        

    }
    public void Xuat()
    {
        Console.WriteLine(this.getMaMon());
        Console.WriteLine(this.getTenMon());
        Console.WriteLine(this.getSTC());
        
    }

    public
        string getMaMon()
        {
            return MaMon;
        }
        string getTenMon()
        {
            return TenMon;    
        }
    public int getSTC()
    {
        return STC;
    }

    public static implicit operator MonHoc(ArrayList v)
    {
        throw new NotImplementedException();
    }
}

class DSMH
{

    private int soLuong;
    private List<MonHoc> List = new List<MonHoc> ();

    public void NhapDSMH()
    {
        Console.WriteLine("Nhap so luong mon hoc: ");
        this.soLuong = int.Parse(Console.ReadLine());
        Console.WriteLine("Nhap mon hoc: ");
        for (int i = 0; i < this.soLuong; i++)
        {
            MonHoc MH = new MonHoc();
            MH.Nhap();
            this.List.Add(MH);
        }
    }
    public void XuatDSMH()
    {
        Console.Write("\n\nDSMH vua nhap la: \n");
        foreach (MonHoc MH in List)
        {
            MH.Xuat();
        }

    }
}


