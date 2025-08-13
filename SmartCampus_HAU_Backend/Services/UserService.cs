using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Users;
using SmartCampus_HAU_Backend.Models.Entities;
using SmartCampus_HAU_Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SmartCampus_HAU_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
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

            var confirmationLink = $"https://localhost:7072/api/User/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

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

            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);
        }

        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return new BadRequestObjectResult("Invalid confirmation request");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new BadRequestObjectResult("User not found");
            }

            if (user.EmailConfirmed)
            {
                return new OkObjectResult("Email already confirmed");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new OkObjectResult("Email confirmed successfully. You can now login to your account.");
            }

            return new BadRequestObjectResult("Email confirmation failed");
        }

        public async Task<IActionResult> ResendEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new BadRequestObjectResult("User not found");
            }

            if (user.EmailConfirmed)
            {
                return new BadRequestObjectResult("Email already confirmed");
            }

            try
            {
                await SendEmailConfirmationAsync(user);
                return new OkObjectResult("Confirmation email sent successfully");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Failed to send email: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.UserName) ||
                string.IsNullOrEmpty(loginDTO.Password))
            {
                return new BadRequestObjectResult("Thông tin đăng nhập không hợp lệ");
            }

            // Tìm user theo username hoặc email
            var user = await _userManager.FindByNameAsync(loginDTO.UserName) ??
                       await _userManager.FindByEmailAsync(loginDTO.UserName);

            if (user == null)
            {
                return new UnauthorizedObjectResult("Tên đăng nhập hoặc mật khẩu không chính xác");
            }

            if (!user.EmailConfirmed)
            {
                return new UnauthorizedObjectResult(new
                {
                    Message = "Email chưa được xác nhận. Vui lòng kiểm tra email để xác nhận tài khoản.",
                    Email = user.Email
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    return new UnauthorizedObjectResult("Tài khoản đã bị khóa do nhập sai mật khẩu quá nhiều lần");
                }

                if (result.RequiresTwoFactor)
                {
                    return new UnauthorizedObjectResult("Yêu cầu xác thực hai yếu tố");
                }

                return new UnauthorizedObjectResult("Tên đăng nhập hoặc mật khẩu không chính xác");
            }

            return new OkObjectResult(new
            {
                Message = "Đăng nhập thành công"
            });
        }
    }
}
