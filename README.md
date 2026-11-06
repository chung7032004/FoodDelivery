# 🍔 FoodDelivery Platform

Hệ thống đặt & giao đồ ăn trực tuyến đa người dùng (**Full-stack Food Ordering & Delivery System**), được xây dựng với kiến trúc **Clean / Layered Architecture** trên nền tảng **ASP.NET Core 8 Web API**, cơ sở dữ liệu **PostgreSQL**, và giao diện người dùng **React 18 + TypeScript + Vite + Tailwind CSS**.

Hệ thống hỗ trợ toàn diện 4 nhóm tác nhân: **Khách hàng (Customer)**, **Chủ nhà hàng & Nhân viên (Restaurant Owner & Staff)**, **Tài xế (Shipper)** và **Quản trị viên (Admin)** với cơ chế điều phối thời gian thực qua **SignalR**.

---

## 📑 Mục lục
1. [Điểm nổi bật & Mẫu thiết kế (Design Patterns)](#-điểm-nổi-bật--mẫu-thiết-kế-design-patterns)
2. [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
3. [Các phân hệ nghiệp vụ chính](#-các-phân-hệ-nghiệp-vụ-chính)
4. [Cấu trúc dự án](#-cấu-trúc-dự-án)
5. [Yêu cầu môi trường](#-yêu-cầu-môi-trường)
6. [Hướng dẫn cài đặt & Khởi chạy](#-hướng-dẫn-cài-đặt--khởi-chạy)
   - [1. Clone mã nguồn](#1-clone-mã-nguồn)
   - [2. Cấu hình & Chạy Backend](#2-cấu-hình--chạy-backend)
   - [3. Cấu hình & Chạy Frontend](#3-cấu-hình--chạy-frontend)
7. [Tài khoản Quản trị viên khởi tạo](#-tài-khoản-quản-trị-viên-khởi-tạo-initial-admin)
8. [Kiểm thử tự động (Unit Tests)](#-kiểm-thử-tự-động-unit-tests)
9. [Tài liệu API & SignalR Hub](#-tài-liệu-api--signalr-hub)

---

## 🧩 Điểm nổi bật & Mẫu thiết kế (Design Patterns)

* **Finite State Machine (FSM) - Quản lý vòng đời đơn hàng (`OrderStateMachine.cs`)**:
  Quản lý chuyển đổi trạng thái đơn hàng nghiêm ngặt qua bảng trạng thái tập trung (`Pending` ➔ `Confirmed` / `WaitingCustomerDecision` ➔ `Preparing` ➔ `ReadyForPickup` ➔ `Shipping` ➔ `Completed` / `Cancelled`). Kiểm tra quyền của từng tác nhân (`ActorRole`) trước khi thực hiện chuyển đổi.
* **Xử lý Race Condition & Tranh chấp nhận đơn (Concurrency Control)**:
  Sử dụng kỹ thuật **Atomic Conditional Update** (`ExecuteUpdateAsync` với điều kiện lọc tại database) kết hợp **PostgreSQL Transaction Advisory Lock** (`pg_advisory_xact_lock`) để đảm bảo:
  - Khi nhiều shipper cùng bấm nhận 1 đơn, chỉ duy nhất 1 shipper nhận thành công.
  - Một shipper không thể "ôm" cùng lúc nhiều đơn đang xử lý.
  - Chặn triệt để lỗ hổng shipper tự nhận đơn do chính tài khoản của mình đặt.
* **Template Method & Strategy Pattern - Tính phí ship thông minh (`ShippingFeeCalculatorTemplate.cs`)**:
  Quy chuẩn hóa quy trình tính phí: *Lấy lộ trình ➔ Phí cơ bản ➔ Áp dụng phụ phí ➔ Làm tròn ➔ Giới hạn trần*. Triển khai đa dạng chiến lược giao hàng (`Standard` và `Express`).
* **Pipeline / Decorator Pattern - Tính phụ phí động (`SurchargePipeline.cs`)**:
  Tự động áp dụng phụ phí giờ cao điểm (`PeakHourSurcharge`), phụ phí ban đêm (`NightSurcharge`), hoặc miễn phí giao hàng (`FreeShipOverAmountSurcharge`).
* **Proxy / Cache-Aside Pattern - Định tuyến lộ trình (`CachedRoutingService.cs`)**:
  Tích hợp bản đồ OpenRouteService tính khoảng cách thực tế, có MemoryCache / Redis cache tọa độ và cơ chế tự động Fallback bằng công thức Haversine ($\times 1.33$) khi API bản đồ gặp sự cố.
* **Dynamic Permission-based & Role-based Access Control (RBAC)**:
  Phân quyền động theo từng chức năng (`PermissionHandler`, `PermissionPolicyProvider`), hỗ trợ tạm khóa vai trò (`UserRoleRestriction`) và quy trình gửi khiếu nại mở lại vai trò (`RoleAppeals`).
* **Realtime Event-Driven Communication**:
  Đồng bộ thông báo tức thì đến người dùng và cập nhật trạng thái đơn hàng thời gian thực qua **SignalR** (`NotificationHub`).

---

## 🛠 Công nghệ sử dụng

### Backend
* **Nền tảng**: .NET 8 (ASP.NET Core Web API)
* **ORM & Database**: Entity Framework Core 8, PostgreSQL (Npgsql)
* **Xác thực & Bảo mật**: JWT Bearer Tokens, Refresh Token, BCrypt, OTP Email (MailKit/MimeKit)
* **Real-time & Caching**: SignalR, MemoryCache, Redis (`StackExchange.Redis`)
* **Kiểm thử**: xUnit, Moq, Microsoft.Data.Sqlite In-Memory
* **Tài liệu API**: Swagger / OpenAPI (Swashbuckle)

### Frontend
* **Core**: React 18, TypeScript, Vite
* **Routing & State**: React Router v6, TanStack Query (React Query v5)
* **Styling & UI**: Tailwind CSS v4, Lucide React, Sonner (Toast Notifications)
* **Bản đồ & Tọa độ**: Leaflet, React-Leaflet
* **Giao tiếp API & Realtime**: Axios (với Interceptor tự động refresh token), `@microsoft/signalr`

---

## 👥 Các phân hệ nghiệp vụ chính

### 1. Khách hàng (Customer)
* Xem danh mục, tìm kiếm, lọc danh sách món ăn, xem trang nhà hàng.
* Chọn địa chỉ nhận hàng trực quan bằng ghim tọa độ trên bản đồ tương tác Leaflet.
* Xem chi tiết phí ship và khoảng cách thực tế từ quán đến nơi nhận trước khi đặt đơn.
* Đặt hàng, hỗ trợ thanh toán: Tiền mặt (COD), Thẻ ngân hàng, Ví điện tử.
* Theo dõi tiến độ giao hàng và xem lịch sử thời gian giao nhận chi tiết (`OrderDeliveryHistoryModal`).
* Tiếp nhận đề xuất thay đổi món khi nhà hàng báo hết hàng (`OutOfStockProposalModal`).
* Đổi mật khẩu, khôi phục mật khẩu qua mã OTP gửi về Email.

### 2. Nhà hàng (Restaurant Owner & Staff)
* Quy trình nộp đơn đăng ký mở quán ăn, chờ Admin xét duyệt hồ sơ.
* Quản lý thực đơn: Thêm, sửa, xóa món ăn, tải ảnh món ăn, cài đặt giá bán và giá vốn.
* Tiếp nhận đơn hàng, ước tính thời gian nấu (`prepMinutes`).
* Chức năng báo hết món (`ReportOutOfStock`) cho khách hàng.
* Chuyển trạng thái chế biến (`StartPreparing`) và đánh dấu món đã nấu xong (`MarkReady` / `ReadyForPickup`).
* Phân quyền và quản lý nhân viên quán (`Staff`).

### 3. Tài xế (Shipper)
* Đăng ký hồ sơ tài xế (CCCD, loại xe, biển số), chờ Admin xét duyệt.
* Bật/Tắt trạng thái sẵn sàng làm việc (Online / Offline).
* Xem danh sách các đơn hàng đang tìm tài xế trong khu vực.
* Nhận đơn vận chuyển (hệ thống tự động khóa không cho nhận thêm đơn khác khi đang có đơn hoạt động).
* Đến quán nhận món (`Pickup` ➔ `Shipping`).
* Xác nhận giao hàng thành công (`Deliver` ➔ `Completed`) hoặc báo giao hàng thất bại (`DeliveryFailed`).
* Thống kê cuốc xe và thu nhập theo ngày/tổng thu nhập.

### 4. Quản trị viên (Admin)
* Bảng điều khiển (Dashboard) thống kê tổng quan hệ thống.
* Xét duyệt hồ sơ mở quán ăn của các đối tác nhà hàng.
* Xét duyệt hồ sơ đăng ký tài xế shipper.
* Quản lý người dùng, phân quyền, khóa vai trò và duyệt khiếu nại mở lại vai trò (`RoleAppeals`).
* Giám sát toàn bộ đơn hàng, danh mục hệ thống và đánh giá món ăn.

---

## 📁 Cấu trúc dự án

```text
FoodDelivery/
├── backend/                             # ASP.NET Core 8 Web API
│   ├── Common/                          # Result pattern, Error codes, Options
│   ├── Constants/                       # Permissions, Roles, Enums
│   ├── Controller/                      # API Endpoints (Auth, Order, Shipper,...)
│   ├── Data/                            # DbContext, Entity Configurations, Seeder
│   ├── DTOs/                            # Data Transfer Objects
│   ├── Entities/                        # Domain Models
│   ├── Hubs/                            # SignalR NotificationHub
│   ├── Middlewares/                     # Global Exception Handler, Permissions
│   ├── Migrations/                      # EF Core Migrations
│   ├── Repositories/                    # Repositories (Interface & Implementation)
│   ├── Service/                         # Business Services & State Machine
│   │   ├── Implements/Shipping/         # Strategy & Template Method cho phí ship
│   │   └── Implements/StateMachine/     # State Pattern cho vòng đời đơn hàng
│   ├── appsettings.json                 # Cấu hình Database, JWT, SMTP, ORS
│   └── Program.cs                       # Cấu hình DI & Pipeline
│
├── frontend/                            # React 18 + TypeScript + Vite
│   ├── src/
│   │   ├── components/                  # UI Components, Layouts, Modals
│   │   │   ├── account/                 # Tabs thông tin tài khoản, địa chỉ
│   │   │   ├── modals/                  # Modal xem lịch sử giao, xử lý hết món
│   │   │   └── notifications/           # Chuông thông báo Realtime SignalR
│   │   ├── pages/
│   │   │   ├── Admin/                   # Trang quản trị Admin
│   │   │   ├── Auth/                    # Đăng nhập, đăng ký, OTP
│   │   │   ├── Restaurant/              # Quản lý nhà hàng, menu, đơn quán
│   │   │   └── Shipper/                 # Dashboard tài xế, nhận đơn, hồ sơ
│   │   ├── routes/                      # AppRoutes, ProtectedRoute
│   │   ├── services/                    # Axios API clients
│   │   └── types/                       # TypeScript interfaces/types
│   ├── package.json
│   └── vite.config.ts
│
├── FoodDelivery.Tests/                  # Unit Tests (xUnit, Moq, SQLite)
│   └── OrderDispatchTests.cs            # Kiểm thử điều phối đơn & Race condition
└── README.md
```

---

## 💻 Yêu cầu môi trường

* **.NET SDK**: Phiên bản `8.0` trở lên ([Tải tại đây](https://dotnet.microsoft.com/download/dotnet/8.0))
* **Node.js**: Phiên bản `18.x` hoặc `20.x` ([Tải tại đây](https://nodejs.org/))
* **Cơ sở dữ liệu**: PostgreSQL `14+`
* *(Tùy chọn)* Redis Server (nếu muốn dùng Distributed Caching)

---

## 🚀 Hướng dẫn cài đặt & Khởi chạy

### 1. Clone mã nguồn
```bash
git clone https://github.com/your-username/FoodDelivery.git
cd FoodDelivery
```

---

### 2. Cấu hình & Chạy Backend

#### Bước 2.1: Cấu hình `appsettings.json`
Mở file `backend/appsettings.json` và cập nhật thông tin chuỗi kết nối PostgreSQL và các dịch vụ liên quan:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=FoodDeliveryDb;Username=postgres;Password=your_password"
  },
  "TokenSecretKey": "YOUR_STRONG_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG",
  "Smtp": {
    "Email": "your-email@gmail.com",
    "Password": "your-app-password",
    "Host": "smtp.gmail.com",
    "Port": 587
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "Ors": {
    "ApiKey": "YOUR_OPENROUTESERVICE_API_KEY",
    "BaseUrl": "https://api.heigit.org/v2/directions/driving-car"
  },
  "Shipping": {
    "MaxDistanceKm": 40,
    "MaxShippingFee": 150000,
    "RoundUnit": 500,
    "Standard": { "BaseFee": 10000, "PerKm": 4000 },
    "Express":  { "BaseFee": 20000, "PerKm": 6000 }
  }
}
```

#### Bước 2.2: Chạy Database Migration
Mở terminal tại thư mục gốc của dự án:
```bash
cd backend
dotnet ef database update
```
*(Nếu máy chưa cài `dotnet-ef`, cài đặt bằng lệnh: `dotnet tool install --global dotnet-ef`)*

#### Bước 2.3: Chạy Backend API
```bash
dotnet run
```
Backend sẽ khởi chạy tại:
* HTTP: `http://localhost:5147`
* HTTPS: `https://localhost:7147` (hoặc port được định nghĩa trong `launchSettings.json`)
* Swagger UI: `https://localhost:7147/swagger`

---

### 3. Cấu hình & Chạy Frontend

#### Bước 3.1: Tạo file cấu hình môi trường
Mở thư mục `frontend`, tạo file `.env` (dựa trên mẫu `.env.example`):
```env
VITE_API_URL=https://localhost:7147/api
VITE_APP_NAME=Food Delivery
```

#### Bước 3.2: Cài đặt thư viện & Chạy ứng dụng
```bash
cd ../frontend
npm install
npm run dev
```
Ứng dụng Frontend sẽ khởi chạy tại: `http://localhost:5173`

---

## 👑 Tài khoản Quản trị viên khởi tạo (Initial Admin)

Hệ thống có sẵn cơ chế `InitialAdminSeeder` tự động kích hoạt tài khoản Admin đầu tiên khi database chưa có Admin. 

Để thiết lập thông tin Admin, đặt các biến môi trường trước khi khởi chạy Backend:

#### Trên Windows (PowerShell):
```powershell
$env:InitialAdmin__Email = "admin@fooddelivery.local"
$env:InitialAdmin__Password = "AdminPassword@123456"
$env:InitialAdmin__FullName = "System Administrator"
$env:InitialAdmin__Phone = "0901234567"
dotnet run
```

#### Trên Linux / macOS (Bash):
```bash
export InitialAdmin__Email="admin@fooddelivery.local"
export InitialAdmin__Password="AdminPassword@123456"
export InitialAdmin__FullName="System Administrator"
export InitialAdmin__Phone="0901234567"
dotnet run
```

> **Lưu ý:** Nếu tài khoản với email trên chưa tồn tại, hệ thống sẽ tự động tạo mới với vai trò `Admin`. Nếu email đã tồn tại, hệ thống chỉ cấp thêm quyền `Admin` mà không thay đổi mật khẩu hiện tại.

---

## 🧪 Kiểm thử tự động (Unit Tests)

Dự án có module kiểm thử độc lập `FoodDelivery.Tests` tập trung kiểm tra tính toàn vẹn của State Machine và cơ chế đồng thời khi điều phối tài xế:

```bash
dotnet test FoodDelivery.Tests/FoodDelivery.Tests.csproj
```

**Các kịch bản kiểm thử tiêu biểu:**
* `PickupRequiresAnAssignedShipper`: Kiểm tra điều kiện shipper phải được gán đơn trước khi lấy hàng.
* `ConcurrentAcceptsAssignOrderToOnlyOneShipper`: Giả lập 2 shipper cùng nhấn nhận 1 đơn cùng lúc, kiểm tra chỉ 1 shipper duy nhất nhận thành công (chống race condition).
* `ShipperWithAnActiveDeliveryCannotAcceptAnotherOrder`: Đảm bảo tài xế đang bận không thể nhận thêm đơn mới.
* `DeliveryFailedTransitionsShippingToCancelledForShipper`: Kiểm tra chuyển trạng thái hợp lệ khi shipper báo giao hàng thất bại.

---

## 📖 Tài liệu API & SignalR Hub

* **Swagger OpenAPI Documentation:**
  Sau khi chạy backend, truy cập: `https://localhost:7147/swagger` để xem toàn bộ danh sách endpoint, request/response payload và test trực tiếp các API.
* **SignalR Realtime Hub:**
  - Endpoint: `/notificationHub`
  - Kết nối yêu cầu gửi token qua query string: `/notificationHub?access_token={JWT_TOKEN}`
  - Client lắng nghe sự kiện: `"ReceiveNotification"`

---

## 🤝 Đóng góp & Phát triển (Contributing)

1. Fork dự án
2. Tạo nhánh tính năng mới (`git checkout -b feature/AmazingFeature`)
3. Commit thay đổi (`git commit -m 'feat: Add some AmazingFeature'`)
4. Push lên nhánh (`git push origin feature/AmazingFeature`)
5. Mở một Pull Request trên GitHub
