# collection-backend
Orginizes phisical collections.

## Install database
For this application to function you need to create your own SQLlite3 database.
The database needs to be created with this path: src/sqlDb.db.
The commands to be executed on the database are:
```SQL
CREATE TABLE Tools (
    id INTEGER PRIMARY KEY,
    name VARCHAR(200) NOT NULL UNIQUE,
    description TEXT,
    type VARCHAR(100) NOT NULL UNIQUE,
    electric BOOLEAN NOT NULL,
    productCode VARCHAR(200),
    EAN INTEGER,
    originalPrice REAL
);
CREATE TABLE Entries (
    id INTEGER PRIMARY KEY,
    toolId INTEGER NOT NULL,
    boughtPrice INTEGER,
    FOREIGN KEY(toolId) REFERENCES Tools (id)
);
CREATE TABLE Migrations (
    id INTEGER PRIMARY KEY,
    migrationVersion INTEGER NOT NULL
);      
```
After running the code above run the code below.
```SQL
INSERT INTO Migrations (migrationVersion) VALUES (1);
```