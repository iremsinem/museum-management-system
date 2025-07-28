Use Museum;


--

--Eserin durumu �Restorasyonda� olarak g�ncellenirken e� zamanl� olarak bak�m kayd� da eklensin.
--Ama iki i�lemden biri ba�ar�s�z olursa hi�biri uygulanmas�n
BEGIN TRY
    BEGIN TRANSACTION;

    -- 1. Eserin durumu g�ncelleniyor
    UPDATE Eserler
    SET MevcutDurum = 'Restorasyonda'
    WHERE ID = 15;

    -- 2. Ayn� eser i�in bak�m kayd� ekleniyor
    INSERT INTO EserBakimKayitlari (EserID, BakimTarihi, YapilanIslem)
    VALUES (12, GETDATE(), 'Y�zey temizli�i ve �atlak onar�m�');

    -- Her �ey ba�ar�l�ysa i�lemi tamamla
    COMMIT;
END TRY
BEGIN CATCH
    -- Hata varsa i�lemi geri al
    ROLLBACK;

    -- Hatan�n ne oldu�unu g�ster (debug i�in)
    THROW;
END CATCH;





--yeni bir sanat��n�n eklenmesiyle birlikte ona ait ilk eserin de veritaban�na kay�t edilmesini
BEGIN TRY
    BEGIN TRANSACTION;

    -- 1. Yeni sanat��y� ekle
    INSERT INTO Sanatcilar (Ad, DogumTarihi, OlumTarihi, Ulke, Biyografi)
    VALUES ('Nil Kara', '1980-05-10', NULL, 'T�rkiye', 'Modern d�nemde dijital sanat �zerine �al��an sanat��.');

    -- 2. Son eklenen sanat��n�n ID'sini al
    DECLARE @SanatciID INT = SCOPE_IDENTITY();

    -- 3. O sanat��n�n ilk eserini ekle
    INSERT INTO Eserler (Ad, Tur_ID, Sanatci_ID, YapimYili, BulunduguMuze, MevcutDurum, DijitalKoleksiyonURL)
    VALUES ('Yans�ma I����', 2, @SanatciID, 2023, '�stanbul Dijital Sanatlar M�zesi', 'Sergileniyor', 'http://ornekurl.com/yansima.jpg');

    -- 4. ��lem ba�ar�l�ysa tamamla
    COMMIT;
END TRY
BEGIN CATCH
    -- Hata varsa i�lemi geri al
    ROLLBACK;

    -- Hatan�n detay�n� g�ster
    THROW;
END CATCH;




--Yeni bir sergi, ayn� anda bir veya daha fazla eserle ili�kilendiriliyor
BEGIN TRY
    BEGIN TRANSACTION;

    -- 1. Yeni sergi ekleniyor
    INSERT INTO Sergiler (Ad, Konum, BaslangicTarihi, BitisTarihi, Aciklama)
    VALUES ('Zaman�n �zleri', '�stanbul Modern', '2025-07-01', '2025-09-30', 'Modern d�nem sanat eserlerinden olu�an �zel koleksiyon.');

    -- 2. Eklenen serginin ID�sini al
    DECLARE @SergiID INT = SCOPE_IDENTITY();

    -- 3. Bu sergiye baz� eserleri ba�la (�rnek olarak ID'leri bilinen 2 eser: 10 ve 12)
    INSERT INTO EserSergileri (EserID, SergiID, BaslangicTarihi, BitisTarihi)
    VALUES 
        (10, @SergiID, '2025-07-01', '2025-09-30'),
        (12, @SergiID, '2025-07-01', '2025-09-30');

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    THROW;
END CATCH;
