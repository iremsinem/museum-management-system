Use Museum;

SET STATISTICS IO  ON;
SET STATISTICS TIME ON;
GO



--en �ok sergilenen 10 eser 
SELECT TOP (10)
       e.ID,
       e.Ad,
       COUNT(*) AS SergiSayisi
FROM   dbo.Eserler       AS e
JOIN   dbo.EserSergileri AS es ON es.EserID = e.ID
GROUP  BY e.ID, e.Ad
ORDER  BY SergiSayisi DESC;
GO

--sanat��ya ait eser say�s�
SELECT s.ID, s.Ad, COUNT(e.ID) AS EserSayisi
FROM Sanatcilar s
LEFT JOIN Eserler e ON e.Sanatci_ID = s.ID
GROUP BY s.ID, s.Ad
ORDER BY EserSayisi DESC;
GO


--Gelir kaynaklar�na g�re toplam gelir
SELECT KaynakTuru, SUM(Tutar) AS ToplamGelir
FROM MuzeGelirleri
GROUP BY KaynakTuru
ORDER BY ToplamGelir DESC;
GO




--en �ok ba��� yapan 5 ba�����
SELECT TOP 5 b.Ad + ' ' + b.Soyad AS BagisciAdi, SUM(bg.Miktar) AS ToplamBagis
FROM Bagiscilar b
JOIN Bagislar bg ON b.ID = bg.BagisciID
GROUP BY b.Ad, b.Soyad
ORDER BY ToplamBagis DESC;
GO