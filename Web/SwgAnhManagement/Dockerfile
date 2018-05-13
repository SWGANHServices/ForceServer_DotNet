FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 55299
EXPOSE 44394

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Web/SwgAnhManagement/SwgAnhManagement.csproj Web/SwgAnhManagement/
RUN dotnet restore Web/SwgAnhManagement/SwgAnhManagement.csproj
COPY . .
WORKDIR /src/Web/SwgAnhManagement
RUN dotnet build SwgAnhManagement.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SwgAnhManagement.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SwgAnhManagement.dll"]
