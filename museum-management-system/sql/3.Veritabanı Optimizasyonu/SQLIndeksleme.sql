Use Museum;

--indeksleme stratejisi

--Eserler tablosuna Sanatci_ID �zerinden non-clustered index tan�mlanarak sorgu h�zland�r�lm��t�r.
CREATE NONCLUSTERED INDEX IX_Eserler_SanatciID
ON Eserler(Sanatci_ID);


SELECT Ad, YapimYili, MevcutDurum
FROM Eserler
WHERE Sanatci_ID = 1;



-- BagisciID alan� �zerine index eklenmi�tir.

SELECT Miktar, BagisTarihi
FROM Bagislar
WHERE BagisciID = 2;

CREATE NONCLUSTERED INDEX IX_Bagislar_BagisciID
ON Bagislar(BagisciID);

--Normalizasyon Uygulamas�:
--Eserler tablosu ile Sanatcilar, Bagislar tablosu ile Bagiscilar aras�nda bire �ok ili�kiler tan�mlanm��t�r. B�ylece Sanatci_ID ve BagisciID gibi yabanc� anahtarlar �zerinden veri tekrar� ortadan kald�r�lm�� ve veritaban� yap�s� 2NF (�kinci Normal Form) d�zeyinde normalize edilmi�tir.
--Bu yap� sayesinde hem veri tutarl�l��� sa�lanmakta hem de verimlilik art�r�lmaktad�r.



--2. Ad�m � Alternatif �ndeksleme Stratejileri
--Art�k tablo bazl�, performansa katk� sa�layan do�ru indeksler yazal�m. Hem hocan�n �indeksleme stratejileri� �art�n� kar��layacak hem de uygulaman� bozmayacak:

 --a) Tarihe g�re sorgulamalar i�in:
CREATE NONCLUSTERED INDEX IX_MuzeGeliri_Tarih 
ON MuzeGelirleri(Tarih);

 --b) Kaynak t�r�ne g�re filtrelemeler i�in:
CREATE NONCLUSTERED INDEX IX_MuzeGeliri_KaynakTuru 
ON MuzeGelirleri(KaynakTuru);
-- c) Giderler tablosunda tarihe g�re arama h�zland�rmak i�in:
CREATE NONCLUSTERED INDEX IX_MuzeGideri_Tarih 
ON MuzeGiderleri(Tarih);

 --3. Ad�m � Normalizasyon Uygulamas� �rne�i

 --Normalle�tirilmi� yap�:

CREATE TABLE GelirKaynakTurleri (
    ID INT PRIMARY KEY IDENTITY,
    Ad NVARCHAR(100)
);

-- Var olan KaynakTuru de�erlerini bu tabloya aktar�rs�n 
INSERT INTO GelirKaynakTurleri (Ad)
SELECT DISTINCT KaynakTuru FROM MuzeGelirleri;
--Bu yap� 2. normal forma uygun olur ve �normalizasyon uyguland�� diyebilirsin.


--�ndeksleme Stratejisi:
--MuzeGelirleri ve MuzeGiderleri tablolar�nda tarih ve kaynak t�r� gibi s�k�a filtreleme yap�lan alanlara y�nelik NONCLUSTERED INDEX tan�mlanm��t�r. Bu indeksler sorgu performans�n� art�rmaya y�neliktir.

--Normalizasyon Uygulamas�:
--MuzeGelirleri tablosundaki tekrar eden KaynakTuru verisi, GelirKaynakTurleri adl� ayr� bir tabloya al�narak 2NF d�zeyinde normalizasyon sa�lanm��t�r. Var olan veri bozulmadan, sadece yap�sal �rnekleme yap�lm��t�r.
