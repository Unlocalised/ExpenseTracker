CREATE DATABASE marten_db;
CREATE USER marten WITH PASSWORD 'LUUcvHJHv22GE7e';
grant all privileges on database marten_db to marten;
\c marten_db marten
GRANT ALL ON SCHEMA public TO marten;