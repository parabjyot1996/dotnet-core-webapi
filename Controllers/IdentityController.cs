using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCoreAPI.Dtos;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;

namespace NetCoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController: ControllerBase
    {
        private readonly IIdentityRepository _identityRepo;
        private readonly IMapper _mapper;

        public IdentityController(IIdentityRepository identityRepo, IMapper mapper)
        {
            _identityRepo = identityRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Returns token when user is registered successfully</returns>
        /// <response code="200">Returns token</response>
        /// <response code="400">User data is invalid</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationResult
                {
                    Errors = ModelState.Values.SelectMany(msgs => msgs.Errors.Select(msg => msg.ErrorMessage)),
                    IsSuccess = false
                });
            }

            var user = _mapper.Map<User>(userDto);
            var authResponse = await _identityRepo.RegisterAsync(user.EmailId, user.Password);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse);
            }

            return Ok(authResponse);
        }

        /// <summary>
        /// Login authenticated user.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Returns token when user authenticated successfully</returns>
        /// <response code="200">Returns token</response>
        /// <response code="400">User data is invalid</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationResult
                {
                    Errors = ModelState.Values.SelectMany(msgs => msgs.Errors.Select(msg => msg.ErrorMessage)),
                    IsSuccess = false
                });
            }

            var user = _mapper.Map<User>(userDto);
            var authResponse = await _identityRepo.LoginAsync(user.EmailId, user.Password);
    
            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse);
            }

            return Ok(authResponse);
        }

        /// <summary>
        /// Refresh Token.
        /// </summary>
        /// <param name="refreshTokenDto"></param>
        /// <returns>Returns refresh token</returns>
        /// <response code="200">Returns refresh token</response>
        /// <response code="400">Token is invalid</response>
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var authResponse = await _identityRepo.
                                        RefreshTokenAsync(refreshTokenDto.Token, refreshTokenDto.RefreshToken);

            
            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse);
            }

            return Ok(authResponse);
        }
    }
}