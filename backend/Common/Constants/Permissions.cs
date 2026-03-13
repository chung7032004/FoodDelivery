namespace FoodDelivery.Constants
{
    public static class Permissions
    {
        // 1. Quản lý thực đơn (Dành cho Chủ quán/Nhân viên quản lý)
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
            public const string ToggleAvailability = "Permissions.Products.ToggleAvailability"; // Bật/Tắt món nhanh khi hết hàng
        }

        public static class Categories
        {
            public const string View = "Permissions.Categories.View";
            public const string Manage = "Permissions.Categories.Manage";
        }

        // 2. Quản lý Đơn hàng (Cực kỳ quan trọng)
        public static class Orders
        {
            public const string View = "Permissions.Orders.View";
            public const string Create = "Permissions.Orders.Create"; // Dành cho khách hoặc nhân viên tạo hộ
            public const string Confirm = "Permissions.Orders.Confirm"; // Xác nhận đơn mới đổ về
            public const string Cancel = "Permissions.Orders.Cancel"; // Hủy đơn (cần quyền cao hơn)
            public const string UpdateStatus = "Permissions.Orders.UpdateStatus"; // Chuyển trạng thái: Đang làm, Đã xong
            public const string AssignShipper = "Permissions.Orders.AssignShipper"; // Điều phối tài xế (Admin/Dispatcher)
        }

        // 3. Quản lý Quán (Multi-tenant)
        public static class Restaurants
        {
            public const string ViewProfile = "Permissions.Restaurants.ViewProfile";
            public const string UpdateProfile = "Permissions.Restaurants.UpdateProfile";
            public const string ManageSettings = "Permissions.Restaurants.ManageSettings"; // Cài đặt giờ đóng/mở cửa, phí dịch vụ
            public const string ViewRevenue = "Permissions.Restaurants.ViewRevenue"; // Xem báo cáo doanh thu quán
        }

        // 4. Quản lý Nhân sự nội bộ quán
        public static class Staffs
        {
            public const string View = "Permissions.Staffs.View";
            public const string Create = "Permissions.Staffs.Create";
            public const string Edit = "Permissions.Staffs.Edit";
            public const string Delete = "Permissions.Staffs.Delete";
        }

        // 5. Quản lý Tài xế (Dành cho hệ thống tổng hoặc Admin điều phối)
        public static class Shippers
        {
            public const string View = "Permissions.Shippers.View";
            public const string Verify = "Permissions.Shippers.Verify"; // Duyệt hồ sơ tài xế mới
            public const string Block = "Permissions.Shippers.Block"; // Khóa tài xế vi phạm
            public const string AcceptOrder = "Permissions.Shippers.AcceptOrder"; // Quyền riêng cho app Shipper
        }

        // 6. Quản lý Khách hàng & Phản hồi
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string ManageReviews = "Permissions.Users.ManageReviews"; // Phản hồi hoặc ẩn các đánh giá xấu
        }
    }
}