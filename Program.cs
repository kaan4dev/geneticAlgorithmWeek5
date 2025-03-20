using System;
using System.Linq;

class GenetikAlgoritma
{
    static Random random = new Random();
    static int nesilBoyutu = 5; // popülasyondaki nesil sayısı
    static int genUzunlugu = 5; // popülasyondaki bireylerin bit sayısı    
    static int nesilSayisi = 20; // bitiş noktası
    static double mutasyonOrani = 0.05; // mutasyon oranı

    static void Main()
    {

        int[] bireyler = BaslangicPopulasyonuOlustur();

        for (int nesil = 0; nesil < nesilSayisi; nesil++)
        {

            int[] uygunlukDegerleri = bireyler.Select(UygunlukFonksiyonu).ToArray(); // bireylerin fitness değerini hesaplamak
            int enIyiIndeks = Array.IndexOf(uygunlukDegerleri, uygunlukDegerleri.Max()); // en iyi fitness değerine sahip bireyi seçmek
            Console.WriteLine($"Nesil {nesil + 1}: En iyi birey = {bireyler[enIyiIndeks]}, Değer = {uygunlukDegerleri[enIyiIndeks]}");


            int[] yeniBireyler = new int[nesilBoyutu]; // yeni hesaplamalar için yeni nesil oluşturmak
            for (int i = 0; i < nesilBoyutu; i += 2) // yeni nesil oluşturmak için ikişerli bir döngü yapmak
            {
                int ebeveyn1 = EbeveynSec(bireyler, uygunlukDegerleri); // ebeveyn seç fonksiyonundan rulet tekerleği yöntemiyle ebeveyn seçmek
                int ebeveyn2 = EbeveynSec(bireyler, uygunlukDegerleri); // ebeveyn seç fonksiyonundan rulet tekerleği yöntemiyle ebeveyn seçmek

                (int cocuk1, int cocuk2) = Caprazlama(ebeveyn1, ebeveyn2); // çaprazlama yöntemiyle iki yeni çocuk oluşturmak
                yeniBireyler[i] = MutasyonUygula(cocuk1); // birinci çocuğa mutasyon uygular ve yeni nesile eklemek
                if (i + 1 < nesilBoyutu) yeniBireyler[i + 1] = MutasyonUygula(cocuk2); // cocuk2’yi yeni nesle eklemek
            }

            bireyler = yeniBireyler; // yeni nesli mevcut nesil olarak atar
        }


        int[] sonUygunlukDegerleri = bireyler.Select(UygunlukFonksiyonu).ToArray(); // son neslin uygunluk değerlerini hesaplamak
        int enIyiSonIndeks = Array.IndexOf(sonUygunlukDegerleri, sonUygunlukDegerleri.Max()); // son nesildeki en iyi fitness değerine sahip bireyi seçmek
        Console.WriteLine($"\nEn iyi çözüm: {bireyler[enIyiSonIndeks]}, Maksimum f(x) = {sonUygunlukDegerleri[enIyiSonIndeks]}");

        Console.ReadKey();
    }

    static int[] BaslangicPopulasyonuOlustur()
    {
        return Enumerable.Range(0, nesilBoyutu)
                         .Select(_ => rastgele.Next(0, 31)) // 0 ile 30 arasında rastgele bir değer oluşturmak
                         .ToArray();
    }

    static int UygunlukFonksiyonu(int x) => x * x; // uygunluk fonksiyonunu verilen soruya göre tanımlamak
    

    static int EbeveynSec(int[] bireyler, int[] uygunlukDegerleri) // rulet tekerleği yöntemine göre seçim ayarlamak
    {
        int toplamUygunluk = uygunlukDegerleri.Sum(); // tüm uygunluk değerlerinin toplamını hesaplamak
        int rastgeleDeger = rastgele.Next(0, toplamUygunluk); // 0 ile toplam uygunluk değeri arasında rastgele bir sayı üretmek
        int toplam = 0; // kümülatif uygunluk değerini tutmak için bir değişken tanımlamak
        for (int i = 0; i < nesilBoyutu; i++)
        {
            toplam += uygunlukDegerleri[i]; // mevcut bireyin uygunluk değerini kümülatif toplama eklemek
            if (toplam > rastgeleDeger) return bireyler[i]; // eğer kümülatif toplam rastgele değeri aşarsa, o bireyi ebeveyn olarak seçmek
        }
        return bireyler[0]; // seçim gerçekleşmezde 0 indeksindeki elemanı ebeveyn seçmek
    }


    static (int, int) Caprazlama(int ebeveyn1, int ebeveyn2)
    {
        Random rastgele = new Random();

        int nokta = rastgele.Next(1, genUzunlugu - 1);

        string ebeveyn1Binary = Convert.ToString(ebeveyn1, 2).PadLeft(genUzunlugu, '0'); // birinci ebeveyni ikilik tabana çevirir ve sol tarafına 0 ekleyerek gen uzunluğuna tamamlamak
        string ebeveyn2Binary = Convert.ToString(ebeveyn2, 2).PadLeft(genUzunlugu, '0'); // ikinci ebeveyni ikilik tabana çevirir ve sol tarafına 0 ekleyerek gen uzunluğuna tamamlamak

        string cocuk1Binary = ebeveyn1Binary.Substring(0, nokta) + ebeveyn2Binary.Substring(nokta); // Birinci çocuğun genlerini oluşturur (birinci ebeveynin çaprazlama noktasına kadar olan kısmı + ikinci ebeveynin çaprazlama noktasından sonraki kısmı).
        string cocuk2Binary = ebeveyn2Binary.Substring(0, nokta) + ebeveyn1Binary.Substring(nokta); // İkinci çocuğun genlerini oluşturur (ikinci ebeveynin çaprazlama noktasına kadar olan kısmı + birinci ebeveynin çaprazlama noktasından sonraki kısmı).

        int cocuk1 = Convert.ToInt32(cocuk1Binary, 2); // birinci çocuğu onluk tabana çevirmek
        int cocuk2 = Convert.ToInt32(cocuk2Binary, 2); // ikinci çocuğu onluk tabana çevirmek

        return (cocuk1, cocuk2);
    }

    static int MutasyonUygula(int gen)
    {
        Random rastgele = new Random();

        double olasilik = rastgele.NextDouble(); // 0 ile 1 arasında rastgele bir olasılık değeri üretmek
        
        if (olasilik < mutasyonOrani)
        {
            string binaryString = Convert.ToString(gen, 2).PadLeft(genUzunlugu, '0'); // geni ikilik tabana çevirir ve sol tarafına 0 ekleyerek gen uzunluğuna tamamlar.
            char[] bitDizisi = binaryString.ToCharArray(); // ikilik stringi karakter dizisine dönüştürür.

            int bitKonumu = rastgele.Next(0, genUzunlugu); // 0 ile genUzunlugu arasında rastgele bir bit konumu seçer.
            if (bitDizisi[bitDizisi.Length - 1 - bitKonumu] == '0') // seçilen bitin değerini kontrol eder.
            {
                bitDizisi[bitDizisi.Length - 1 - bitKonumu] = '1'; // eğer bit 0 ise, 1 yapar.
            }
            else
            {
                bitDizisi[bitDizisi.Length - 1 - bitKonumu] = '0'; // eğer bit 1 ise, 0 yapar.
            }

            string yeniBinary = new string(bitDizisi); // değiştirilmiş bit dizisini tekrar stringe dönüştürür.
            gen = Convert.ToInt32(yeniBinary, 2); // yeni ikilik stringi onluk tabana çevirir.
        }

        return gen;
    }

}