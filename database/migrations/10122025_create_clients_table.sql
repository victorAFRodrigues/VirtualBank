PRAGMA foreign_keys = ON;
CREATE TABLE IF NOT EXISTS Clients (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    Email TEXT UNIQUE NOT NULL,
    Phone TEXT,
    Password TEXT NOT NULL,
    Cpf TEXT UNIQUE NOT NULL,
    AccountId TEXT NOT NULL,
    FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
);

