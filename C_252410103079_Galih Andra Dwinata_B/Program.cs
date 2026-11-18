using System;
using System.Collections.Generic;

public class Agen
{
    public string idAgen;
    public string namaAgen;
    public int stokAgen;

    public Agen(string idAgen, string namaAgen, int stokAgen)
    {
        this.idAgen = idAgen;
        this.namaAgen = namaAgen;
        this.stokAgen = stokAgen;
    }

    public void TampilInfo()
    {
        Console.WriteLine("ID Agen    : " + idAgen);
        Console.WriteLine("Nama Agen  : " + namaAgen);
        Console.WriteLine("Stok Agen  : " + stokAgen);
    }

    public void TambahStok(int jumlah)
    {
        stokAgen += jumlah;
    }
}

public abstract class ProdukGalon
{
    public string idProduk { get; private set; }
    public string namaProduk { get; private set; }

    public int stokGudang { get; private set; }

    public ProdukGalon(string idProduk, string namaProduk, int stokGudang)
    {
        if (stokGudang < 0)
        {
            throw new ArgumentException("Stok tidak boleh negatif!");
        }

        this.idProduk = idProduk;
        this.namaProduk = namaProduk;
        this.stokGudang = stokGudang;
    }

    public void KurangiStok(int jumlah)
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("Jumlah pengurangan harus lebih dari 0!");
        }
        else if (jumlah > stokGudang)
        {
            Console.WriteLine("Stok gudang tidak mencukupi!");
        }
        else
        {
            stokGudang -= jumlah;
            Console.WriteLine("Stok gudang berhasil dikurangi.");
        }
    }

    public abstract void HitungBiaya(int jumlah);

    public void TampilProduk()
    {
        Console.WriteLine("ID Produk   : " + idProduk);
        Console.WriteLine("Nama Produk : " + namaProduk);
        Console.WriteLine("Stok Gudang : " + stokGudang);
    }
}

public class GalonAkua : ProdukGalon
{
    public GalonAkua(string idProduk, string namaProduk, int stokGudang)
        : base(idProduk, namaProduk, stokGudang)
    {
    }

    public override void HitungBiaya(int jumlah)
    {
        int total = jumlah * 20000;
        Console.WriteLine("Total biaya Galon Akua: Rp" + total);
    }
}

public class GalonBerkeran : ProdukGalon
{
    public string jenisKeran { get; private set; }

    public GalonBerkeran(string idProduk, string namaProduk,
        int stokGudang, string jenisKeran)
        : base(idProduk, namaProduk, stokGudang)
    {
        this.jenisKeran = jenisKeran;
    }

    public override void HitungBiaya(int jumlah)
    {
        int total = jumlah * 12000;
        Console.WriteLine("Total biaya Galon Berkeran: Rp" + total);
    }
}

public interface DapatDidistribusikan
{
    void KirimPaket();
}

public class DistribusiGalon : DapatDidistribusikan
{
    private Agen agenTujuan;
    private ProdukGalon produk;
    private int jumlah;

    public DistribusiGalon(Agen agenTujuan, ProdukGalon produk, int jumlah)
    {
        this.agenTujuan = agenTujuan;
        this.produk = produk;
        this.jumlah = jumlah;
    }

    public void KirimPaket()
    {
        Console.WriteLine("\n===== PROSES DISTRIBUSI =====");

        if (jumlah <= 0)
        {
            Console.WriteLine("Pengiriman gagal: Jumlah harus lebih dari 0!");
            return;
        }

        if (jumlah > produk.stokGudang)
        {
            Console.WriteLine("Pengiriman gagal: Stok gudang tidak mencukupi!");
            return;
        }

        produk.KurangiStok(jumlah);
        agenTujuan.TambahStok(jumlah);

        Console.WriteLine("Paket berhasil dikirimkan ke agen "
            + agenTujuan.namaAgen);
        Console.WriteLine("Produk       : " + produk.namaProduk);
        Console.WriteLine("Jumlah Kirim : " + jumlah);
        Console.WriteLine("Sisa Gudang  : " + produk.stokGudang);
        Console.WriteLine("Stok Agen    : " + agenTujuan.stokAgen);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("SISTEM MANAJEMEN GALON");

        Agen agen1 = new Agen("001", "Budi", 10);
        Agen agen2 = new Agen("002", "Andi", 5);

        Console.WriteLine("\n===== INFORMASI AGEN =====");

        agen1.TampilInfo();
        Console.WriteLine();

        agen2.TampilInfo();

        GalonAkua akua = new GalonAkua("P001", "Galon Akua", 100);

        GalonBerkeran keran = new GalonBerkeran(
            "P002", "Galon Berkeran", 50, "Keran Putar"
        );

        Console.WriteLine("\n===== PERHITUNGAN BIAYA =====");

        ProdukGalon produk1 = akua;
        ProdukGalon produk2 = keran;

        produk1.HitungBiaya(3);
        produk2.HitungBiaya(2);

        DistribusiGalon distribusi1 =
            new DistribusiGalon(agen1, akua, 20);

        distribusi1.KirimPaket();

        DistribusiGalon distribusi2 =
            new DistribusiGalon(agen2, keran, 10);

        distribusi2.KirimPaket();
    }
}


