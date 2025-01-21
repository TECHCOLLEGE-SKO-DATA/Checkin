USE CheckInSystem
GO
CREATE PROCEDURE CheckInOut @cardID char(11) AS
DECLARE @employeeID INT;
BEGIN 
    SET @employeeID = (SELECT ID FROM employee WHERE cardID = @cardID)
    
    IF EXISTS(SELECT * FROM onSiteTime WHERE employeeID = @employeeID AND departureTime is null)
    BEGIN
        UPDATE onSiteTime
        SET departureTime = GETDATE()
        WHERE employeeID = @employeeID AND departureTime is null
    END
    ELSE
    BEGIN 
       INSERT INTO onSiteTime(employeeID, arrivalTime)
        VALUES (@employeeID, GETDATE())
    END 
END