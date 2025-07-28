Use Museum;



-- Yaln�zca AdminID e�le�en veriler g�r�nt�lenecek
CREATE FUNCTION dbo.fn_RLS_BagisFiltre (@AdminID INT)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
    SELECT 1 AS Result
    WHERE @AdminID = CAST(SESSION_CONTEXT(N'AdminID') AS INT);

-- Politika tan�m�
CREATE SECURITY POLICY BagisGormePolitikasi
ADD FILTER PREDICATE dbo.fn_RLS_BagisFiltre(AdminID) ON dbo.Bagislar
WITH (STATE = ON);


