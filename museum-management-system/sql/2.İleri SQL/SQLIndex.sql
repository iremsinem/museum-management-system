Use Museum;

--INDEX
CREATE NONCLUSTERED INDEX Adminler_Ad_INDEX ON Adminler (Ad);  
CREATE NONCLUSTERED INDEX Adminler_Soyad_INDEX ON Adminler (Soyad);  

CREATE NONCLUSTERED INDEX Bagiscilar_Ad_INDEX ON Bagiscilar (Ad); 
CREATE NONCLUSTERED INDEX Bagiscilar_Soyad_INDEX ON Bagiscilar (Soyad); 

CREATE NONCLUSTERED INDEX Personel_Ad_Soyad_INDEX ON Personel (Ad, Soyad); 

CREATE NONCLUSTERED INDEX Eserler_Ad_INDEX ON Eserler (Ad); 

CREATE NONCLUSTERED INDEX Sanatcilar_Ulke_INDEX ON Sanatcilar (Ulke); 

CREATE NONCLUSTERED INDEX Eserler_YapimYili_INDEX ON Eserler (YapimYili); 

CREATE NONCLUSTERED INDEX Etkinlikler_Ad_INDEX ON Etkinlikler (Ad); 

CREATE NONCLUSTERED INDEX Etkinlikler_Tur_INDEX ON Etkinlikler (Tur); 

CREATE NONCLUSTERED INDEX SanatAkimlari_Ad_INDEX ON SanatAkimlari (Ad);