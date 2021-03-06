﻿using Logic.Students.Commands;
using Logic.Students.Queries;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Controllers
{
    public class StudentController : Controller
    {
        private readonly Dispatcher _messages;

        public StudentController(Dispatcher messages)
        {
            _messages = messages;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string enrolled, int? number)
        {
            var list = await _messages.Dispatch(new GetListQuery(enrolled, number));
            return View(list);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] NewStudentDto dto)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _messages.Dispatch(
                new RegisterCommand(dto.Name, dto.Email, dto.Course1, dto.Course1Grade.ToString(), dto.Course2, dto.Course2Grade?.ToString()));

            if (result.IsFailure)
                return Error(result.Error);

            return RedirectToAction(nameof(Index));
        }


        private IActionResult Error(string errorMessage)
        {
            return BadRequest(errorMessage);
        }
    }
}
