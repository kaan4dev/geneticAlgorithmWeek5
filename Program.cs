using System;
using System.Linq;

class GenetikAlgoritma
{
    static Random rastgele = new Random();
    static int nesilBoyutu = 5;
    static int genUzunlugu = 5;
    static int nesilSayisi = 20;
    static double mutasyonOrani = 0.05;

    static void Main()
    {

        int[] bireyler = BaslangicPopulasyonuOlustur();

        for (int nesil = 0; nesil < nesilSayisi; nesil++)
        {

            int[] uygunlukDegerleri = bireyler.Select(UygunlukFonksiyonu).ToArray();

            int enIyiIndeks = Array.IndexOf(uygunlukDegerleri, uygunlukDegerleri.Max());
            Console.WriteLine($"Nesil {nesil + 1}: En iyi birey = {bireyler[enIyiIndeks]}, Değer = {uygunlukDegerleri[enIyiIndeks]}");

            int[] yeniBireyler = new int[nesilBoyutu];
            for (int i = 0; i < nesilBoyutu; i += 2)
            {
                int ebeveyn1 = EbeveynSec(bireyler, uygunlukDegerleri);
                int ebeveyn2 = EbeveynSec(bireyler, uygunlukDegerleri);

                (int cocuk1, int cocuk2) = Caprazlama(ebeveyn1, ebeveyn2);
                yeniBireyler[i] = MutasyonUygula(cocuk1);
                if (i + 1 < nesilBoyutu) yeniBireyler[i + 1] = MutasyonUygula(cocuk2);
            }

            bireyler = yeniBireyler;
        }

        int[] sonUygunlukDegerleri = bireyler.Select(UygunlukFonksiyonu).ToArray();
        int enIyiSonIndeks = Array.IndexOf(sonUygunlukDegerleri, sonUygunlukDegerleri.Max());
        Console.WriteLine($"\nEn iyi çözüm: {bireyler[enIyiSonIndeks]}, Maksimum f(x) = {sonUygunlukDegerleri[enIyiSonIndeks]}");

        Console.ReadKey();
    }

    static int[] BaslangicPopulasyonuOlustur()
    {
        return Enumerable.Range(0, nesilBoyutu)
                         .Select(_ => rastgele.Next(0, 31))
                         .ToArray();
    }

    static int UygunlukFonksiyonu(int x) => x * x;

    // Ebeveyn Seçimi (Rulet Çarkı Seçimi)
    static int EbeveynSec(int[] bireyler, int[] uygunlukDegerleri)
    {
        int toplamUygunluk = uygunlukDegerleri.Sum();
        int rastgeleDeger = rastgele.Next(0, toplamUygunluk);
        int toplam = 0;
        for (int i = 0; i < nesilBoyutu; i++)
        {
            toplam += uygunlukDegerleri[i];
            if (toplam > rastgeleDeger) return bireyler[i];
        }
        return bireyler[0];
    }



    static (int, int) Caprazlama(int ebeveyn1, int ebeveyn2)
    {
        Random rastgele = new Random();

        int nokta = rastgele.Next(1, genUzunlugu - 1);

        string ebeveyn1Binary = Convert.ToString(ebeveyn1, 2).PadLeft(genUzunlugu, '0');
        string ebeveyn2Binary = Convert.ToString(ebeveyn2, 2).PadLeft(genUzunlugu, '0');

        string cocuk1Binary = ebeveyn1Binary.Substring(0, nokta) + ebeveyn2Binary.Substring(nokta);
        string cocuk2Binary = ebeveyn2Binary.Substring(0, nokta) + ebeveyn1Binary.Substring(nokta);

        int cocuk1 = Convert.ToInt32(cocuk1Binary, 2);
        int cocuk2 = Convert.ToInt32(cocuk2Binary, 2);

        return (cocuk1, cocuk2);
    }

    static int MutasyonUygula(int gen)
    {
        Random rastgele = new Random();

        double olasilik = rastgele.NextDouble();
        if (olasilik < mutasyonOrani)
        {
            string binaryString = Convert.ToString(gen, 2).PadLeft(genUzunlugu, '0');
            char[] bitDizisi = binaryString.ToCharArray();

            int bitKonumu = rastgele.Next(0, genUzunlugu);
            if (bitDizisi[bitDizisi.Length - 1 - bitKonumu] == '0')
            {
                bitDizisi[bitDizisi.Length - 1 - bitKonumu] = '1'; 
            }
            else
            {
                bitDizisi[bitDizisi.Length - 1 - bitKonumu] = '0';
            }

            string yeniBinary = new string(bitDizisi);
            gen = Convert.ToInt32(yeniBinary, 2);
        }

        return gen;
    }

}
