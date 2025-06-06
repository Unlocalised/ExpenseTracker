#!/bin/bash
cat > ~/.pgpass <<EOL
localhost:5432:postgres:postgres:postgres
localhost:5432:marten_db:marten:LUUcvHJHv22GE7e
EOL
chmod 600 ~/.pgpass

set -e
set -u

psql -v ON_ERROR_STOP=1 --username "postgres" <<-EOSQL
    CREATE USER marten WITH PASSWORD 'LUUcvHJHv22GE7e';
    ALTER ROLE marten WITH CREATEDB 
EOSQL

psql -v ON_ERROR_STOP=1 --username "marten" --dbname "postgres" <<-EOSQL
    CREATE DATABASE marten_db;
    grant all privileges on database marten_db to marten;
EOSQL

psql -v ON_ERROR_STOP=1 --username "marten" --dbname "marten_db" <<-EOSQL
    GRANT ALL ON SCHEMA "public" TO marten;
EOSQL
