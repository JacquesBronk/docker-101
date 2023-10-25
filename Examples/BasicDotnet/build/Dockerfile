FROM mcr.microsoft.com/dotnet/aspnet:7.0

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*


WORKDIR /app

COPY ./publish .

ENTRYPOINT ["dotnet", "MyAwesomeMicroservice.dll"]