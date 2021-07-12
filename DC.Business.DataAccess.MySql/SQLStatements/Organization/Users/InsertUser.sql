INSERT INTO Users (
    NAME,
    DELETED,
    PASSWD,
    EMAIL,
    TYPE,
    CREATIONDATE,
    UPDATEDATE
)
VALUES (
    @Name,
    0,
    @hashedPassword,
    @Email,
    @Type,
    @CreationDate,
    @UpdateDate);

SELECT LAST_INSERT_ID();
