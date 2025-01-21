USE CheckInSystem
GO
CREATE PROCEDURE CardScanned @cardID char(11) AS
BEGIN 
    IF NOT EXISTS(SELECT * FROM employee WHERE cardID = @cardID) 
    BEGIN
        INSERT INTO employee(cardID) VALUES (@cardID)
    end
    
    EXEC CheckInOut @cardID
end