PRAGMA foreign_keys = ON;
CREATE TABLE IF NOT EXISTS Accounts (
    Id TEXT PRIMARY KEY,
    Balance DECIMAL NOT NULL DEFAULT 0,
    Currency TEXT NOT NULL,
    TransferKey TEXT UNIQUE
);