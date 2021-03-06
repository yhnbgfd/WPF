CREATE TABLE "main"."T_Report" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"DateTime"  TIMESTAMP,
"UnitsName"  TEXT DEFAULT NULL,
"Use"  TEXT DEFAULT NULL,
"Income"  decimal DEFAULT 0,
"Expenses"  decimal DEFAULT 0,
"Type"  INTEGER,
"DeleteTime"  TIMESTAMP DEFAULT NULL
)
;

CREATE TABLE "main"."T_User" (
"id"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"username"  TEXT NOT NULL,
"password"  TEXT,
"Status"  INTEGER DEFAULT 0,
"remark"  TEXT
)
;

CREATE TABLE "main"."T_Log" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"TIME"  timestamp,
"TITLE"  TEXT,
"CONTENT"  TEXT,
"REMARK"  TEXT,
"TYPE"  TEXT
)
;

CREATE TABLE "main"."T_Surplus" (
"id"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"year"  INTEGER,
"month"  INTEGER,
"surplus"  REAL DEFAULT 0,
"type"  INTEGER
)
;

CREATE TABLE "main"."T_Type" (
"id"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"key"  INTEGER NOT NULL,
"value"  TEXT
)
;

CREATE INDEX "main"."Index_T_Report_DateTime"
ON "T_Report" ("DateTime" ASC);

CREATE INDEX "main"."Index_T_Report_Type"
ON "T_Report" ("Type" ASC);

