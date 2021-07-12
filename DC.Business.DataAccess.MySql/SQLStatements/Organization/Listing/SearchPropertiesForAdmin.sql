SELECT
    Id,
    UserId,
    Price,
    NetAream2,
    Typology,
    NumberOfBathrooms,
    Country,
    City,
    State
FROM Property 
WHERE Property.State = @Type 