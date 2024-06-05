# syntax=docker/dockerfile:1
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
COPY . /source
WORKDIR /source
ARG TARGETARCH
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development
COPY --from=build-env /Market/out .
ENTRYPOINT ["dotnet", "/app/Market.dll"]
