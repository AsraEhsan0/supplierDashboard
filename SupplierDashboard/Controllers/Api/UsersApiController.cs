using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierDashboard.Data;
using SupplierDashboard.Enems;
using SupplierDashboard.Models.Entities;

namespace SupplierDashboard.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<UsersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Agency)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        MiddleName = u.MiddleName,
                        AccountActive = u.AccountActive,
                        AllowBookUnderCancellationPolicy = u.AllowBookUnderCancellationPolicy,
                        AllowCancellationAfterVoucher = u.AllowCancellationAfterVoucher,
                        ConsultantReceiveBookingEmail = u.ConsultantReceiveBookingEmail,
                        RoleType = u.RoleType,
                        Timezone = u.Timezone,
                        CompanyPhone = u.CompanyPhone,
                        AccountingId = u.AccountingId,
                        AgencyId = u.AgencyId,
                        AgencyName = u.Agency != null ? u.Agency.AgencyName : null,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Agency)
                    .Where(u => u.Id == id)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        MiddleName = u.MiddleName,
                        AccountActive = u.AccountActive,
                        AllowBookUnderCancellationPolicy = u.AllowBookUnderCancellationPolicy,
                        AllowCancellationAfterVoucher = u.AllowCancellationAfterVoucher,
                        ConsultantReceiveBookingEmail = u.ConsultantReceiveBookingEmail,
                        RoleType = u.RoleType,
                        Timezone = u.Timezone,
                        CompanyPhone = u.CompanyPhone,
                        AccountingId = u.AccountingId,
                        AgencyId = u.AgencyId,
                        AgencyName = u.Agency != null ? u.Agency.AgencyName : null,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(CreateUserDto dto)
        {
            try
            {
                // Validate agency exists if provided
                if (!string.IsNullOrEmpty(dto.AgencyId))
                {
                    var agencyExists = await _context.Agencies.AnyAsync(a => a.Id == dto.AgencyId);
                    if (!agencyExists)
                    {
                        return BadRequest("Agency not found");
                    }
                }

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(), // ✅ random Guid
                    UserName = dto.UserName,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    MiddleName = dto.MiddleName,
                    AccountActive = dto.AccountActive,
                    AllowBookUnderCancellationPolicy = dto.AllowBookUnderCancellationPolicy,
                    AllowCancellationAfterVoucher = dto.AllowCancellationAfterVoucher,
                    ConsultantReceiveBookingEmail = dto.ConsultantReceiveBookingEmail,
                    RoleType = dto.RoleType,
                    Timezone = dto.Timezone,
                    CompanyPhone = dto.CompanyPhone,
                    AccountingId = dto.AccountingId,
                    AgencyId = dto.AgencyId,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                if (user.Email != dto.Email)
                {
                    var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
                    if (existingEmail != null && existingEmail.Id != id)
                    {
                        return BadRequest("Email already exists");
                    }
                }

                // Validate agency exists if provided
                if (!string.IsNullOrEmpty(dto.AgencyId))
                {
                    var agencyExists = await _context.Agencies.AnyAsync(a => a.Id == dto.AgencyId);
                    if (!agencyExists)
                    {
                        return BadRequest("Agency not found");
                    }
                }

                user.Email = dto.Email;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.MiddleName = dto.MiddleName;
                user.AccountActive = dto.AccountActive;
                user.AllowBookUnderCancellationPolicy = dto.AllowBookUnderCancellationPolicy;
                user.AllowCancellationAfterVoucher = dto.AllowCancellationAfterVoucher;
                user.ConsultantReceiveBookingEmail = dto.ConsultantReceiveBookingEmail;
                user.RoleType = dto.RoleType;
                user.Timezone = dto.Timezone;
                user.CompanyPhone = dto.CompanyPhone;
                user.AccountingId = dto.AccountingId;
                user.AgencyId = dto.AgencyId;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("role-types")]
        public ActionResult<IEnumerable<object>> GetRoleTypes()
        {
            var roleTypes = Enum.GetValues<UserRoleType>()
                .Select(r => new { Value = (int)r, Name = r.ToString() })
                .ToList();

            return Ok(roleTypes);
        }

    }

    public class CreateUserDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? MiddleName { get; set; }

        public bool AccountActive { get; set; } = true;
        public bool AllowBookUnderCancellationPolicy { get; set; } = false;
        public bool AllowCancellationAfterVoucher { get; set; } = false;
        public bool ConsultantReceiveBookingEmail { get; set; } = false;

        public UserRoleType RoleType { get; set; }

        public string? Timezone { get; set; }

        public string? CompanyPhone { get; set; }

        public string? AccountingId { get; set; }

        // Foreign Key to Agency
        public string? AgencyId { get; set; }
    }
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();

        // User Rights
        public bool AccountActive { get; set; }
        public bool AllowBookUnderCancellationPolicy { get; set; }
        public bool AllowCancellationAfterVoucher { get; set; }
        public bool ConsultantReceiveBookingEmail { get; set; }

        // User Role Type
        public UserRoleType RoleType { get; set; }
        public string RoleTypeName => RoleType.ToString();

        // Company Details
        public string? Timezone { get; set; }
        public string? CompanyPhone { get; set; }
        public string? AccountingId { get; set; }

        // Agency Details
        public string? AgencyId { get; set; }
        public string? AgencyName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
