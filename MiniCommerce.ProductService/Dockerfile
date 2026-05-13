FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "Directory.Build.props", "./"]
COPY ["MiniCommerce.Shared/MiniCommerce.Shared.csproj", "MiniCommerce.Shared/"]
COPY ["MiniCommerce.ProductService/MiniCommerce.ProductService.csproj", "MiniCommerce.ProductService/"]
RUN dotnet restore "MiniCommerce.ProductService/MiniCommerce.ProductService.csproj"
COPY ["MiniCommerce.Shared/", "MiniCommerce.Shared/"]
COPY ["MiniCommerce.ProductService/", "MiniCommerce.ProductService/"]
WORKDIR "/src/MiniCommerce.ProductService"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MiniCommerce.ProductService.dll"]