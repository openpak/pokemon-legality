FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /src

COPY PKHaX.csproj .

RUN dotnet restore "PKHaX.csproj"

COPY . .

RUN dotnet publish "PKHaX.csproj" -c Release -o /app/publish

FROM alpine:3.23.3
WORKDIR /app

ENV PKHAX_PORT=20170
ENV PKHAX_PRIVATE_KEY_PATH=/data/private.key
VOLUME /data
EXPOSE 20170

RUN apk add --no-cache icu-dev openssl-dev

COPY --from=build /app/publish .

ENTRYPOINT ["./PKHaX"]
