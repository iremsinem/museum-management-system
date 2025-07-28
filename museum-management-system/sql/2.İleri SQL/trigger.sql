SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--personel ya� s�n�r�
CREATE TRIGGER Personel_Yas_Kontrolu 
   ON Museum.dbo.Personel 
   AFTER INSERT
AS 
BEGIN

	IF (Yas < 18)
	RETURN;
        RAISERROR('Personel ya�� 18den k���k olamaz',10 ,1)
		ROLLBACK

END
GO

CREATE TRIGGER Personel_Yas_Kontrolu 
   ON Museum.dbo.Personel 
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE DATEDIFF(YEAR, DogumTarihi, GETDATE()) < 18
    )
    BEGIN
        RAISERROR('Personel ya�� 18''den k���k olamaz', 16, 1);
        ROLLBACK TRANSACTION;
    END
END

--yeni admin eklenmesi
CREATE TRIGGER Adminler_Log 
   ON Museum.dbo.Log 
   AFTER INSERT
AS 
BEGIN

	INSERT INTO Log (LogDetay)
	SELECT 'Yeni admin eklendi'

END
GO



--yeni eser eklenmesi
CREATE TRIGGER Eserler_Log 
   ON Museum.dbo.Eserler 
   AFTER INSERT
AS 
BEGIN

	INSERT INTO Log (LogDetay)
	SELECT 'Yeni eser eklendi' + Ad

END
GO
--eser silinmesi
CREATE TRIGGER Eserler_Silme_Log
ON Museum.dbo.Eserler
AFTER DELETE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Eser silindi: ' + Ad
    FROM deleted
END
GO
--eser g�ncelleme
CREATE TRIGGER Eserler_Guncelleme_Log
ON Museum.dbo.Eserler
AFTER UPDATE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Eser g�ncellendi: ' + i.Ad
    FROM inserted i
END
GO



--yeni sergi ekleme
CREATE TRIGGER Sergiler_Ekleme_Log
ON Museum.dbo.Sergiler
AFTER INSERT
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Yeni sergi eklendi: ' + Ad
    FROM inserted
END
GO
--sergi g�ncelleme
CREATE TRIGGER Sergiler_Guncelleme_Log
ON Museum.dbo.Sergiler
AFTER UPDATE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Sergi g�ncellendi: ' + i.Ad
    FROM inserted i
END
GO
--sergi silme
CREATE TRIGGER Sergiler_Silme_Log
ON Museum.dbo.Sergiler
AFTER DELETE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Sergi silindi: ' + Ad
    FROM deleted
END
GO



--yeni sanatc� ekleme
CREATE TRIGGER Sanatcilar_Ekleme_Log
ON Museum.dbo.Sanatcilar
AFTER INSERT
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Yeni sanat�� eklendi: ' + Ad
    FROM inserted
END
GO
--sanatci silme
CREATE TRIGGER Sanatcilar_Silme_Log
ON Museum.dbo.Sanatcilar
AFTER DELETE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Sanat�� silindi: ' + Ad
    FROM deleted
END
GO
--sanatc� g�ncelleme
CREATE TRIGGER Sanatcilar_Guncelleme_Log
ON Museum.dbo.Sanatcilar
AFTER UPDATE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Sanat�� g�ncellendi: ' + i.Ad
    FROM inserted i
END
GO



--bagisci ekleme
CREATE TRIGGER Bagiscilar_Ekleme_Log
ON Museum.dbo.Bagiscilar
AFTER INSERT
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Yeni ba����� eklendi: ' + Ad + ' ' + Soyad
    FROM inserted
END
GO
--bagisci silme
CREATE TRIGGER Bagiscilar_Silme_Log
ON Museum.dbo.Bagiscilar
AFTER DELETE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Ba����� silindi: ' + Ad + ' ' + Soyad
    FROM deleted
END
GO
--bagisci g�ncelleme
CREATE TRIGGER Bagiscilar_Guncelleme_Log
ON Museum.dbo.Bagiscilar
AFTER UPDATE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Ba����� g�ncellendi: ' + i.Ad + ' ' + i.Soyad
    FROM inserted i
END
GO




--yeni etkinlik
CREATE TRIGGER Etkinlikler_Ekleme_Log
ON Museum.dbo.Etkinlikler
AFTER INSERT
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Yeni etkinlik eklendi: ' + Ad
    FROM inserted
END
GO
--etkinlik silme
CREATE TRIGGER Etkinlikler_Silme_Log
ON Museum.dbo.Etkinlikler
AFTER DELETE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Etkinlik silindi: ' + Ad
    FROM deleted
END
GO
--etkinlik g�ncelleme
CREATE TRIGGER Etkinlikler_Guncelleme_Log
ON Museum.dbo.Etkinlikler
AFTER UPDATE
AS
BEGIN
    INSERT INTO Log (LogDetay)
    SELECT 'Etkinlik g�ncellendi: ' + i.Ad
    FROM inserted i
END
GO
