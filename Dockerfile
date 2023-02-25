FROM mcr.microsoft.com/dotnet/sdk:6.0.202 AS build-env

WORKDIR /app
COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build-env /app/dist .

ENV Database_Connection="put your connection string"
ENV Origin="put your Origin"
ENV JWTOKEN_DECRYPT="put your token decrypt key"
ENV JWREFRESHTOKEN_DECRYPT="put your refreshToken decrypt key"

ENTRYPOINT [ "dotnet", "Webapi.dll" ]