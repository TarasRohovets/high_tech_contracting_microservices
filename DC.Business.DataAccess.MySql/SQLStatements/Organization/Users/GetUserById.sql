SELECT
     Users.ID,
     Users.NAME,
     Users.SURNAME,
     Users.EMAIL,
     Users.CITY,
     Users.COUNTRY,
     Users.ADDRESS,
     Users.TAXNUMBER
FROM Users
WHERE (@Id IS NULL OR USERS.ID = @Id)
AND DELETED = 0