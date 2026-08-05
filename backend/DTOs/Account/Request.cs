using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DTOs.Account
{
    public class AccountRequest
    {
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(254, ErrorMessage = "Email không được vượt quá 254 ký tự")]
        public string? Email {get;set;}
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MinLength(2, ErrorMessage = "Họ tên tối thiểu phải có 2 ký tự")]
        [MaxLength(100, ErrorMessage = "Họ tên tối đa chỉ được 100 ký tự")]
        public string? FullName {get; set;}
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
        public string? Phone {get; set;}
        public IFormFile? Avatar {get; set;}
    }

    public class UpdateProfileRequest
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MinLength(2, ErrorMessage = "Họ tên tối thiểu phải có 2 ký tự")]
        [MaxLength(100, ErrorMessage = "Họ tên tối đa chỉ được 100 ký tự")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
        public string Phone { get; set; } = string.Empty;
    }
}