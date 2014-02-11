create table T_Report(
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	datetime timestamp,
	unitsname text,
	use text,
	income real,
	expenses real,
	type integer
);

create table T_Type(
	id integer,
	name text
);

CREATE TABLE "main"."T_Log" (
	"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	"TIME"  timestamp,
	"TITLE"  TEXT,
	"CONTENT"  TEXT,
	"REMARK"  TEXT,
	"TYPE"  TEXT
);