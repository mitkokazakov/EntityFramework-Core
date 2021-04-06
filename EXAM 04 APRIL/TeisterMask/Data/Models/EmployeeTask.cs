﻿using System.ComponentModel.DataAnnotations;

namespace TeisterMask.Data.Models
{
    public class EmployeeTask
    {
        [Required]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required]
        public int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}

//•	EmployeeId - integer, Primary Key, foreign key(required)
//•	Employee -  Employee
//•	TaskId - integer, Primary Key, foreign key(required)
//•	Task - Task
