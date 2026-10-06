// File ini berisi TODO -- lihat SOAL.md untuk kontrak lengkap tiap level.
// Bagian bertanda TODO(Level N) adalah tugas kalian; kode lain sudah
// disediakan. Jangan mengubah nama/tipe yang sudah ada kecuali TODO
// memintanya secara eksplisit.

namespace Pertemuan04;

public class Buku
{
    // TODO(Level 1): field PUBLIK di bawah ini melanggar enkapsulasi (siapa pun
    //   bisa mengubahnya sembarangan). Jadikan field PRIVATE (awali nama dengan
    //   _) lalu ekspos lewat properti read-only: public get, tanpa setter
    //   publik. Nama properti tetap Isbn, Judul, StokTotal, StokTersedia.

    //SAY : variabel asli diubah jadi private pake "_" di depannya
    //supaya gabisa di assign sembarangan dari luar (misal: buku.StokTersedia = -999)
    private string _isbn = "";
    private string _judul = "";
    private int _stokTotal;
    private int _stokTersedia;

    // TODO(Level 8): properti di bawah ini menerima nilai apa saja. Beri nilai
    //   awal 7 dan tambahkan logika validasi di accessor set (perlu field
    //   pendukung): nilai harus 1..30, di luar itu lempar
    //   ArgumentOutOfRangeException dan JANGAN mengubah nilai lama.

    //SAY : karna butuh validasi saat diset ulang, kita biki backing field (_batasHariPinjam) dg nilai awal 7
    //di setnya kita cek, kalo diluar rentang 1-30 langsung lempar EROR, jd nilainya aman
    private int _batasHariPinjam = 7;  // level 8 : niali awal 7 hari
     public int BatasHariPinjam 
    { 
        get => _batasHariPinjam;
        set
        {
            if (value < 1 || value > 30)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Batas hari pinjam harus antara 1 sampai 30 hari");
            }
            _batasHariPinjam = value;
        }
    }


    // TODO(Level 2): validasi di AWAL konstruktor -- judul null/kosong/spasi
    //   saja atau stokTotal negatif -> lempar ArgumentException
    //   (ArgumentOutOfRangeException juga boleh); jangan ada state yang berubah
    //   kalau ditolak.
    // TODO(Level 6): validasi & normalisasi ISBN -- buang tanda '-' dan spasi;
    //   hasilnya harus tepat 13 digit angka dengan digit cek ISBN-13 yang benar;
    //   kalau tidak, lempar ArgumentException. Isbn menyimpan versi TANPA '-'.


    //SAY : konstruktor ini gerbang pertama waktu kita buat objek "new Buku(...)"
    //mknya validasi judul, stok, sm ISBNditaro paling awal sblm field nya diisi
    public Buku(string isbn, string judul, int stokTotal)
    {
        if (string.IsNullOrWhiteSpace(judul))
        throw new ArgumentException("Judul tidak boleh kosong.", nameof(judul));
    }
    if (stokTotal < 0)
    {
        throw new ArgumentOutOfRangeException(nameof(stokTotal), "Stok total tidak boleh negatif.");
    }

    //panggil method helper buat ngebersihin dan ngecek validitas ISBN-13
    string bersihIsbn = ValidasiDanNormalisasiIsbn(isbn);

    // TODO(Level 1): isi Isbn, Judul, StokTotal dari parameter; StokTersedia
        //   awal = stokTotal.
        _isbn = bersihIsbn;
        _judul = judul;
        _stokTotal = stokTotal;
        _stokTersedia = stokTotal; // Stok tersedia awal pas dibikin ya sama kayak stok totalnya
    }

    // Method helper khusus buat ngurusin aturan ISBN-13 (Level 6)
    private static string ValidasiDanNormalisasiIsbn(string isbn)
    {
        if (string.IsNullOrEmpty(isbn))
        {
            throw new ArgumentException("ISBN tidak boleh null atau kosong.", nameof(isbn));
        }

        // 1. Buang semua tanda strip (-) dan spasi
        string bersih = isbn.Replace("-", "").Replace(" ", "");

        // 2. Pastikan panjangnya pas 13 karakter dan semuanya angka
        if (bersih.Length != 13 || !long.TryParse(bersih, out _))
        {
            throw new ArgumentException("ISBN harus 13 digit angka.", nameof(isbn));
        }

        // 3. Algoritma cek digit ISBN-13 (dikalikan bobot 1 dan 3 bergantian)
        int total = 0;
        for (int i = 0; i < 13; i++)
        {
            int digit = bersih[i] - '0';
            int bobot = (i % 2 == 0) ? 1 : 3;
            total += digit * bobot;
        }

       // Total keseluruhannya harus habis dibagi 10 (% 10 == 0)
        if (total % 10 != 0)
        {
            throw new ArgumentException("Digit cek ISBN-13 tidak valid.", nameof(isbn));
        }

        return bersih;
    } 

    // Ekspos properti read-only (cuma punya 'get', gak ada 'set' publik) sesuai Level 1
    public string Isbn => _isbn;
    public string Judul => _judul;
    public int StokTotal => _stokTotal;
    public int StokTersedia => _stokTersedia;
    

    public void Pinjam()
    {
        // TODO(Level 3): kurangi StokTersedia satu. Kalau stok sudah 0, lempar
        //   InvalidOperationException dan biarkan stok tetap.

        //SAY : kalo mau dipinjem tp stoknya udah 0/minus,
        //tolak pake InvalidOperationException dan biarin stoknya tetep 0
        if (_stokTersedia <= 0)
        {
            throw new InvalidOperationException("Stok buku udah habis.");
        }
        _stokTersedia--;
    }

    public void Kembalikan()
    {
        // TODO(Level 4): tambah StokTersedia satu. Kalau stok sudah sama dengan
        //   StokTotal (tidak ada yang sedang dipinjam), lempar
        //   InvalidOperationException dan biarkan stok tetap.

        //SAY : jgn sampe buku yg dibalikin bikin stok tersedia jd melebihi stok total awal
        // kalo udh penug, lempar exception
        if (_stokTersedia >= _stokTotal)
        {
        throw new InvalidOperationException("Stok udah penuh");
        }
        _stokTersedia++;
    }

    // Level 5: properti TERHITUNG -- tanpa field pendukung, tanpa setter.
    public double PersentaseTersedia
    {
        get
        {
            // TODO(Level 5): kembalikan StokTersedia / StokTotal * 100 (double).
            //   Kalau StokTotal = 0 kembalikan 0 (bukan NaN).

            //SAY : hati" pembagian integer, mknya di-cast jd (double)
            //kalo stok totalnya 0, lgsung balikin 0 aja biar ga NaN (Not a Number)
            if (_stokTotal == 0) return 0;
            return (double)_stokTersedia / _stokTotal * 100;
        }
    }

    public string Status
    {
        get
        {
            // TODO(Level 5): kembalikan "Tersedia" kalau StokTersedia > 0,
            //   selain itu "Habis".
            return _stokTersedia > 0 ? "Tersedia" : "Habis";
        }
    }

}
