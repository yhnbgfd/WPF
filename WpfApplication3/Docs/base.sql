CREATE TABLE "main"."T_Report" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"DateTime"  TIMESTAMP,
"UnitsName"  TEXT DEFAULT NULL,
"Use"  TEXT DEFAULT NULL,
"Income"  REAL DEFAULT 0,
"Expenses"  REAL DEFAULT 0,
"Type"  INTEGER
);

CREATE TABLE "main"."T_User" (
"id"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"username"  TEXT NOT NULL,
"password"  TEXT,
"Status"  INTEGER DEFAULT 0,
"remark"  TEXT
);
insert into main.T_User(username,password) values('admin','123');

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
);