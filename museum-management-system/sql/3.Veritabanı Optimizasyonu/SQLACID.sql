Use Museum;


--

--Eserin durumu “Restorasyonda” olarak güncellenirken eþ zamanlý olarak bakým kaydý da eklensin.
--Ama iki iþlemden biri baþarýsýz olursa hiçbiri uygulanmasýn
BEGIN TRY
    BEGIN TRANSACTION;

    -- 1. Eserin durumu güncelleniyor
    UPDATE Eserler
    SET MevcutDurum = 'Restorasyonda'
    WHERE ID = 15;

    -- 2. Ayný eser için bakým kaydý ekleniyor
    INSERT INTO EserBakimKayitlari (EserID, BakimTarihi, YapilanIslem)
    VALUES (12, GETDATE(), 'Yüzey temizliði ve çatlak onarýmý');

    -- Her þey baþarýlýysa iþlemi tamamla
    COMMIT;
END TRY
BEGIN CATCH
    -- Hata varsa iþlemi geri al
    ROLLBACK;

    -- Hatanýn ne olduðunu göster (debug için)
    THROW;
END CATCH;





--yeni bir sanatçýnýn eklenmesiyle birlikte ona ait ilk eserin de veritabanýna kayýt edilmesini
BEGIN TRY
    BEGIN TRANSACTION;

    -- 1. Yeni sanatçýyý ekle
    INSERT INTO Sanatcilar (Ad, DogumTarihi, OlumTarihi, Ulke, Biyografi)
    VALUES ('Nil Kara', '1980-05-10', NULL, 'Türkiye', 'Modern dönemde dijital sanat üzerine çalýþan sanatçý.');

    -- 2. Son eklenen sanatçýnýn ID'sini al
    DECLARE @SanatciID INT = SCOPE_IDENTITY();

    -- 3. O sanatçýnýn ilk eserini ekle
    INSERT INTO Eserler (Ad, Tur_ID, Sanatci_ID, YapimYili, BulunduguMuze, MevcutDurum, DijitalKoleksiyonURL)
    VALUES ('Yansýma Iþýðý', 2, @SanatciID, 2023, 'Ýstanbul Dijital Sanatlar Müzesi', 'Sergileniyor', 'http://ornekurl.com/yansima.jpg');

    -- 4. Ýþlem baþarýlýysa tamamla
    COMMIT;
END TRY
BEGIN CATCH
    -- Hata varsa iþlemi geri al
    ROLLBACK;

    -- Hatanýn detayýný göster
    THROW;
END CATCH;




--Yeni bir sergi, ayný anda bir veya daha fazla eserle iliþkilendiriliyor
BEGIN TRY
    BEGIN TRANSACTION;

    -- 1. Yeni sergi ekleniyor
    INSERT INTO Sergiler (Ad, Konum, BaslangicTarihi, BitisTarihi, Aciklama)
    VALUES ('Zamanýn Ýzleri', 'Ýstanbul Modern', '2025-07-01', '2025-09-30', 'Modern dönem sanat eserlerinden oluþan özel koleksiyon.');

    -- 2. Eklenen serginin ID’sini al
    DECLARE @SergiID INT = SCOPE_IDENTITY();

    -- 3. Bu sergiye bazý eserleri baðla (örnek olarak ID'leri bilinen 2 eser: 10 ve 12)
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
