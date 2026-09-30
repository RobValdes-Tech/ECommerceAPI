FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ECommerceApi.csproj", "./"]
RUN dotnet restore "./ECommerceApi.csproj"
COPY . .
RUN dotnet publish "ECommerceApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ECommerceApi.dll"]
