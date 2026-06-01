using Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace _4Axon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Realiza login e retorna token JWT
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.LoginAsync(request);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        /// <summary>
        /// Obtém todos os usuários (paginado)
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Página e tamanho devem ser maiores que 0");

            var (users, totalCount) = await _userService.GetAllUsersAsync(pageNumber, pageSize);
            return Ok(new
            {
                success = true,
                items = users,
                totalCount = totalCount,
                pageNumber = pageNumber,
                pageSize = pageSize
            });
        }

        /// <summary>
        /// Obtém um usuário pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("Usuário não encontrado");

            return Ok(new { success = true, user });
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id <= 0)
                return BadRequest("ID inválido");

            var result = await _userService.UpdateUserAsync(id, request);
            if (!result)
                return BadRequest("Erro ao atualizar usuário");

            return Ok(new { success = true, message = "Usuário atualizado com sucesso" });
        }

        /// <summary>
        /// Deleta um usuário
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return BadRequest("Erro ao deletar usuário");

            return Ok(new { success = true, message = "Usuário deletado com sucesso" });
        }
    }
}
