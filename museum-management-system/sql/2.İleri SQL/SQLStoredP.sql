
USE Museum;

--STORED PROCEDURES

ALTER PROCEDURE 

--1.yeni eser ekleme
CREATE PROCEDURE sp_EserEkle
    @Ad NVARCHAR(100),
    @Tur_ID INT,
    @Sanatci_ID INT,
    @YapimYili INT,
    @BulunduguMuze NVARCHAR(100),
    @MevcutDurum NVARCHAR(50),
    @DijitalKoleksiyonURL NVARCHAR(255)
AS
BEGIN
    INSERT INTO Eserler (Ad, Tur_ID, Sanatci_ID, YapimYili, BulunduguMuze, MevcutDurum, DijitalKoleksiyonURL)
    VALUES (@Ad, @Tur_ID, @Sanatci_ID, @YapimYili, @BulunduguMuze, @MevcutDurum, @DijitalKoleksiyonURL);
	SELECT SCOPE_IDENTITY();

END;


EXEC sp_EserEkle
    @Ad = N'Zihin Dalgalar�',
    @Tur_ID = 17,  -- sistemde hen�z olmayan yeni t�r (�rnek: N�rosanat, Dijital Soyutlama vs.)
    @Sanatci_ID = 21,  -- sistemde olmayan yeni sanat��
    @YapimYili = 2024,
    @BulunduguMuze = N'Sanal Bilin� Merkezi',
    @MevcutDurum = N'Dijital Ar�ivde',
    @DijitalKoleksiyonURL = N'https://koleksiyon.muze.com/zihin-dalgalari';




-- 2. Sanat�� ekler
CREATE PROCEDURE sp_SanatciEkle
    @Ad NVARCHAR(100),
    @DogumTarihi DATE,
    @OlumTarihi DATE = NULL,
    @Ulke NVARCHAR(50),
    @Biyografi TEXT
AS
BEGIN
    INSERT INTO Sanatcilar (Ad, DogumTarihi, OlumTarihi, Ulke, Biyografi)
    VALUES (@Ad, @DogumTarihi, @OlumTarihi, @Ulke, @Biyografi);
	SELECT SCOPE_IDENTITY();
END;

EXEC sp_SanatciEkle
    @Ad = N'Fatma Nur Aksoy',
    @DogumTarihi = '1988-10-12',
    @OlumTarihi = NULL,
    @Ulke = N'T�rkiye',
    @Biyografi = N'�a�da� dijital kolaj ve art�r�lm�� ger�eklik sanat� �zerine �al��an yenilik�i bir sanat��d�r.';
	

	

--3.eser bak�m� kaydetme
CREATE PROCEDURE sp_EserBakimiEkle
    @EserID INT,
    @BakimTarihi DATE,
    @YapilanIslem NVARCHAR(255)
    
AS
BEGIN
    INSERT INTO EserBakimKayitlari (EserID, BakimTarihi, YapilanIslem)
    VALUES (@EserID, @BakimTarihi, @YapilanIslem);
	
END

EXEC sp_EserBakimiEkle
    @EserID = 15,  -- �rnek: "Kara Kare" (ID 15 olabilir)
    @BakimTarihi = '2025-06-13',
    @YapilanIslem = N'Temizlik ve UV koruyucu cila uyguland�.',
    @SorumluKisi = 3;  -- �rnek: Personel tablosunda mevcut olan bir ki�i




-- 4. Yeni etkinlik ekler
CREATE PROCEDURE sp_EtkinlikEkle
    @Ad NVARCHAR(100),
    @Tur NVARCHAR(50),
    @BaslangicTarihi DATE,
    @BitisTarihi DATE,
    @Aciklama NVARCHAR(255),
    @TurID INT
AS
BEGIN
    INSERT INTO Etkinlikler (Ad, Tur, BaslangicTarihi, BitisTarihi, Aciklama, TurID)
    VALUES (@Ad, @Tur, @BaslangicTarihi, @BitisTarihi, @Aciklama, @TurID);
END;

EXEC sp_EtkinlikEkle
    @Ad = N'Ge�mi�ten G�n�m�ze Dijital Sanat',
    @Tur = N'Sergi',
    @BaslangicTarihi = '2025-09-01',
    @BitisTarihi = '2025-10-15',
    @Aciklama = N'Dijital sanat�n tarihsel geli�imini anlatan interaktif sergi.',
    @TurID = 4;  -- �rne�in �Sergi� t�r� i�in EtkinlikTurleri tablosunda ID = 4 olabilir



--5.sanat��ya sanat ak�m� ekleme
CREATE PROCEDURE sp_SanatciyaAkimEkle
    @SanatciID INT,
    @AkimID INT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM SanatciAkim WHERE SanatciID = @SanatciID AND AkimID = @AkimID)
    BEGIN
        INSERT INTO SanatciAkim (SanatciID, AkimID)
        VALUES (@SanatciID, @AkimID);
    END
END

EXEC sp_SanatciyaAkimEkle
    @SanatciID = 5,   -- �rne�in: Salvador Dal�
    @AkimID = 10;     -- �rne�in: S�rrealizm ak�m� (�rnek ID)



--6.sergiye eser ekelme
CREATE PROCEDURE sp_EseriSergiyeEkle
    @EserID INT,
    @SergiID INT,
    @BaslangicTarihi DATE,
    @BitisTarihi DATE
AS
BEGIN
    INSERT INTO EserSergileri (EserID, SergiID, BaslangicTarihi, BitisTarihi)
    VALUES (@EserID, @SergiID, @BaslangicTarihi, @BitisTarihi);
END

EXEC sp_EseriSergiyeEkle
    @EserID = 12,          -- �rnek: "Yans�ma" eseri
    @SergiID = 3,          -- �rnek: "Modern Zamanlar Sergisi" (var say�lan sergi)
    @BaslangicTarihi = '2025-07-01',
    @BitisTarihi = '2025-08-31';


-- 7. Belirli bir sergiye ait eserleri listeler
CREATE PROCEDURE sp_SergidekiEserleriListele
    @SergiID INT
AS
BEGIN
    SELECT E.Ad, E.YapimYili, E.MevcutDurum
    FROM EserSergileri ES
    JOIN Eserler E ON ES.EserID = E.ID
    WHERE ES.SergiID = @SergiID;
END;

EXEC sp_SergidekiEserleriListele
    @SergiID = 2;

-- 8. Sanat��ya ait eserleri listeler
CREATE PROCEDURE sp_SanatciyaAitEserler
    @SanatciID INT
AS
BEGIN
    SELECT * FROM Eserler WHERE Sanatci_ID = @SanatciID;
END;

EXEC sp_SanatciyaAitEserler
    @SanatciID = 2;


	ALTER PROCEDURE 

-- 9. Gelir ekler ve log yazar
CREATE PROCEDURE sp_GelirEkle_Logla
    @KaynakTuru NVARCHAR(50),
    @Tutar DECIMAL(10,2),
    @Tarih DATE,
    @AdminID INT
AS
BEGIN
    BEGIN TRAN;
    BEGIN TRY
        INSERT INTO MuzeGelirleri (KaynakTuru, Tutar, Tarih)
        VALUES (@KaynakTuru, @Tutar, @Tarih);

        INSERT INTO Log (LogDetay, AdminID)
        VALUES ('Gelir eklendi: ' + @KaynakTuru, @AdminID);

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH;
END;

EXEC sp_GelirEkle_Logla
    @KaynakTuru = N'Etkinlik',
    @Tutar = 750.00,
    @Tarih = '2025-06-13',
    @AdminID = 2;

	
	


--10. gider kayd� eklme
CREATE PROCEDURE sp_MuzeGiderEkle
    @Aciklama NVARCHAR(255),
    @Tutar DECIMAL(10,2),
    @Tarih DATE
AS
BEGIN
    INSERT INTO MuzeGiderleri (Aciklama, Tutar, Tarih)
    VALUES (@Aciklama, @Tutar, @Tarih);
	SELECT SCOPE_IDENTITY();
END

EXEC sp_MuzeGiderEkle
    @Aciklama = N'Eser ta��mac�l��� ve sigorta gideri',
    @Tutar = 1200.00,
    @Tarih = '2025-06-13';


	CREATE PROCEDURE sp_GelirEkle
    @Kaynak NVARCHAR(100),
    @Tutar DECIMAL(10, 2),
    @Tarih DATE
AS
BEGIN
    INSERT INTO MuzeGelirleri (KaynakTuru, Tutar, Tarih)
    VALUES (@Kaynak, @Tutar, @Tarih);

    SELECT SCOPE_IDENTITY(); 
END




--11.ziyaret�i kayd� ekleme
CREATE PROCEDURE sp_ZiyaretciEkle
    @Ad NVARCHAR(50),
    @Soyad NVARCHAR(50),
    @DogumTarihi DATE,
    @Email NVARCHAR(100),
    @UyelikDurumu BIT
AS
BEGIN
    INSERT INTO Ziyaretciler (Ad, Soyad, DogumTarihi, Email, UyelikDurumu)
    VALUES (@Ad, @Soyad, @DogumTarihi, @Email, @UyelikDurumu);
END

EXEC sp_ZiyaretciEkle
    @Ad = N'Merve',
    @Soyad = N'Tanr�verdi',
    @DogumTarihi = '1998-07-22',
    @Email = N'merve.tanriverdi@example.com',
    @UyelikDurumu = 1;




-- 12.Ziyaret�iyi etkinli�e kaydeder
CREATE PROCEDURE sp_ZiyaretciEtkinlikKaydiYap
    @ZiyaretciID INT,
    @EtkinlikID INT,
    @KayitTarihi DATE
AS
BEGIN
    INSERT INTO EtkinlikKayitlari (ZiyaretciID, EtkinlikID, KayitTarihi)
    VALUES (@ZiyaretciID, @EtkinlikID, @KayitTarihi);
END;

EXEC sp_ZiyaretciEtkinlikKaydiYap
    @ZiyaretciID = 7,          -- �rnek: sistemde kay�tl� bir ziyaret�i
    @EtkinlikID = 2,           -- �rnek: var olan bir etkinlik
    @KayitTarihi = '2025-06-13';


--13.Eser g�ncelleme
CREATE PROCEDURE sp_EserGuncelle
    @ID INT,
    @Ad NVARCHAR(100),
    @Tur_ID INT,
    @Sanatci_ID INT,
    @YapimYili INT,
    @BulunduguMuze NVARCHAR(100),
    @MevcutDurum NVARCHAR(50),
    @DijitalKoleksiyonURL NVARCHAR(255)
AS
BEGIN
    UPDATE Eserler
    SET 
        Ad = @Ad,
        Tur_ID = @Tur_ID,
        Sanatci_ID = @Sanatci_ID,
        YapimYili = @YapimYili,
        BulunduguMuze = @BulunduguMuze,
        MevcutDurum = @MevcutDurum,
        DijitalKoleksiyonURL = @DijitalKoleksiyonURL
    WHERE ID = @ID;
END

EXEC sp_EserGuncelle
    @ID = 1,
    @Ad = N'Mona Lisa (Restorasyon Sonras�)',
    @Tur_ID = 1,
    @Sanatci_ID = 1,
    @YapimYili = 1503,
    @BulunduguMuze = N'Louvre M�zesi',
    @MevcutDurum = N'Restorasyonda',
    @DijitalKoleksiyonURL = N'https://koleksiyon.muze.com/mona-lisa-yeni';




--14. Yeni sergi ekleme
CREATE PROCEDURE sp_SergiEkle
    @Ad NVARCHAR(100),
    @Konum NVARCHAR(100),
    @BaslangicTarihi DATE,
    @BitisTarihi DATE,
    @Aciklama NVARCHAR(255)
AS
BEGIN
    INSERT INTO Sergiler (Ad, Konum, BaslangicTarihi, BitisTarihi, Aciklama)
    VALUES (@Ad, @Konum, @BaslangicTarihi, @BitisTarihi, @Aciklama);

	SELECT CAST(SCOPE_IDENTITY() AS INT); 
END

EXEC sp_SergiEkle
    @Ad = N'Dijital Gelecek Vizyonlar�',
    @Konum = N'Zorlu PSM',
    @BaslangicTarihi = '2025-11-01',
    @BitisTarihi = '2025-12-15',
    @Aciklama = N'Dijital ve art�r�lm�� ger�eklik eserlerinin yer ald��� tematik sergi';




--15.Personel ekleme
CREATE PROCEDURE sp_PersonelEkle
    @Ad NVARCHAR(50),
    @Soyad NVARCHAR(50),
    @DogumTarihi DATE,
    @Gorev NVARCHAR(50),
    @Maas DECIMAL(10,2),
    @IseBaslamaTarihi DATE
AS
BEGIN
    INSERT INTO Personel (Ad, Soyad, DogumTarihi, Gorev, Maas, IseBaslamaTarihi)
    VALUES (@Ad, @Soyad, @DogumTarihi, @Gorev, @Maas, @IseBaslamaTarihi);
	SELECT SCOPE_IDENTITY();
END

EXEC sp_PersonelEkle
    @Ad = N'B��ra',
    @Soyad = N'�zt�rk',
    @DogumTarihi = '1996-04-22',
    @Gorev = N'Eser Foto�raf��s�',
    @Maas = 8700.00,
    @IseBaslamaTarihi = '2025-06-01';

	

--16.Etkinlik turu ekleme
CREATE PROCEDURE sp_EtkinlikTuruEkle
    @Ad NVARCHAR(100),
    @Aciklama NVARCHAR(255)
AS
BEGIN
    INSERT INTO EtkinlikTurleri (Ad, Aciklama)
    VALUES (@Ad, @Aciklama);
END

EXEC sp_EtkinlikTuruEkle
    @Ad = N'VR Deneyimi',
    @Aciklama = N'Sanal ger�eklik tabanl� etkile�imli etkinlikler';




--17. Sanat��ya Ak�m G�ncelleme / Ekleme (Var olan varsa silip yeniden ekler)
CREATE PROCEDURE sp_SanatciyaAkimGuncelle
    @SanatciID INT,
    @YeniAkimID INT
AS
BEGIN
    DELETE FROM SanatciAkim WHERE SanatciID = @SanatciID;

    INSERT INTO SanatciAkim (SanatciID, AkimID)
    VALUES (@SanatciID, @YeniAkimID);
END

EXEC sp_SanatciyaAkimGuncelle
    @SanatciID = 5,  -- �rne�in Salvador Dal�
    @YeniAkimID = 14; -- Konsept�el Sanat gibi bir ak�m


--18.Eser silme
CREATE PROCEDURE sp_EserSil
    @ID INT
AS
BEGIN
    DELETE FROM Eserler
    WHERE ID = @ID;
END;

EXEC sp_EserSil
    @ID = 10;  -- �rnek: 10 numaral� eseri siler




--19.sergi g�ncelleme
CREATE PROCEDURE sp_SergiGuncelle
    @ID INT,
    @Ad NVARCHAR(100),
    @Konum NVARCHAR(100),
    @BaslangicTarihi DATE,
    @BitisTarihi DATE,
    @Aciklama NVARCHAR(255)
AS
BEGIN
    UPDATE Sergiler
    SET 
        Ad = @Ad,
        Konum = @Konum,
        BaslangicTarihi = @BaslangicTarihi,
        BitisTarihi = @BitisTarihi,
        Aciklama = @Aciklama
    WHERE ID = @ID;
END;

EXEC sp_SergiGuncelle
    @ID = 3,
    @Ad = N'Yenilenmi� Osmanl� Hat Sanat�',
    @Konum = N'Topkap� Saray�',
    @BaslangicTarihi = '2025-04-01',
    @BitisTarihi = '2025-06-01',
    @Aciklama = N'G�ncellenmi� a��klamalarla Osmanl� hat sanat�n�n zengin �rnekleri.';


--20.sergi silme
CREATE PROCEDURE sp_SergiSil
    @ID INT
AS
BEGIN
    DELETE FROM Sergiler
    WHERE ID = @ID;
END;

EXEC sp_SergiSil
    @ID = 5;  -- 5 numaral� sergiyi siler




--21.sanatc� g�ncelleme
CREATE PROCEDURE sp_SanatciGuncelle
    @ID INT,
    @Ad NVARCHAR(100),
    @DogumTarihi DATE,
    @OlumTarihi DATE = NULL,
    @Ulke NVARCHAR(50),
    @Biyografi TEXT
AS
BEGIN
    UPDATE Sanatcilar
    SET 
        Ad = @Ad,
        DogumTarihi = @DogumTarihi,
        OlumTarihi = @OlumTarihi,
        Ulke = @Ulke,
        Biyografi = @Biyografi
    WHERE ID = @ID;
END;

EXEC sp_SanatciGuncelle
    @ID = 12,
    @Ad = N'Ay�e Y�lmaz',
    @DogumTarihi = '1985-05-10',
    @OlumTarihi = NULL,
    @Ulke = N'T�rkiye',
    @Biyografi = N'G�ncellenmi� biyografi: Modern sanat ve sosyal konular �zerine yeni �al��malar�yla �ne ��k�yor.';




--22.personel bilgisi g�ncelleme
CREATE PROCEDURE sp_PersonelGuncelle
    @ID INT,
    @Ad NVARCHAR(50),
    @Soyad NVARCHAR(50),
    @DogumTarihi DATE,
    @Gorev NVARCHAR(50),
    @Maas DECIMAL(10,2),
    @IseBaslamaTarihi DATE
AS
BEGIN
    UPDATE Personel
    SET 
        Ad = @Ad,
        Soyad = @Soyad,
        DogumTarihi = @DogumTarihi,
        Gorev = @Gorev,
        Maas = @Maas,
        IseBaslamaTarihi = @IseBaslamaTarihi
    WHERE ID = @ID;
END;

EXEC sp_PersonelGuncelle
    @ID = 8,
    @Ad = N'Emre',
    @Soyad = N'Do�an',
    @DogumTarihi = '1989-05-28',
    @Gorev = N'�dari ��ler M�d�r�',
    @Maas = 12500.00,
    @IseBaslamaTarihi = '2013-05-19';



--23.etkinlik g�ncelleme
CREATE PROCEDURE sp_EtkinlikGuncelle
    @ID INT,
    @Ad NVARCHAR(100),
    @Tur NVARCHAR(50),
    @BaslangicTarihi DATE,
    @BitisTarihi DATE,
    @Aciklama NVARCHAR(255),
    @TurID INT
AS
BEGIN
    UPDATE Etkinlikler
    SET 
        Ad = @Ad,
        Tur = @Tur,
        BaslangicTarihi = @BaslangicTarihi,
        BitisTarihi = @BitisTarihi,
        Aciklama = @Aciklama,
        TurID = @TurID
    WHERE ID = @ID;
END;

EXEC sp_EtkinlikGuncelle
    @ID = 4,
    @Ad = N'�ocuklar ��in Yeni Tiyatro',
    @Tur = N'Tiyatro',
    @BaslangicTarihi = '2025-07-01',
    @BitisTarihi = '2025-07-02',
    @Aciklama = N'�ocuklara y�nelik interaktif tiyatro g�sterisi',
    @TurID = 7;  -- "Tiyatro" t�r�



--24.ziyaretci g�ncelleme
CREATE PROCEDURE sp_ZiyaretciGuncelle
    @ID INT,
    @Ad NVARCHAR(50),
    @Soyad NVARCHAR(50),
    @DogumTarihi DATE,
    @Email NVARCHAR(100),
    @UyelikDurumu BIT
AS
BEGIN
    UPDATE Ziyaretciler
    SET 
        Ad = @Ad,
        Soyad = @Soyad,
        DogumTarihi = @DogumTarihi,
        Email = @Email,
        UyelikDurumu = @UyelikDurumu
    WHERE ID = @ID;
END;

EXEC sp_ZiyaretciGuncelle
    @ID = 7,
    @Ad = N'Emre',
    @Soyad = N'�ahin',
    @DogumTarihi = '1980-08-30',
    @Email = N'yeni.emre@example.com',
    @UyelikDurumu = 1;


--25.sanat ak�m� ekle
CREATE PROCEDURE sp_SanatAkimiEkle
    @Ad NVARCHAR(100),
    @Aciklama NVARCHAR(255)
AS
BEGIN
    INSERT INTO SanatAkimlari (Ad, Aciklama)
    VALUES (@Ad, @Aciklama);

    SELECT SCOPE_IDENTITY();  -- Eklenen ID'yi d�nd�r
END
