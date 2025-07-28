Use Museum;


--Full-Text Search

CREATE FULLTEXT CATALOG FullTextCatalog AS DEFAULT;

CREATE FULLTEXT INDEX ON dbo.Sanatcilar(Biyografi)
KEY INDEX PK__Sanatcil__3214EC2734B629D3;

/* 0) Veritabanýnda FTS’i etkinleþtir ve varsayýlan bir katalog oluþtur*/
USE Museum;
GO
EXEC sp_fulltext_database 'enable';        -- veritabanýný FTS için aç
GO
IF NOT EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE name = 'ftc_Museum')
    CREATE FULLTEXT CATALOG ftc_Museum AS DEFAULT;
GO


/*1) ESERLER  (Ad, MevcutDurum sözcüklerinde arama)*/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_Eserler_ID')
    CREATE UNIQUE INDEX ui_Eserler_ID ON dbo.Eserler(ID);
GO
CREATE FULLTEXT INDEX ON dbo.Eserler
(
    Ad            LANGUAGE 1055,
    MevcutDurum   LANGUAGE 1055
)
KEY INDEX ui_Eserler_ID
WITH STOPLIST = SYSTEM;
GO

SELECT ID, Ad, MevcutDurum
FROM   dbo.Eserler
WHERE  CONTAINS((Ad, MevcutDurum), ' Sergileniyor ');
GO

/*2) SANATÇILAR  (Biyografi alaný)*/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_Sanatcilar_ID')
    CREATE UNIQUE INDEX ui_Sanatcilar_ID ON dbo.Sanatcilar(ID);
GO
CREATE FULLTEXT INDEX ON dbo.Sanatcilar
(
    Ulke  LANGUAGE 1055
)
KEY INDEX ui_Sanatcilar_ID;
GO

SELECT Ad, Ulke
FROM   dbo.Sanatcilar
WHERE  FREETEXT (Ulke, 'Ýtalya');
GO

/*3) ESER TÜRLERÝ  (Aciklama)*/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_EserTurleri_ID')
    CREATE UNIQUE INDEX ui_EserTurleri_ID ON dbo.EserTurleri(ID);
GO
CREATE FULLTEXT INDEX ON dbo.EserTurleri
(
    Aciklama LANGUAGE 1055
)
KEY INDEX ui_EserTurleri_ID;
GO

SELECT Ad, Aciklama
FROM   dbo.EserTurleri
WHERE  CONTAINS (Aciklama, ' "tarihi" ');
GO

/* 4) SERGÝLER  (Aciklama)*/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_Sergiler_ID')
    CREATE UNIQUE INDEX ui_Sergiler_ID ON dbo.Sergiler(ID);
GO
CREATE FULLTEXT INDEX ON dbo.Sergiler
(
    Aciklama LANGUAGE 1055
)
KEY INDEX ui_Sergiler_ID;
GO

SELECT Ad, BaslangicTarihi
FROM   dbo.Sergiler
WHERE  FREETEXT (Aciklama, ' heykel');
GO

/*  5) ETKÝNLÝKLER  (Ad + Aciklama)*/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_Etkinlikler_ID')
    CREATE UNIQUE INDEX ui_Etkinlikler_ID ON dbo.Etkinlikler(ID);
GO
CREATE FULLTEXT INDEX ON dbo.Etkinlikler
(
    Ad        LANGUAGE 1055,
    Aciklama  LANGUAGE 1055
)
KEY INDEX ui_Etkinlikler_ID;
GO

SELECT Ad, Tur, BaslangicTarihi
FROM   dbo.Etkinlikler
WHERE  CONTAINS (Ad, ' tarihi ');
GO

/* 6) ESER BAKIM KAYITLARI  (YapilanIslem)*/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_BakimKayitlari_ID')
    CREATE UNIQUE INDEX ui_BakimKayitlari_ID ON dbo.EserBakimKayitlari(ID);
GO
CREATE FULLTEXT INDEX ON dbo.EserBakimKayitlari
(
    YapilanIslem LANGUAGE 1055
)
KEY INDEX ui_BakimKayitlari_ID;
GO

SELECT EserID, BakimTarihi, YapilanIslem
FROM   dbo.EserBakimKayitlari
WHERE  CONTAINS (YapilanIslem, ' "temizlik" ');
GO

/*  7) SANAT AKIMLARI  (Ad + Aciklama)*/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_SanatAkimlari_ID')
    CREATE UNIQUE INDEX ui_SanatAkimlari_ID ON dbo.SanatAkimlari(ID);
GO
CREATE FULLTEXT INDEX ON dbo.SanatAkimlari
(
    Ad        LANGUAGE 1055,
    Aciklama  LANGUAGE 1055
)
KEY INDEX ui_SanatAkimlari_ID;
GO

SELECT *
FROM   dbo.SanatAkimlari
WHERE  CONTAINS (Ad, ' "rönesans" ');
GO

/* 8) BAÐIÞLAR  (KullanimAlani)*/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_Bagislar_ID')
    CREATE UNIQUE INDEX ui_Bagislar_ID ON dbo.Bagislar(ID);
GO
CREATE FULLTEXT INDEX ON dbo.Bagislar
(
    KullanimAlani LANGUAGE 1055
)
KEY INDEX ui_Bagislar_ID;
GO

SELECT BagisTarihi, Miktar
FROM   dbo.Bagislar
WHERE  CONTAINS (KullanimAlani, ' "yenileme" ');
GO

/*  9) PERSONEL  (Gorev)          -- kýsa metin ama sýk sorgulanabilir*/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_Personel_ID')
    CREATE UNIQUE INDEX ui_Personel_ID ON dbo.Personel(ID);
GO
CREATE FULLTEXT INDEX ON dbo.Personel
(
    Gorev LANGUAGE 1055
)
KEY INDEX ui_Personel_ID;
GO

SELECT Ad, Soyad, Gorev
FROM   dbo.Personel
WHERE  CONTAINS (Gorev, ' "rehber" ');
GO

/*10) Gider açýklama alaný */
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'ui_MuzeGiderleri_ID')
    CREATE UNIQUE INDEX ui_MuzeGiderleri_ID ON dbo.MuzeGiderleri(ID);
GO

CREATE FULLTEXT INDEX ON dbo.MuzeGiderleri
(
    Aciklama LANGUAGE 1055
)
KEY INDEX ui_MuzeGiderleri_ID;
GO

-- Örnek arama
SELECT Tutar, Tarih, Aciklama
FROM   dbo.MuzeGiderleri
WHERE  FREETEXT(Aciklama, ' sigorta ');
GO

