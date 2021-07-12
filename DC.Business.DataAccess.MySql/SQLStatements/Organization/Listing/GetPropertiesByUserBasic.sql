SELECT 
    Id,
    UserId,
    Price,
    NetAream2,
    Typology,
    NumberOfBathrooms,
    Country,
    City
FROM Property
WHERE UserId = @userId
