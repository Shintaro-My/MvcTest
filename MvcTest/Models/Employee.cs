using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MvcTest.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }
    }
}