# Stage 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file project và restore dependencies
COPY ["FoodDelivery.csproj", "./"]
RUN dotnet restore "FoodDelivery.csproj"

# Copy toàn bộ source code trong thư mục backend vào container và build
COPY . .
RUN dotnet publish "FoodDelivery.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime (chạy ứng dụng nhẹ nhất)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Cấu hình port cho Render (Render sẽ truyền ngầm biến môi trường PORT)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "FoodDelivery.dll"]