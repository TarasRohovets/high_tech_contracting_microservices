SELECT
     Users.ID,
     Users.NAME,
     Users.SURNAME,
     Users.EMAIL,
     Users.CITY,
     Users.COUNTRY,
     Users.ADDRESS,
     Users.Type,
     Users.TAXNUMBER,
     Users.ACTIVE,
     Users.IMAGEPATH,
     Users.IMAGENAME
FROM Users
WHERE Users.EMAIL = @Email
AND DELETED = 0