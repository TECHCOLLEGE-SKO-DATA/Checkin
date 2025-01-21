USE CheckInSystem
GO
CREATE FUNCTION IsEmployeeCheckedIn (@EmployeeID INT) RETURNS BIT
AS 
BEGIN 
    DECLARE @IsCheckedIn BIT;
    if exists(SELECT * FROM onSiteTime WHERE employeeID = @EmployeeID AND departureTime IS NULL) 
    BEGIN
        SET @IsCheckedIn = 1
    END 
    ELSE
    BEGIN 
       Set @IsCheckedIn = 0
    END
    RETURN @IsCheckedIn
END