FROM microsoft/dotnet:2.0-sdk as build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy and build everything else
COPY . ./
RUN dotnet publish -c Release -o out
FROM microsoft/dotnet:2.0-sdk
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "SwgAnh.Docker.dll"]
