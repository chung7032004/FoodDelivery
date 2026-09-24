# Stage 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file .csproj và restore
COPY ["FoodDelivery.csproj", "./"]
RUN dotnet restore "FoodDelivery.csproj"

# Copy toàn bộ code và publish
COPY . .
RUN dotnet publish "FoodDelivery.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render sẽ tự cấp cổng PORT ngầm, thiết lập để ASP.NET lắng nghe cổng này
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "FoodDelivery.dll"]