UPDATE Property SET
    State = 2,
    UpdateDate = NOW()
WHERE Id = @Id