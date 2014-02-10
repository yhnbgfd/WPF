create table T_Report(
	id integer,
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