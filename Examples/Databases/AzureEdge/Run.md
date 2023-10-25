[Found Here](https://hub.docker.com/_/microsoft-azure-sql-edge)
Azure SQL Edge is a small-footprint containerized database engine provided by Microsoft, optimized for edge computing.

```shell
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=yourStrong(!)Password' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
```