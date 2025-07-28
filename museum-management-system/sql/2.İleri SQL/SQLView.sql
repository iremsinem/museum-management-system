
--VIEWS
USE Museum;

--1)Eserler, sanat�� ve t�r bilgilerini birle�tirir.
CREATE VIEW vw_EserDetaylari AS
SELECT 
    e.ID AS EserID,
    e.Ad AS EserAdi,
    s.Ad AS SanatciAdi,
    t.Ad AS EserTuru,
    e.YapimYili,
    e.BulunduguMuze,
    e.MevcutDurum
FROM Eserler e
INNER JOIN Sanatcilar s ON e.Sanatci_ID = s.ID
INNER JOIN EserTurleri t ON e.Tur_ID = t.ID;

SELECT * FROM vw_EserDetaylari;


--2)Sadece �u anda aktif olan sergileri listeler.
CREATE VIEW vw_AktifSergiler AS
SELECT 
    ID,
    Ad,
    Konum,
    BaslangicTarihi,
    BitisTarihi
FROM Sergiler
WHERE GETDATE() BETWEEN BaslangicTarihi AND BitisTarihi;

SELECT * FROM vw_AktifSergiler;


--3)Ziyaret�ilerin kat�ld��� etkinlikleri g�sterir.   
CREATE VIEW vw_ZiyaretciEtkinlikKayitlari AS
SELECT 
    z.Ad + ' ' + z.Soyad AS ZiyaretciAdSoyad,
    e.Ad AS EtkinlikAdi,
    ek.KayitTarihi
FROM EtkinlikKayitlari ek
INNER JOIN Ziyaretciler z ON ek.ZiyaretciID = z.ID
INNER JOIN Etkinlikler e ON ek.EtkinlikID = e.ID;


SELECT * FROM vw_ZiyaretciEtkinlikKayitlari;


--4)M�ze gelir ve giderlerini �zetler.    
CREATE VIEW vw_GelirGiderOzet AS
SELECT 'Gelir' AS Tip, SUM(Tutar) AS ToplamTutar FROM MuzeGelirleri
UNION
SELECT 'Gider' AS Tip, SUM(Tutar) FROM MuzeGiderleri;

SELECT * FROM vw_GelirGiderOzet;


--5)Her eserin bak�m ge�mi�ini listeler.  
CREATE VIEW vw_EserBakimGecmisi AS
SELECT 
    e.Ad AS EserAdi,
    b.BakimTarihi,
    b.YapilanIslem
FROM EserBakimKayitlari b
INNER JOIN Eserler e ON b.EserID = e.ID;

SELECT * FROM vw_EserBakimGecmisi;


--6)Sanat��lar�n dahil oldu�u sanat ak�mlar�n� listeler.  
CREATE VIEW vw_SanatciAkimlari AS
SELECT 
    s.Ad AS Sanatci,
    a.Ad AS Akim,
    a.Aciklama
FROM SanatciAkim sa
JOIN Sanatcilar s ON sa.SanatciID = s.ID
JOIN SanatAkimlari a ON sa.AkimID = a.ID;

SELECT * FROM vw_SanatciAkimlari;


--7)Yaln�zca �yelikli ziyaret�ileri listeler.  
CREATE VIEW vw_UyelikliZiyaretciler AS
SELECT 
    Ad,
    Soyad,
	DogumTarihi,
    Email
FROM Ziyaretciler
WHERE UyelikDurumu = 1;

SELECT * FROM vw_UyelikliZiyaretciler;


--8)T�m eser transfer ge�mi�ini �zetler.
CREATE VIEW vw_EserTransferKayitlari AS
SELECT 
    e.Ad AS EserAdi,
    t.KaynakMuze,
    t.HedefMuze,
    t.Tarih,
    t.TransferDurumu
FROM EserTransferleri t
INNER JOIN Eserler e ON t.EserID = e.ID;

SELECT * FROM vw_EserTransferKayitlari;


--9)Ziyaret�i giri�-��k�� bilgilerini sergi ve etkinlik ile birlikte g�sterir.  
CREATE VIEW vw_ZiyaretciGirisRaporu AS
SELECT 
    z.Ad + ' ' + z.Soyad AS Ziyaretci,
    g.GirisTarihi,
    g.CikisTarihi,
    s.Ad AS Sergi,
    e.Ad AS Etkinlik
FROM ZiyaretciGirisKayitlari g
INNER JOIN Ziyaretciler z ON g.ZiyaretciID = z.ID
LEFT JOIN Sergiler s ON g.SergiID = s.ID
LEFT JOIN Etkinlikler e ON g.EtkinlikID = e.ID;

SELECT * FROM vw_ZiyaretciGirisRaporu;


--10)Ba����� detaylar�
CREATE VIEW vw_BagisciDetaylari AS
SELECT b.Ad + ' ' + b.Soyad AS BagisciAdi, d.Miktar, d.BagisTarihi, d.KullanimAlani
FROM Bagiscilar b
JOIN Bagislar d ON b.ID = d.BagisciID;

SELECT * FROM vw_BagisciDetaylari;


--11)Personel maa� listesi
CREATE VIEW vw_PersonelMaasListesi AS
SELECT Ad + ' ' + Soyad AS AdSoyad, Gorev, Maas
FROM Personel;

SELECT * FROM vw_PersonelMaasListesi;


