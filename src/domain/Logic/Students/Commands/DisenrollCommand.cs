﻿using CSharpFunctionalExtensions;
using Logic.Students.Models;
using Logic.Students.Repositories;
using Logic.Utils.Shared;
using System;
using System.Threading.Tasks;

namespace Logic.Studentss.Commands
{
    public sealed class DisenrollCommand : ICommand
    {
        public long Id { get; }
        public int EnrollmentNumber { get; }
        public string Comment { get; }

        public DisenrollCommand(long id, int enrollmentNumber, string comment)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            Comment = comment;
        }


        internal sealed class DisenrollCommandHandler : ICommandHandler<DisenrollCommand>
        {
            private readonly IGenericRepository<Student> _studentRepository;

            public DisenrollCommandHandler(IGenericRepository<Student> studentRepository)
            {
                _studentRepository = studentRepository;
            }

            public Type CommandType => typeof(DisenrollCommand);

            public async Task<Result> Handle(DisenrollCommand command)
            {
                var studentResult = await _studentRepository.Get(command.Id);
                if (studentResult.HasNoValue)
                    return Result.Failure($"No student found for Id {command.Id}");

                var student = studentResult.Value;
                if (string.IsNullOrWhiteSpace(command.Comment))
                    return Result.Failure("Disenrollment comment is required");

                var enrollmentResult = student.GetEnrollment(command.EnrollmentNumber);
                if (enrollmentResult.HasNoValue)
                    return Result.Failure($"No enrollment found with number '{command.EnrollmentNumber}'");

                student.RemoveEnrollment(enrollmentResult.Value, command.Comment);

                await _studentRepository.Save();

                return Result.Success();
            }
        }
    }
}