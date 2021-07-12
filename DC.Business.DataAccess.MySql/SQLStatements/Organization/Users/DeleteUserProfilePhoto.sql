UPDATE Users SET
    ImageName = NULL,
    ImagePath = NULL,
    UpdateDate = NOW()
WHERE Id = @Id