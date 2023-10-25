[Found Here](https://hub.docker.com/_/microsoft-mssql-server)

Sql Server Database

```shell
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```