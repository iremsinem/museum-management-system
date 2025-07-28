CREATE DATABASE Museum;

USE Museum;


CREATE TABLE EserTurleri (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(50),
    Aciklama NVARCHAR(255)
);

CREATE TABLE Sanatcilar (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(100),
    DogumTarihi DATE,
    OlumTarihi DATE NULL,
    Ulke NVARCHAR(50),
    Biyografi TEXT
);

CREATE TABLE Eserler (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(100),
    Tur_ID INT,
    Sanatci_ID INT,
    YapimYili INT,
    BulunduguMuze NVARCHAR(100),
    MevcutDurum NVARCHAR(50),
    DijitalKoleksiyonURL NVARCHAR(255),
    FOREIGN KEY (Tur_ID) REFERENCES EserTurleri(ID),
    FOREIGN KEY (Sanatci_ID) REFERENCES Sanatcilar(ID)
);

CREATE TABLE Eserler_Arsiv (
    ID INT,
    Ad NVARCHAR(100),
    Tur_ID INT,
    Sanatci_ID INT,
    YapimYili INT,
    BulunduguMuze NVARCHAR(100),
    MevcutDurum NVARCHAR(50),
    DijitalKoleksiyonURL NVARCHAR(200)
);


CREATE TABLE Sergiler (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(100),
    Konum NVARCHAR(100),
    BaslangicTarihi DATE,
    BitisTarihi DATE,
    Aciklama NVARCHAR(255)
);



CREATE TABLE EserSergileri (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    EserID INT,
    SergiID INT,
    BaslangicTarihi DATE,
    BitisTarihi DATE,
    FOREIGN KEY (EserID) REFERENCES Eserler(ID),
    FOREIGN KEY (SergiID) REFERENCES Sergiler(ID)
);

CREATE TABLE Personel (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(50),
    Soyad NVARCHAR(50),
	DogumTarihi DATE,
    Gorev NVARCHAR(50),
    Maas DECIMAL(10,2) MASKED WITH (FUNCTION = 'default()'),
    IseBaslamaTarihi DATE
);


CREATE TABLE EserTransferleri (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    EserID INT NOT NULL,
    KaynakMuze NVARCHAR(100),
    HedefMuze NVARCHAR(100),
    Tarih DATE,
    TransferDurumu NVARCHAR(50),
    FOREIGN KEY (EserID) REFERENCES Eserler(ID)
);


CREATE TABLE Ziyaretciler (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(50),
    Soyad NVARCHAR(50),
    DogumTarihi DATE,
    Email NVARCHAR(100) UNIQUE,
    UyelikDurumu BIT
);

CREATE TABLE BiletTurleri (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(50),
    Fiyat DECIMAL(10,2),
    GecerlilikSuresi INT
);

CREATE TABLE ZiyaretciGirisKayitlari ( 
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ZiyaretciID INT,
    GirisTarihi DATETIME,
    CikisTarihi DATETIME NULL,
    BiletTuru INT,
    SergiID INT NULL,
    EtkinlikID INT NULL,
    FOREIGN KEY (ZiyaretciID) REFERENCES Ziyaretciler(ID),
    FOREIGN KEY (BiletTuru) REFERENCES BiletTurleri(ID),
    FOREIGN KEY (SergiID) REFERENCES Sergiler(ID),
    FOREIGN KEY (EtkinlikID) REFERENCES Etkinlikler(ID)
);


CREATE TABLE Etkinlikler (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(100),
    Tur NVARCHAR(50),                  -- metinsel açýklama olarak tutulacak (görüntüleme vs. için)
    BaslangicTarihi DATE,
    BitisTarihi DATE,
    Aciklama NVARCHAR(255),
    TurID INT,                         -- iliþki için sayýsal foreign key
    FOREIGN KEY (TurID) REFERENCES EtkinlikTurleri(ID)
);


CREATE TABLE EtkinlikTurleri (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(100) NOT NULL, -- Türün adý (örn. Konser, Sergi)
    Aciklama NVARCHAR(255) -- Tür hakkýnda açýklama
);

CREATE TABLE EtkinlikKayitlari (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ZiyaretciID INT,
    EtkinlikID INT,
    KayitTarihi DATE,
    FOREIGN KEY (ZiyaretciID) REFERENCES Ziyaretciler(ID),
    FOREIGN KEY (EtkinlikID) REFERENCES Etkinlikler(ID)
);


CREATE TABLE Bagiscilar (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(50),
    Soyad NVARCHAR(50),
    Kurum NVARCHAR(100),
    Email NVARCHAR(100) MASKED WITH (FUNCTION ='email()'),
    Telefon NVARCHAR(20) MASKED WITH (FUNCTION ='default()')
);


CREATE TABLE Bagislar (
    ID INT IDENTITY(1,1) PRIMARY KEY,
	AdminID INT,
    BagisciID INT,
    Miktar DECIMAL(10,2) MASKED WITH (FUNCTION ='default()'),
    BagisTarihi DATE,
    KullanimAlani NVARCHAR(100),
    FOREIGN KEY (BagisciID) REFERENCES Bagiscilar(ID)
);



CREATE TABLE MuzeGelirleri (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    KaynakTuru NVARCHAR(50) NOT NULL,
    Tutar DECIMAL(10,2) MASKED WITH (FUNCTION ='default()'),
	Tarih DATE
);



CREATE TABLE MuzeGiderleri (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Aciklama NVARCHAR(255),
    Tutar DECIMAL(10,2) MASKED WITH (FUNCTION ='default()'),
    Tarih DATE
);

CREATE TABLE AylikFinansOzet (
    Ay DATE,
    ToplamGelir DECIMAL(10,2),
    ToplamGider DECIMAL(10,2)
);


CREATE TABLE EserBakimKayitlari (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    EserID INT NOT NULL,
	PersonelID    INT           NOT NULL,
    BakimTarihi DATE,
    YapilanIslem NVARCHAR(255),
    FOREIGN KEY (EserID) REFERENCES Eserler(ID),
    FOREIGN KEY (PersonelID) REFERENCES Personel(ID)
);


CREATE TABLE SanatAkimlari (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(100) NOT NULL,
    Aciklama NVARCHAR(255)
);


CREATE TABLE SanatciAkim (
    SanatciID INT,
    AkimID INT,
    PRIMARY KEY (SanatciID, AkimID),
    FOREIGN KEY (SanatciID) REFERENCES Sanatcilar(ID),
    FOREIGN KEY (AkimID) REFERENCES SanatAkimlari(ID)
);

CREATE TABLE Adminler (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    KullaniciAdi NVARCHAR(50) NOT NULL UNIQUE,
    SifreHash NVARCHAR(255) NOT NULL,
    Ad NVARCHAR(50),
    Soyad NVARCHAR(50),
    Email NVARCHAR(100),
    YetkiSeviyesi NVARCHAR(20) -- Örn: 'Tam Yetki', 'Sýnýrlý' vb.
);

CREATE TABLE Log (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    LogDetay NVARCHAR(255),
    AdminID INT,
    FOREIGN KEY (AdminID) REFERENCES Adminler(ID)
);


