UPDATE Users SET
    ImageName = @ImageName,
    ImagePath = @ImagePath,
    UpdateDate = NOW()
WHERE Id = @Id