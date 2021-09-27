using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace YogaClassPlanner.Models.YogaModels
{

    public class studentContext:DbContext
    {
        public DbSet<student> student_data { get; set; }
    }
    public class student
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string passwordHash { get; set; }
        public string healthHistory { get; set; }
        public string level { get; set; }
        public string expected_results { get; set; }

    }
}