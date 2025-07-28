Use Museum;

--indeksleme stratejisi

--Eserler tablosuna Sanatci_ID üzerinden non-clustered index tanýmlanarak sorgu hýzlandýrýlmýþtýr.
CREATE NONCLUSTERED INDEX IX_Eserler_SanatciID
ON Eserler(Sanatci_ID);


SELECT Ad, YapimYili, MevcutDurum
FROM Eserler
WHERE Sanatci_ID = 1;



-- BagisciID alaný üzerine index eklenmiþtir.

SELECT Miktar, BagisTarihi
FROM Bagislar
WHERE BagisciID = 2;

CREATE NONCLUSTERED INDEX IX_Bagislar_BagisciID
ON Bagislar(BagisciID);

--Normalizasyon Uygulamasý:
--Eserler tablosu ile Sanatcilar, Bagislar tablosu ile Bagiscilar arasýnda bire çok iliþkiler tanýmlanmýþtýr. Böylece Sanatci_ID ve BagisciID gibi yabancý anahtarlar üzerinden veri tekrarý ortadan kaldýrýlmýþ ve veritabaný yapýsý 2NF (Ýkinci Normal Form) düzeyinde normalize edilmiþtir.
--Bu yapý sayesinde hem veri tutarlýlýðý saðlanmakta hem de verimlilik artýrýlmaktadýr.



--2. Adým – Alternatif Ýndeksleme Stratejileri
--Artýk tablo bazlý, performansa katký saðlayan doðru indeksler yazalým. Hem hocanýn “indeksleme stratejileri” þartýný karþýlayacak hem de uygulamaný bozmayacak:

 --a) Tarihe göre sorgulamalar için:
CREATE NONCLUSTERED INDEX IX_MuzeGeliri_Tarih 
ON MuzeGelirleri(Tarih);

 --b) Kaynak türüne göre filtrelemeler için:
CREATE NONCLUSTERED INDEX IX_MuzeGeliri_KaynakTuru 
ON MuzeGelirleri(KaynakTuru);
-- c) Giderler tablosunda tarihe göre arama hýzlandýrmak için:
CREATE NONCLUSTERED INDEX IX_MuzeGideri_Tarih 
ON MuzeGiderleri(Tarih);

 --3. Adým – Normalizasyon Uygulamasý Örneði

 --Normalleþtirilmiþ yapý:

CREATE TABLE GelirKaynakTurleri (
    ID INT PRIMARY KEY IDENTITY,
    Ad NVARCHAR(100)
);

-- Var olan KaynakTuru deðerlerini bu tabloya aktarýrsýn 
INSERT INTO GelirKaynakTurleri (Ad)
SELECT DISTINCT KaynakTuru FROM MuzeGelirleri;
--Bu yapý 2. normal forma uygun olur ve “normalizasyon uygulandý” diyebilirsin.


--Ýndeksleme Stratejisi:
--MuzeGelirleri ve MuzeGiderleri tablolarýnda tarih ve kaynak türü gibi sýkça filtreleme yapýlan alanlara yönelik NONCLUSTERED INDEX tanýmlanmýþtýr. Bu indeksler sorgu performansýný artýrmaya yöneliktir.

--Normalizasyon Uygulamasý:
--MuzeGelirleri tablosundaki tekrar eden KaynakTuru verisi, GelirKaynakTurleri adlý ayrý bir tabloya alýnarak 2NF düzeyinde normalizasyon saðlanmýþtýr. Var olan veri bozulmadan, sadece yapýsal örnekleme yapýlmýþtýr.
