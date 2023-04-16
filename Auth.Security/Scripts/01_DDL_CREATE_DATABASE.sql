-- DROP DATABASE IF EXISTS security_dev;

CREATE DATABASE security_dev
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Portuguese_Brazil.1252'
    LC_CTYPE = 'Portuguese_Brazil.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

GRANT ALL ON DATABASE security_dev TO postgres;

GRANT TEMPORARY, CONNECT ON DATABASE security_dev TO PUBLIC;

GRANT ALL ON DATABASE security_dev TO garage474_dev;