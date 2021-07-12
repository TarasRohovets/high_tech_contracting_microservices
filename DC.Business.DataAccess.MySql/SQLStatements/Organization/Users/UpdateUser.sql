UPDATE Users SET
    Name = @Name,
    Surname = @Surname,
    Email = @Email,
    City = @City,
    Country = @Country,
    Address = @Address,
    TaxNumber = @TaxNumber,
    Active = @Active,
    UpdateDate = NOW()
WHERE Id = @Id
