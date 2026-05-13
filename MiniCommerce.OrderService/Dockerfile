FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "Directory.Build.props", "./"]
COPY ["MiniCommerce.Shared/MiniCommerce.Shared.csproj", "MiniCommerce.Shared/"]
COPY ["MiniCommerce.OrderService/MiniCommerce.OrderService.csproj", "MiniCommerce.OrderService/"]
RUN dotnet restore "MiniCommerce.OrderService/MiniCommerce.OrderService.csproj"
COPY ["MiniCommerce.Shared/", "MiniCommerce.Shared/"]
COPY ["MiniCommerce.OrderService/", "MiniCommerce.OrderService/"]
WORKDIR "/src/MiniCommerce.OrderService"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MiniCommerce.OrderService.dll"]