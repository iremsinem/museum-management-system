Use Museum;


SELECT * FROM AylikFinansOzet;


--View
SELECT * FROM vw_EserDetaylari;

SELECT * FROM vw_AktifSergiler;

SELECT * FROM vw_ZiyaretciEtkinlikKayitlari;

SELECT * FROM vw_GelirGiderOzet;

SELECT * FROM vw_EserBakimGecmisi;

SELECT * FROM vw_SanatciAkimlari;

SELECT * FROM vw_UyelikliZiyaretciler;

SELECT * FROM vw_EserTransferKayitlari;

SELECT * FROM vw_ZiyaretciGirisRaporu;

SELECT * FROM vw_BagisciDetaylari;

SELECT * FROM vw_PersonelMaasListesi;



--Full-Text
SELECT ID, Ad, MevcutDurum
FROM   dbo.Eserler
WHERE  CONTAINS((Ad, MevcutDurum), ' Sergileniyor ');
GO

SELECT Ad, Ulke
FROM   dbo.Sanatcilar
WHERE  FREETEXT (Ulke, 'Ýtalya');
GO

SELECT Ad, Aciklama
FROM   dbo.EserTurleri
WHERE  CONTAINS (Aciklama, ' "tarihi" ');
GO

SELECT Ad, BaslangicTarihi
FROM   dbo.Sergiler
WHERE  FREETEXT (Aciklama, ' heykel');
GO

SELECT Ad, Tur, BaslangicTarihi
FROM   dbo.Etkinlikler
WHERE  CONTAINS (Ad, ' tarihi ');
GO

SELECT EserID, BakimTarihi, YapilanIslem
FROM   dbo.EserBakimKayitlari
WHERE  CONTAINS (YapilanIslem, ' "temizlik" ');
GO

SELECT *
FROM   dbo.SanatAkimlari
WHERE  CONTAINS (Ad, ' "rönesans" ');
GO

SELECT BagisTarihi, Miktar
FROM   dbo.Bagislar
WHERE  CONTAINS (KullanimAlani, ' "yenileme" ');
GO

SELECT Ad, Soyad, Gorev
FROM   dbo.Personel
WHERE  CONTAINS (Gorev, ' "rehber" ');
GO

SELECT Tutar, Tarih, Aciklama
FROM   dbo.MuzeGiderleri
WHERE  FREETEXT(Aciklama, ' sigorta ');
GO



--UDFs

SELECT 
    ID,
    Ad,
    dbo.fn_SanatciYas(ID) AS SanatciYasi
FROM Sanatcilar;

SELECT dbo.fn_SanatciYas(1) AS SanatciYasi;


SELECT 
    ID AS SanatciID,
    Ad,
    dbo.fn_EserSayisiBySanatci(ID) AS EserSayisi
FROM Sanatcilar;

SELECT dbo.fn_EserSayisiBySanatci(1) AS EserSayisi;


SELECT 
    ID AS SanatciID,
    Ad,
    dbo.fn_SanatciUlkesi(ID) AS Ulkesi
FROM Sanatcilar;

SELECT dbo.fn_SanatciUlkesi(1) AS Ulkesi;


SELECT 
    ID AS BagisciID,
    Ad,
    Soyad,
    dbo.fn_BagisToplamMiktarByBagisci(ID) AS ToplamBagis
FROM Bagiscilar;

SELECT dbo.fn_BagisToplamMiktarByBagisci(1) AS ToplamBagis;


SELECT * 
FROM dbo.fn_SanatciEserleri(1);


SELECT * 
FROM dbo.fn_EserlerinBakimKayitlari(3);


SELECT * 
FROM dbo.fn_EserSergilendigiYerler(1);


SELECT 
    'Müzenin toplam geliri:' AS Aciklama, 
    dbo.fn_MuzeToplamGeliri() AS ToplamGelir;


	SELECT 
    'Müzenin toplam gideri:' AS Aciklama, 
    dbo.fn_MuzeToplamGideri() AS ToplamGider;

SELECT dbo.fn_EtkinlikSuresiGun(1) AS EtkinlikSuresiGun;



SELECT * FROM dbo.fn_SonBakimTarihi();


SELECT dbo.fn_EserYasiniGetir(5) AS EserYasi;






--Execution

SELECT TOP (10)
       e.ID,
       e.Ad,
       COUNT(*) AS SergiSayisi
FROM   dbo.Eserler       AS e
JOIN   dbo.EserSergileri AS es ON es.EserID = e.ID
GROUP  BY e.ID, e.Ad
ORDER  BY SergiSayisi DESC;
GO


SELECT s.ID, s.Ad, COUNT(e.ID) AS EserSayisi
FROM Sanatcilar s
LEFT JOIN Eserler e ON e.Sanatci_ID = s.ID
GROUP BY s.ID, s.Ad
ORDER BY EserSayisi DESC;
GO



SELECT KaynakTuru, SUM(Tutar) AS ToplamGelir
FROM MuzeGelirleri
GROUP BY KaynakTuru
ORDER BY ToplamGelir DESC;
GO


SELECT TOP 5 b.Ad + ' ' + b.Soyad AS BagisciAdi, SUM(bg.Miktar) AS ToplamBagis
FROM Bagiscilar b
JOIN Bagislar bg ON b.ID = bg.BagisciID
GROUP BY b.Ad, b.Soyad
ORDER BY ToplamBagis DESC;
GO