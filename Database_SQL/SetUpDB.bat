sqlcmd -S localhost\MSSQLSERVER -E -i Tables\Table_Creation.sql

for /f %%f in ('dir /b Functions') do sqlcmd -S localhost\MSSQLSERVER -E -i Functions\%%f
for /f %%f in ('dir /b StoredProcedures') do sqlcmd -S localhost\MSSQLSERVER -E -i StoredProcedures\%%f