# FoodDelivery Backend API

Backend API cho hệ thống **đặt & giao đồ ăn**, xây dựng bằng **ASP.NET Core Web API**, hỗ trợ xác thực JWT, phân quyền người dùng và làm việc nhóm.

---

## Mục tiêu dự án

- Cung cấp API cho ứng dụng đặt đồ ăn
- Quản lý người dùng, vai trò (Admin / Staff / Customer / Shipper)
- Xác thực & phân quyền bằng JWT + Refresh Token
- Dễ mở rộng, dễ bảo trì, phù hợp làm việc nhóm

---

## 🛠 Công nghệ sử dụng

- **ASP.NET Core 8**
- **Entity Framework Core**
- **PostgreSQL**
- **JWT Bearer Authentication**
- **Swagger / OpenAPI**
- **CORS** (Frontend: React)

---

## Kiến trúc

Áp dụng mô hình **Clean Architecture / Layered Architecture**

### Nguyên tắc:

- Controller: xử lý HTTP, không viết logic nghiệp vụ
- Service: xử lý business logic
- Repository: truy vấn database
- UnitOfWork: quản lý transaction

---

## Cấu trúc thư mục

FoodDelivery
│
├── Controllers // API Controllers
├── Entities // Entity / Domain Models
├── DTOs // Request / Response DTO
│
├── Repositories
│ ├── Interfaces
│ └── Implementations
│
├── Service
│ ├── Interfaces
│ └── Implementations
│
├── Migrations
├── Program.cs
├── appsettings.json
└── README.md

---

## Hướng dẫn chạy dự án

### 1️ Clone source code

```bash
git clone https://github.com/your-username/food-delivery-backend.git
cd food-delivery-backend

### 2️ Cấu hình Database & JWT

{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=FoodDelivery;Username=postgres;Password=123456"
  },
  "TokenSecretKey": "YOUR_SUPER_SECRET_KEY",
}
### 3️ Chạy migration
dotnet ef database update

### 4 Cấu hình admin ban đầu (tùy chọn)

Để tạo tài khoản admin đầu tiên, cấu hình các biến môi trường sau trước khi chạy backend:

- `InitialAdmin__Email`
- `InitialAdmin__Password` (tối thiểu 12 ký tự)
- `InitialAdmin__FullName`
- `InitialAdmin__Phone` (10 chữ số, bắt đầu bằng `0`)

Ví dụ với PowerShell:

```powershell
$env:InitialAdmin__Email = "admin@example.com"
$env:InitialAdmin__Password = "<strong-unique-password>"
$env:InitialAdmin__FullName = "System Administrator"
$env:InitialAdmin__Phone = "0900000000"
```

Seeder tạo tài khoản khi email chưa tồn tại; nếu email đã có tài khoản thì chỉ gán quyền Admin, không thay đổi mật khẩu. Cần bảo đảm email cấu hình thuộc quyền kiểm soát của quản trị viên. Nếu không cấu hình các biến này, bước tạo admin được bỏ qua.

### 5 Run project
dotnet run
```
