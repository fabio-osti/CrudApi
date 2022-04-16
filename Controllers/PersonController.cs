using CrudApi.Contexts;
using CrudApi.Models;
using CrudApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PersonController : Controller
{
	private readonly PersonContext _context;

	public PersonController(PersonContext context)
	{
		_context = context;
	}

	[HttpPost]
	public IActionResult Create(Person person)
	{
		try
		{
			var p = _context.Persons!.Add(person);
			_context.SaveChanges();
			return Ok((int)p.Entity.Id!);
		}
		catch (DbUpdateException e)
		{
			return Problem(e.Message);
		}
	}

	[HttpGet]
	public IActionResult Read(
		int rows, int page, PersonsOrdering orderBy, string? name, short? age, bool? sex, HairColor? hc
	)
	{
		try
		{
			IQueryable<Person> persons = _context.Persons!;
			if (name != null)
				persons = persons.Where(person => person.Name == name);
			if (age != null)
				persons = persons.Where(person => person.Age == age);
			if (sex != null)
				persons = persons.Where(person => person.Sex == sex);
			if (hc != null)
				persons = persons.Where(person => person.HairColor == hc);

			persons = orderBy switch
			{
				PersonsOrdering.NameA => persons.OrderBy(Person => Person.Name),
				PersonsOrdering.NameD => persons.OrderByDescending(Person => Person.Name),
				PersonsOrdering.AgeA => persons.OrderBy(Person => Person.Age),
				PersonsOrdering.AgeD => persons.OrderByDescending(Person => Person.Age),
				PersonsOrdering.SexA => persons.OrderBy(Person => Person.Sex),
				PersonsOrdering.SexD => persons.OrderByDescending(Person => Person.Sex),
				PersonsOrdering.HairColorA => persons.OrderBy(Person => Person.HairColor),
				PersonsOrdering.HairColorD => persons.OrderByDescending(Person => Person.HairColor),
				_ => persons,
			};

			return Ok(new ApiResponse<Person>(
				persons.Skip(rows * (page - 1)).Take(rows).ToArray(), persons.Count()));
		}
		catch (Exception e)
		{
			return Problem(e.Message);
		}
	}

	[HttpPost]
	public IActionResult Update(Person person)
	{
		try
		{
			_context.Persons!.Update(person);
			_context.SaveChanges();
			return Ok();
		}
		catch (DbUpdateException e)
		{
			return Problem(e.Message);
		}
	}

	[HttpPost]
	public IActionResult Delete(int id)
	{
		try
		{
			var remove = _context.Persons!.Find(id);
			if (remove == null)
				return Problem("Id not found");
			_context.Remove(remove);
			_context.SaveChanges();
			return Ok();
		}
		catch (DbUpdateException e)
		{
			return Problem(e.Message);
		}
	}
}

