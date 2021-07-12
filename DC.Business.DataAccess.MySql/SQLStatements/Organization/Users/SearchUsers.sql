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
WHERE Users.EMAIL LIKE CONCAT("%",@Email,"%") 
AND Users.NAME LIKE CONCAT("%",@Name,"%")
AND DELETED = 0