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
            public const string ViewAll = "Permissions.Orders.ViewAll"; // Xem tất cả đơn (Admin/Dispatcher)
            public const string ViewHistory = "Permissions.Orders.ViewHistory"; // Xem lịch sử đơn đã hoàn thành
            public const string Create = "Permissions.Orders.Create"; // Dành cho khách hoặc nhân viên tạo hộ
            public const string Confirm = "Permissions.Orders.Confirm"; // Xác nhận đơn mới đổ về
            public const string Cancel = "Permissions.Orders.Cancel"; // Hủy đơn (cần quyền cao hơn)
            public const string UpdateStatus = "Permissions.Orders.UpdateStatus"; // Chuyển trạng thái: Đang làm, Đã xong
            public const string AssignShipper = "Permissions.Orders.AssignShipper"; // Điều phối tài xế (Admin/Dispatcher)
            public const string RespondToProposal = "Permissions.Orders.RespondToProposal"; // Phản hồi đề xuất thay đổi đơn hàng từ quán
            public const string ConfirmOrder = "Permissions.Orders.ConfirmOrder"; // Xác nhận đơn hàng (Admin)
            public const string OutOfStock = "Permissions.Orders.OutOfStock"; // Báo hết hàng (Admin)
            public const string StartPreparing = "Permissions.Orders.StartPreparing"; // Bắt đầu làm món (Admin)
            public const string MarkAsReady = "Permissions.Orders.MarkAsReady"; // Đánh dấu món đã xong (Admin)
            public const string MarkAsDelivered = "Permissions.Orders.MarkAsDelivered"; // Đánh dấu đã giao (Shipper)
            public const string MarkPaymentComplete = "Permissions.Orders.MarkPaymentComplete"; // Đánh dấu đã thanh toán (Admin)
        }

        // 3. Quản lý Quán (Multi-tenant)
        public static class Restaurants
        {
            public const string View = "Permissions.Restaurants.View";
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
        // 7. Quản lý tài khoản
        public static class Accounts
        {
            // user tự quản lý tài khoản của mình
            public const string ViewOwn = "Permissions.Accounts.ViewOwn";
            public const string UpdateOwn = "Permissions.Accounts.UpdateOwn";
            public const string ChangePassword = "Permissions.Accounts.ChangePassword";

            // admin quản lý toàn bộ tài khoản
            public const string ViewAll = "Permissions.Accounts.ViewAll";
            public const string View = "Permissions.Accounts.View";
            public const string Edit = "Permissions.Accounts.Edit";
            public const string Delete = "Permissions.Accounts.Delete";

            // các quyền nâng cao
            public const string Lock = "Permissions.Accounts.Lock";      // khóa tài khoản
            public const string Unlock = "Permissions.Accounts.Unlock";  // mở khóa
            public const string AssignRole = "Permissions.Accounts.AssignRole"; // gán role
        }
        public static class Address
        {
            public const string View = "Permissions.Address.View";
            public const string Create = "Permissions.Address.Create";
            public const string Edit = "Permissions.Address.Edit";
            public const string Delete = "Permissions.Address.Delete";
            public const string SetDefault = "Permissions.Address.SetDefault";
        }
        public static class Cart
        {
            public const string View = "Permissions.Cart.View";
            public const string AddItem = "Permissions.Cart.AddItem";
            public const string UpdateItem = "Permissions.Cart.UpdateItem";
            public const string RemoveItem = "Permissions.Cart.RemoveItem";
            public const string ClearCart = "Permissions.Cart.ClearCart";
        }
        public static class Review
        {
            public const string Create = "Permissions.Review.Create";
            public const string Edit = "Permissions.Review.Edit";
            public const string View = "Permissions.Review.View";
            public const string Manage = "Permissions.Review.Manage"; // Ẩn/Xóa review không phù hợp
            public const string Delete = "Permissions.Review.Delete";
        }
        public static class Notifications
        {
            public const string View = "Permissions.Notifications.View";
            public const string MarkAsRead = "Permissions.Notifications.MarkAsRead";
            public const string Delete = "Permissions.Notifications.Delete";
            public const string Send = "Permissions.Notifications.Send"; // Admin gửi thông báo hệ thống
        }

        public static class Dashboard
        {
            public const string ViewOwn = "Permissions.Dashboard.ViewOwn";       // Owner xem dashboard quán mình
            public const string ViewSystem = "Permissions.Dashboard.ViewSystem"; // Admin xem toàn hệ thống
        }
    }
}