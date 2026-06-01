using Core;
using Core.DTOs;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;

		public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
		{
			_unitOfWork = unitOfWork;
			_configuration = configuration;
		}

		public async Task<AuthResponse> RegisterAsync(UserRequest request)
		{
			try
			{
				if (request == null || string.IsNullOrWhiteSpace(request.Email))
				{
					return new AuthResponse
					{
						Success = false,
						Message = "Dados inválidos"
					};
				}

				var existingUser = await _unitOfWork.User.GetByEmail(request.Email);
				if (existingUser != null)
				{
					return new AuthResponse
					{
						Success = false,
						Message = "E-mail já registrado"
					};
				}

				var user = new User
				{
					Name = request.Name,
					Email = request.Email,
					Password = HashPassword(request.Password)
				};

				await _unitOfWork.User.Add(user);
				var result = _unitOfWork.Save();

				if (result > 0)
				{
					return new AuthResponse
					{
						Success = true,
						Message = "Usuário registrado com sucesso",
						User = MapToUserResponse(user)
					};
				}

				return new AuthResponse
				{
					Success = false,
					Message = "Erro ao registrar usuário"
				};
			}
			catch (Exception ex)
			{
				return new AuthResponse
				{
					Success = false,
					Message = $"Erro: {ex.Message}"
				};
			}
		}

		public async Task<AuthResponse> LoginAsync(LoginRequest request)
		{
			try
			{
				if (request == null || string.IsNullOrWhiteSpace(request.Email))
				{
					return new AuthResponse
					{
						Success = false,
						Message = "E-mail e senha são obrigatórios"
					};
				}

				var user = await _unitOfWork.User.GetByEmail(request.Email);
				if (user == null || !VerifyPassword(request.Password, user.Password))
				{
					return new AuthResponse
					{
						Success = false,
						Message = "E-mail ou senha inválidos"
					};
				}

				var token = GenerateJwtToken(user);

				return new AuthResponse
				{
					Success = true,
					Message = "Login realizado com sucesso",
					Token = token,
					User = MapToUserResponse(user)
				};
			}
			catch (Exception ex)
			{
				return new AuthResponse
				{
					Success = false,
					Message = $"Erro: {ex.Message}"
				};
			}
		}

		public async Task<UserResponse?> GetUserByIdAsync(int userId)
		{
			if (userId <= 0)
				return null;

			var user = await _unitOfWork.User.GetById(userId);
			return user != null ? MapToUserResponse(user) : null;
		}

		public async Task<bool> UpdateUserAsync(int userId, UserRequest request)
		{
			try
			{
				if (userId <= 0 || request == null)
					return false;

				var user = await _unitOfWork.User.GetById(userId);
				if (user == null)
					return false;

				var userByEmail = await _unitOfWork.User.GetByEmail(request.Email);
				if (userByEmail != null && userByEmail.Id != userId)
					return false;

				user.Name = request.Name;
				user.Email = request.Email;
				user.Password = HashPassword(request.Password);

				_unitOfWork.User.Update(user);
				var result = _unitOfWork.Save();

				return result > 0;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> DeleteUserAsync(int userId)
		{
			try
			{
				if (userId <= 0)
					return false;

				var user = await _unitOfWork.User.GetById(userId);
				if (user == null)
					return false;

				_unitOfWork.User.Delete(user);
				var result = _unitOfWork.Save();

				return result > 0;
			}
			catch
			{
				return false;
			}
		}

		public async Task<(IEnumerable<UserResponse> Items, int TotalCount)> GetAllUsersAsync(int pageNumber, int pageSize)
		{
			var (users, totalCount) = await _unitOfWork.User.GetAll(pageNumber, pageSize);
			var userResponses = users.Select(MapToUserResponse);
			return (userResponses, totalCount);
		}

		private string GenerateJwtToken(User user)
		{
			var jwtKey = _configuration["Jwt:Key"];
			var jwtIssuer = _configuration["Jwt:Issuer"];
			var jwtAudience = _configuration["Jwt:Audience"];
			var jwtExpirMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
								new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
								new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email),
								new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Name)
						};

			var token = new JwtSecurityToken(
					issuer: jwtIssuer,
					audience: jwtAudience,
					claims: claims,
					expires: DateTime.UtcNow.AddMinutes(jwtExpirMinutes),
					signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private string HashPassword(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				return Convert.ToBase64String(hashedBytes);
			}
		}

		private bool VerifyPassword(string password, string hash)
		{
			var hashOfInput = HashPassword(password);
			return hashOfInput.Equals(hash);
		}

		private UserResponse MapToUserResponse(User user)
		{
			return new UserResponse
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				CreatedDate = user.CreatedDate,
				UpdatedDate = user.UpdatedDate
			};
		}
	}
	public interface IUserService
	{
		Task<AuthResponse> RegisterAsync(UserRequest request);
		Task<AuthResponse> LoginAsync(LoginRequest request);
		Task<UserResponse?> GetUserByIdAsync(int userId);
		Task<bool> UpdateUserAsync(int userId, UserRequest request);
		Task<bool> DeleteUserAsync(int userId);
		Task<(IEnumerable<UserResponse> Items, int TotalCount)> GetAllUsersAsync(int pageNumber, int pageSize);
	}
}
