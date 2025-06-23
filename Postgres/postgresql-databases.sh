#!/bin/bash

set -e
set -u

psql -v ON_ERROR_STOP=1 --username "postgres" <<-EOSQL
    CREATE USER audit_service WITH PASSWORD 'LUUcvHJHv22GE7e';
    ALTER ROLE audit_service WITH CREATEDB;
EOSQL

psql -v ON_ERROR_STOP=1 --username "audit_service" --dbname "postgres" <<-EOSQL
    CREATE DATABASE audit_service_db;
    grant all privileges on database audit_service_db to audit_service;
EOSQL

psql -v ON_ERROR_STOP=1 --username "audit_service" --dbname "audit_service_db" <<-EOSQL
    GRANT ALL ON SCHEMA "public" TO audit_service;
EOSQL

psql -v ON_ERROR_STOP=1 --username "postgres" <<-EOSQL
    CREATE USER expanse_service WITH PASSWORD 'LUUcvHJHv22GE7e';
    ALTER ROLE expanse_service WITH CREATEDB;
EOSQL

psql -v ON_ERROR_STOP=1 --username "expanse_service" --dbname "postgres" <<-EOSQL
    CREATE DATABASE expanse_service_db;
    grant all privileges on database expanse_service_db to expanse_service;
EOSQL

psql -v ON_ERROR_STOP=1 --username "expanse_service" --dbname "expanse_service_db" <<-EOSQL
    GRANT ALL ON SCHEMA "public" TO expanse_service;
EOSQL

