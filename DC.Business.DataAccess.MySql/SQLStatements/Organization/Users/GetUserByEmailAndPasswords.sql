SELECT
     ID,
     NAME,
     EMAIL,
     CREATIONDATE,
     UPDATEDATE,
     DELETED
FROM Users
WHERE Email = @Email
AND PASSWD IN @HashedPasswords
AND DELETED = 0