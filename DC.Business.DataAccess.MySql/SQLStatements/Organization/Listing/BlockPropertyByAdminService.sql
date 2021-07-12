UPDATE Property SET
    State = 3,
    UpdateDate = NOW()
WHERE Id = @Id