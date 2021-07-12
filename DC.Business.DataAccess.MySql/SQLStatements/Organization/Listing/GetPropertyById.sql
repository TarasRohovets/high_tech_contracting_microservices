SELECT 
    Id,
    UserId,
    Price,
    NetAream2,
    Typology,
    NumberOfBathrooms,
    Country,
    City,
    Latitude,
    Longitude
FROM Property
WHERE Id = @id