using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Users;
using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SmartCampus_HAU_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<IActionResult> RegisterNewUserAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null || string.IsNullOrEmpty(registerDTO.UserName) ||
                string.IsNullOrEmpty(registerDTO.Email) || string.IsNullOrEmpty(registerDTO.Password))
            {
                return new BadRequestObjectResult("Thông tin đăng ký không hợp lệ");
            }

            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
            {
                return new BadRequestObjectResult("Email đã tồn tại");
            }

            var user = new User
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                FullName = registerDTO.FullName ?? registerDTO.UserName,
                EmailConfirmed = false // Chưa xác nhận email
            };

            var createNewUser = await _userManager.CreateAsync(user, registerDTO.Password);

            if (createNewUser.Succeeded)
            {
                try
                {
                    await SendEmailConfirmationAsync(user);
                    return new OkObjectResult(new
                    {
                        Message = "Vui lòng kiểm tra email để hoàn tất đăng ký tài khoản.",
                        RequiresEmailConfirmation = true
                    });
                }
                catch (Exception ex)
                {
                    return new OkObjectResult(new
                    {
                        Message = "Chưa tạo tài khoản thành công.",
                        RequiresEmailConfirmation = true,
                        EmailError = ex.Message
                    });
                }
            }

            return new BadRequestObjectResult(createNewUser.Errors.Select(e => e.Description));
        }

        private async Task SendEmailConfirmationAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = $"https://localhost:7072/api/User/user/confirm-email?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";

            var emailSubject = "Xác nhận tài khoản SmartCampus HAU";
            var emailBody = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #2c3e50;'>Chào mừng đến với SmartCampus HAU!</h2>
                    <p>Xin chào <strong>{user.FullName}</strong>,</p>
                    <p>Cảm ơn bạn đã đăng ký tài khoản tại SmartCampus HAU. Để hoàn tất quá trình đăng ký, vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết bên dưới:</p>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{confirmationLink}' 
                           style='background-color: #3498db; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                            Xác nhận Email
                        </a>
                    </div>
                    
                    <p><strong>Thông tin tài khoản:</strong></p>
                    <ul>
                        <li><strong>Tên đăng nhập:</strong> {user.UserName}</li>
                        <li><strong>Email:</strong> {user.Email}</li>
                        <li><strong>Họ tên:</strong> {user.FullName}</li>
                    </ul>
                    
                    <p style='color: #e74c3c; font-size: 14px;'>
                        <strong>Lưu ý:</strong> Liên kết này sẽ hết hạn sau 24 giờ. Nếu bạn không thực hiện việc đăng ký này, vui lòng bỏ qua email này.
                    </p>
                    
                    <hr style='margin: 30px 0; border: none; border-top: 1px solid #ecf0f1;'>
                    <p style='font-size: 12px; color: #7f8c8d;'>
                        Email này được gửi tự động từ hệ thống SmartCampus HAU. Vui lòng không trả lời email này.
                    </p>
                </div>";

            await _emailService.SendEmailAsync(user.Email!, emailSubject, emailBody);
        }

        public async Task<IActionResult> ConfirmEmailAsync(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return new BadRequestObjectResult("Thông tin xác nhận không hợp lệ");
            }

            email = Uri.UnescapeDataString(email);
            token = Uri.UnescapeDataString(token);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new BadRequestObjectResult("Không tìm thấy người dùng");
            }

            if (user.EmailConfirmed)
            {
                return new OkObjectResult("Email đã tồn tại");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new OkObjectResult("Email đã được xác nhận. Hiện tại bạn có thể đăng nhập");
            }

            return new BadRequestObjectResult("Xác nhận email không thành công");
        }

        public async Task<IActionResult> ResendEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new BadRequestObjectResult("Không tìm thấy người dùng");
            }

            if (user.EmailConfirmed)
            {
                return new BadRequestObjectResult("Email đã được xác nhận");
            }

            try
            {
                await SendEmailConfirmationAsync(user);
                return new OkObjectResult("Email xác nhận đã được gửi");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Lỗi khi gửi tới email: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<ServiceResult> SendForgotPasswordEmailAsync([FromBody] ForgotPasswordRequest request)
        {
            var email = request.Email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed)
            {
                return ServiceResult.Failure("Email không tồn tại hoặc chưa xác nhận");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://localhost:7072/api/User/user/verify-reset-token?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

            var subject = "Đặt lại mật khẩu";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #e74c3c;'>Đặt lại mật khẩu SmartCampus HAU</h2>
                    <p>Xin chào <strong>{user.FullName}</strong>,</p>
                    <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn tại SmartCampus HAU. Để tiếp tục, vui lòng nhấp vào nút bên dưới:</p>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='background-color: #e74c3c; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                            Đặt lại mật khẩu
                        </a>
                    </div>
                    
                    <p style='color: #e74c3c; font-size: 14px;'>
                        <strong>Lưu ý:</strong> Liên kết này sẽ hết hạn sau 1 giờ vì lý do bảo mật. Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này và tài khoản của bạn sẽ vẫn an toàn.
                    </p>
                    
                    <div style='background-color: #fff3cd; border: 1px solid #ffeaa7; border-radius: 4px; padding: 15px; margin: 20px 0;'>
                        <p style='margin: 0; color: #856404; font-size: 14px;'>
                            <strong>⚠️ Bảo mật:</strong> Nếu bạn không yêu cầu đặt lại mật khẩu, có thể ai đó đang cố gắng truy cập tài khoản của bạn. Vui lòng liên hệ bộ phận hỗ trợ ngay lập tức.
                        </p>
                    </div>
                    
                    <hr style='margin: 30px 0; border: none; border-top: 1px solid #ecf0f1;'>
                    <p style='font-size: 12px; color: #7f8c8d;'>
                        Email này được gửi tự động từ hệ thống SmartCampus HAU. Vui lòng không trả lời email này.
                        <br>Nếu cần hỗ trợ, vui lòng liên hệ: support@smartcampus-hau.edu.vn
                    </p>
                </div>";


            await _emailService.SendEmailAsync(email, subject, body);

            return ServiceResult.Success("Email đặt lại mật khẩu đã được gửi. Vui lòng kiểm tra email.");
        }

        public async Task<ServiceResult<string>> VerifyResetTokenAsync(string email, string token)
        {
            try
            {
                email = Uri.UnescapeDataString(email);
                token = Uri.UnescapeDataString(token);

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                {
                    return ServiceResult<string>.Failure("Thông tin không hợp lệ");
                }

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return ServiceResult<string>.Failure("Không tìm thấy người dùng");
                }

                var isvalidToken = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
                if (isvalidToken)
                {
                    var frontendUrl = _configuration["Frontend:ResetPasswordUrl"];
                    var redirectUrl = $"{frontendUrl}?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}&valid=true";

                    return ServiceResult<string>.Success(redirectUrl, "Token hợp lệ");
                }

                return ServiceResult<string>.Failure("Token không hợp lệ hoặc đã hết hạn");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure("Lỗi hệ thống", new List<string> { ex.Message });
            }
        }

        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(resetPasswordDTO.Email) ||
                    string.IsNullOrEmpty(resetPasswordDTO.Token) ||
                    string.IsNullOrEmpty(resetPasswordDTO.NewPassword))
                {
                    return ServiceResult.Failure("Thông tin không đầy đủ");
                }

                var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
                if (user == null)
                {
                    return ServiceResult.Failure("Không tìm thấy người dùng");
                }

                var result = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.NewPassword);

                if (result.Succeeded)
                {
                    return ServiceResult.Success("Đặt lại mật khẩu thành công");
                }

                return ServiceResult.Failure("Đặt lại mật khẩu thất bại", result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("Lỗi hệ thống", new List<string> { ex.Message });
            }
        }

        public async Task<ServiceResult> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || changePasswordDTO == null ||
                    string.IsNullOrEmpty(changePasswordDTO.CurrentPassword) ||
                    string.IsNullOrEmpty(changePasswordDTO.NewPassword))
                {
                    return ServiceResult.Failure("Thông tin đổi mật khẩu không hợp lệ");
                }

                if (changePasswordDTO.NewPassword != changePasswordDTO.ConfirmPassword)
                {
                    return ServiceResult.Failure("Mật khẩu mới và xác nhận mật khẩu không khớp");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure("Không tìm thấy người dùng");
                }

                if (!user.EmailConfirmed)
                {
                    return ServiceResult.Failure("Email chưa được xác nhận. Vui lòng xác nhận email trước khi đổi mật khẩu");
                }

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);

                if (result.Succeeded)
                {
                    try
                    {
                        await SendPasswordChangedNotificationAsync(user);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send password change notification: {ex.Message}");
                    }

                    return ServiceResult.Success("Đổi mật khẩu thành công");
                }

                var errors = result.Errors.Select(e => e.Description).ToList();

                if (errors.Any(e => e.Contains("current password") || e.Contains("incorrect")))
                {
                    return ServiceResult.Failure("Mật khẩu hiện tại không chính xác");
                }

                return ServiceResult.Failure("Đổi mật khẩu thất bại", errors);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("Lỗi hệ thống", new List<string> { ex.Message });
            }
        }

        private async Task SendPasswordChangedNotificationAsync(User user)
        {
            var subject = "Mật khẩu đã được thay đổi - SmartCampus HAU";
            var emailBody = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <h2 style='color: #27ae60;'>Mật khẩu đã được thay đổi thành công</h2>
            <p>Xin chào <strong>{user.FullName}</strong>,</p>
            <p>Mật khẩu cho tài khoản SmartCampus HAU của bạn đã được thay đổi thành công vào lúc:</p>
            
            <div style='background-color: #e8f5e8; border-left: 4px solid #27ae60; padding: 15px; margin: 20px 0;'>
                <p style='margin: 0;'><strong>Thời gian:</strong> {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} (UTC+7)</p>
                <p style='margin: 5px 0 0 0;'><strong>Tài khoản:</strong> {user.UserName} ({user.Email})</p>
            </div>
            
            <p>Nếu <strong>bạn không thực hiện</strong> thay đổi này, vui lòng:</p>
            <ul>
                <li>Đăng nhập ngay để kiểm tra tài khoản</li>
                <li>Liên hệ bộ phận hỗ trợ: support@smartcampus-hau.edu.vn</li>
                <li>Thay đổi mật khẩu mới ngay lập tức</li>
            </ul>
            
            <div style='background-color: #fff3cd; border: 1px solid #ffeaa7; border-radius: 4px; padding: 15px; margin: 20px 0;'>
                <p style='margin: 0; color: #856404; font-size: 14px;'>
                    <strong>💡 Lời khuyên bảo mật:</strong> Sử dụng mật khẩu mạnh có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.
                </p>
            </div>
            
            <hr style='margin: 30px 0; border: none; border-top: 1px solid #ecf0f1;'>
            <p style='font-size: 12px; color: #7f8c8d;'>
                Email này được gửi tự động từ hệ thống SmartCampus HAU. Vui lòng không trả lời email này.
            </p>
        </div>";

            await _emailService.SendEmailAsync(user.Email!, subject, emailBody);
        }

        public async Task<IActionResult> UpdateUserInfoAsync(string userId, UpdateUserInfoDTO updateUserInfoDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || updateUserInfoDTO == null)
                {
                    return new BadRequestObjectResult("Thông tin cập nhật không hợp lệ");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new BadRequestObjectResult("Không tìm thấy người dùng");
                }

                if (!user.EmailConfirmed)
                {
                    return new BadRequestObjectResult("Email chưa được xác nhận. Vui lòng xác nhận email trước khi cập nhật thông tin");
                }

                string oldFullName = user.FullName;
                string oldPhoneNumber = user.PhoneNumber;

                bool hasChanges = false;

                if (!string.IsNullOrWhiteSpace(updateUserInfoDTO.FullName) &&
                    updateUserInfoDTO.FullName.Trim() != user.FullName)
                {
                    user.FullName = updateUserInfoDTO.FullName.Trim();
                    hasChanges = true;
                }

                if (updateUserInfoDTO.PhoneNumber != user.PhoneNumber)
                {
                    // Kiểm tra số điện thoại đã được sử dụng bởi user khác chưa
                    if (!string.IsNullOrWhiteSpace(updateUserInfoDTO.PhoneNumber))
                    {
                        var existingUserWithPhone = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.PhoneNumber == updateUserInfoDTO.PhoneNumber && u.Id != userId);

                        if (existingUserWithPhone != null)
                        {
                            return new BadRequestObjectResult("Số điện thoại đã được sử dụng bởi tài khoản khác");
                        }
                    }

                    user.PhoneNumber = string.IsNullOrWhiteSpace(updateUserInfoDTO.PhoneNumber)
                        ? null
                        : updateUserInfoDTO.PhoneNumber.Trim();
                    hasChanges = true;
                }

                if (!hasChanges)
                {
                    return new OkObjectResult(new
                    {
                        Message = "Không có thay đổi nào được thực hiện",
                        User = new
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            FullName = user.FullName,
                            PhoneNumber = user.PhoneNumber
                        }
                    });
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send info change notification: {ex.Message}");
                    }

                    return new OkObjectResult(new
                    {
                        Message = "Cập nhật thông tin thành công",
                        User = new
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            FullName = user.FullName,
                            PhoneNumber = user.PhoneNumber
                        },
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                var errors = result.Errors.Select(e => e.Description).ToList();
                return new BadRequestObjectResult(new
                {
                    Message = "Cập nhật thông tin thất bại",
                    Errors = errors
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Lỗi hệ thống: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetUserInfoAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return new BadRequestObjectResult("User ID không hợp lệ");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new BadRequestObjectResult("Không tìm thấy người dùng");
                }

                return new OkObjectResult(new
                {
                    Message = "Lấy thông tin người dùng thành công",
                    User = new
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                    }
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Lỗi hệ thống: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> DeleteUsersAsync(string username)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == username);   
                if (user == null)
                {
                    return new OkObjectResult("Người dùng không tồn tại");
                }

                var result = await _userManager.DeleteAsync(user!);
                if (!result.Succeeded)
                {
                    return new BadRequestObjectResult(new
                    {
                        Message = "Xóa người dùng thất bại",
                        Errors = result.Errors.Select(e => e.Description)
                    });
                }
                return new OkObjectResult("Đã xóa người dùng thành công");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Lỗi hệ thống: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}

