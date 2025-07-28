Use Museum;

--UDFs

--1Sanatçýnýn yaþýný döndürü
CREATE FUNCTION fn_SanatciYas(@SanatciID INT)
RETURNS INT
AS
BEGIN
    DECLARE @yas INT;

    SELECT @yas = 
        DATEDIFF(YEAR, DogumTarihi, ISNULL(OlumTarihi, GETDATE()))
    FROM Sanatcilar
    WHERE ID = @SanatciID;

    RETURN @yas;
END;

SELECT 
    ID,
    Ad,
    dbo.fn_SanatciYas(ID) AS SanatciYasi
FROM Sanatcilar;

SELECT dbo.fn_SanatciYas(1) AS SanatciYasi;


--2Bir sanatçýnýn eser sayýsý
CREATE FUNCTION fn_EserSayisiBySanatci(@SanatciID INT)
RETURNS INT
AS
BEGIN
    RETURN (
        SELECT COUNT(*) 
        FROM Eserler 
        WHERE Sanatci_ID = @SanatciID
    );
END;

SELECT 
    ID AS SanatciID,
    Ad,
    dbo.fn_EserSayisiBySanatci(ID) AS EserSayisi
FROM Sanatcilar;

SELECT dbo.fn_EserSayisiBySanatci(1) AS EserSayisi;


--3Sanatçýnýn ülkesi
CREATE FUNCTION fn_SanatciUlkesi(@SanatciID INT)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @ulke NVARCHAR(50);

    SELECT @ulke = Ulke FROM Sanatcilar WHERE ID = @SanatciID;

    RETURN @ulke;
END;

SELECT 
    ID AS SanatciID,
    Ad,
    dbo.fn_SanatciUlkesi(ID) AS Ulkesi
FROM Sanatcilar;

SELECT dbo.fn_SanatciUlkesi(1) AS Ulkesi;


--4Baðýþçýnýn toplam baðýþ miktarý 
CREATE FUNCTION fn_BagisToplamMiktarByBagisci(@BagisciID INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (
        SELECT ISNULL(SUM(Miktar),0)
        FROM Bagislar
        WHERE BagisciID = @BagisciID
    );
END;

SELECT 
    ID AS BagisciID,
    Ad,
    Soyad,
    dbo.fn_BagisToplamMiktarByBagisci(ID) AS ToplamBagis
FROM Bagiscilar;

SELECT dbo.fn_BagisToplamMiktarByBagisci(1) AS ToplamBagis;


--5Bir sanatçýnýn tüm eserleri 
CREATE FUNCTION fn_SanatciEserleri(@SanatciID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT ID, Ad, YapimYili, BulunduguMuze, MevcutDurum 
    FROM Eserler 
    WHERE Sanatci_ID = @SanatciID
);

SELECT * 
FROM dbo.fn_SanatciEserleri(1);


--6Bir eserin bakým kayýdý
CREATE FUNCTION fn_EserlerinBakimKayitlari(@EserID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT BakimTarihi, YapilanIslem, SorumluKisi 
    FROM EserBakimKayitlari 
    WHERE EserID = @EserID
);

SELECT * 
FROM dbo.fn_EserlerinBakimKayitlari(3);


--7Bir eserin sergilendiði sergiler 
CREATE FUNCTION fn_EserSergilendigiYerler(@EserID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT s.ID, s.Ad, s.Konum, s.BaslangicTarihi, s.BitisTarihi
    FROM Sergiler s
    INNER JOIN EserSergileri es ON s.ID = es.SergiID
    WHERE es.EserID = @EserID
);

SELECT * 
FROM dbo.fn_EserSergilendigiYerler(1);


--8müze toplam geliri
CREATE FUNCTION fn_MuzeToplamGeliri()
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (
        SELECT SUM(Tutar) FROM MuzeGelirleri
    );
END;



SELECT 
    'Müzenin toplam geliri:' AS Aciklama, 
    dbo.fn_MuzeToplamGeliri() AS ToplamGelir;


--9müze toplam gider
CREATE FUNCTION fn_MuzeToplamGideri()
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (
        SELECT SUM(Tutar) FROM MuzeGiderleri
    );
END;

SELECT 
    'Müzenin toplam gideri:' AS Aciklama, 
    dbo.fn_MuzeToplamGideri() AS ToplamGider;


--10etkinliðin süresi   BU KOD BAK
CREATE FUNCTION fn_EtkinlikSuresiGun(@EtkinlikID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Sure INT;

    SELECT @Sure = DATEDIFF(DAY, BaslangicTarihi, BitisTarihi)
    FROM Etkinlikler
    WHERE ID = @EtkinlikID;

    RETURN @Sure;
END;

SELECT dbo.fn_EtkinlikSuresiGun(1) AS EtkinlikSuresiGun;


--eserin son bakim tarihi
CREATE FUNCTION fn_SonBakimTarihi()
RETURNS TABLE
AS
RETURN
(
    SELECT EserID, MAX(BakimTarihi) AS SonBakimTarihi
    FROM EserBakimKayitlari
    GROUP BY EserID
);

SELECT * FROM dbo.fn_SonBakimTarihi();


--eser yaþý
CREATE FUNCTION fn_EserYasiniGetir(@EserID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Yas INT;

    SELECT @Yas = DATEDIFF(YEAR, YapimYili, GETDATE())
    FROM Eserler
    WHERE ID = @EserID;

    RETURN @Yas;
END;


SELECT dbo.fn_EserYasiniGetir(5) AS EserYasi;