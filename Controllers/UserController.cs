﻿using CrudApi.Contexts;
using CrudApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TokenAuthenticationHelper;

namespace CrudApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
	private readonly UserContext _context;
	private readonly ITokenBuilder _tokenBuilder;

	public UserController(UserContext context, ITokenBuilder tokenBuilder)
	{
		_context = context;
		_tokenBuilder = tokenBuilder;
	}

	[HttpGet]
	public DateTime Ping()
	{
		return DateTime.UtcNow;
	}

	[Authorize]
	[HttpGet]
	public IActionResult GetUser()
	{
		return Ok(User.Identity!.Name);
	}

	[HttpPost]
	public IActionResult SignUp([FromBody] User user)
	{
		try {
			_context.Users!.Add(user);
			_context.SaveChanges();
			return Ok(BuildTokenHelper(user));
		} catch (DbUpdateException e) {
			return Problem(e.Message);
		}
	}

	[HttpPost]
	public IActionResult GetSalt([FromBody] string email)
	{
		try {
			var queryUser = _context.Users!.Single(b => b.Email == email);
			return Ok(queryUser.Salt);
		} catch (InvalidOperationException) {
			return Problem("User not found");
		}
	}

	public class Login
	{
		public string? Email { get; set; }
		public string? HashedPassword { get; set; }
	}

	[HttpPost]
	public IActionResult LogIn([FromBody] Login login)
	{
		try {
			var queryUser = _context.Users!.Single(b => b.Email == login.Email);
			return queryUser.Password == login.HashedPassword ?
				Ok(BuildTokenHelper(queryUser))
				: BadRequest("Wrong Password");
		} catch (InvalidOperationException e) {
			return Problem(e.Message);
		}
	}

	string BuildTokenHelper(User user) =>
		_tokenBuilder.BuildToken(new Claim[]
			{
				new Claim(ClaimTypes.Sid, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email!),
				new Claim(ClaimTypes.Name, user.Username!)
			},
			DateTime.UtcNow.AddDays(14)
		);
}

