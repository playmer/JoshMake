# this is a simple mockup of what a database would
# look like for the DAG. It gives us an easy way to 
# see what files need to be updated, allowing for optimazation of unessisary recompilation



CREATE TABLE IF NOT EXISTS File(
GUID            int NOT NULL, 
Name            varchar(255) NOT NULL,
OutputFile      varchar(255) NOT NULL,
Timestamp		date
PRIMARY KEY(guid),
UNIQUE(Username)
) ENGINE = INNODB;

CREATE TABLE IF NOT EXISTS Dependencies(
fileGUID,	int NOT NULL,
depGUID		int NOT NULL,
PRIMARY KEY(fileGUID, depGUID)
)