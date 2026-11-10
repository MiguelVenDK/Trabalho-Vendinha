CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Clients" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Clients" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Cpf" TEXT NOT NULL,
    "BirthDate" TEXT NOT NULL,
    "Email" TEXT NOT NULL
);

CREATE TABLE "Debts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Debts" PRIMARY KEY AUTOINCREMENT,
    "Value" decimal(18,2) NOT NULL,
    "IsPaid" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "PaidAt" TEXT NULL,
    "ClientId" INTEGER NOT NULL,
    CONSTRAINT "FK_Debts_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Clients_Cpf" ON "Clients" ("Cpf");

CREATE INDEX "IX_Debts_ClientId" ON "Debts" ("ClientId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261109155400_AjusteMigracao', '10.0.14');

COMMIT;

