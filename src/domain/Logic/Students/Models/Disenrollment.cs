﻿using CSharpFunctionalExtensions;
using System;

namespace Logic.Students.Models
{
    public class Disenrollment : Entity
    {
        public virtual Student Student { get; protected set; }
        public virtual Course Course { get; protected set; }
        public virtual DateTime DateTime { get; protected set; }
        public virtual string Comment { get; protected set; }


        protected Disenrollment()
        {
        }

        public Disenrollment(Student student, Course course, string comment)
        {
            Student = student;
            Course = course;
            Comment = comment;
            DateTime = DateTime.UtcNow;
        }
    }
}
