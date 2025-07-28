Use Museum;


CREATE ROLE Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON Eserler TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON EserSergileri TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON EserTransferleri TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON EserBakimKayitlari TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON EserTurleri TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON EtkinlikKayitlari TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON Etkinlikler TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON EtkinlikTurleri TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON MuzeGelirleri TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON MuzeGiderleri TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON Personel TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON SanatAkimlari TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON SanatciAkim TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON Sanatcilar TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON Sergiler TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON Bagiscilar TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON Bagislar TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON Ziyaretciler TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON ZiyaretciGirisKayitlari TO Admin;
GRANT SELECT , INSERT , DELETE , UPDATE ON BiletTurleri TO Admin;


CREATE ROLE Employee;
GRANT SELECT ON Eserler TO Employee;
GRANT SELECT ON EserSergileri TO Employee;
GRANT SELECT ON EserTransferleri TO Employee;
GRANT SELECT ON EserBakimKayitlari TO Employee;
GRANT SELECT ON EserTurleri TO Employee;
GRANT SELECT ON EtkinlikKayitlari TO Employee;
GRANT SELECT ON Etkinlikler TO Employee;
GRANT SELECT ON EtkinlikTurleri TO Employee;
GRANT SELECT ON SanatAkimlari TO Employee;
GRANT SELECT ON SanatciAkim TO Employee;
GRANT SELECT ON Sergiler TO Employee;
GRANT SELECT ON Ziyaretciler TO Employee;
GRANT SELECT ON ZiyaretciGirisKayitlari TO Employee;
GRANT SELECT ON BiletTurleri TO Employee;



CREATE ROLE Manager;
GRANT SELECT , INSERT ON Eserler TO  Manager;
GRANT SELECT , INSERT ON EserSergileri TO  Manager;
GRANT SELECT , INSERT ON EserTransferleri TO  Manager;
GRANT SELECT , INSERT ON EserBakimKayitlari TO  Manager;
GRANT SELECT , INSERT ON EserTurleri TO  Manager;
GRANT SELECT , INSERT ON EtkinlikKayitlari TO  Manager;
GRANT SELECT , INSERT ON Etkinlikler TO  Manager;
GRANT SELECT , INSERT ON EtkinlikTurleri TO  Manager;
GRANT SELECT , INSERT ON SanatAkimlari TO  Manager;
GRANT SELECT , INSERT ON SanatciAkim TO  Manager;
GRANT SELECT , INSERT ON Sergiler TO  Manager;
GRANT SELECT , INSERT ON Ziyaretciler TO  Manager;
GRANT SELECT , INSERT ON ZiyaretciGirisKayitlari TO  Manager;
GRANT SELECT , INSERT ON BiletTurleri TO  Manager;
GRANT SELECT , INSERT ON Bagiscilar TO  Manager;
GRANT SELECT , INSERT ON Bagislar TO  Manager;


EXEC sp_addrolemember 'Admin', 'guest';

