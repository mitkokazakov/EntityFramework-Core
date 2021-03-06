﻿using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;


namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            string result = GetEmployeesInPeriod(context);

            Console.WriteLine(result);
        }

        //Task 3
        public static string GetEmployeesFullInformation(SoftUniContext context) 
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Select(e => new
            {
                FirstName = e.FirstName,
                MiddleName = e.MiddleName,
                LastName = e.LastName,
                JobTitle = e.JobTitle,
                Salary = e.Salary
            }).ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // Task 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context) 
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Where(e => e.Salary > 50000).Select(e => new
            {
                e.FirstName,
                e.Salary
            }).OrderBy(e => e.FirstName).ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // Task 5

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Where(e => e.Department.Name == "Research and Development")
                                                        .Select(e => new
                                                        {
                                                            e.FirstName,
                                                            e.LastName,
                                                            DepartmentName = e.Department.Name,
                                                            e.Salary
                                                        }).OrderBy(e => e.Salary)
                                                            .ThenByDescending(e => e.FirstName)
                                                            .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // Task 6

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
            employee.Address = address;
            context.SaveChanges();

            var addresses = context.Employees.OrderByDescending(e => e.AddressId)
                                             .Select(e => new
                                             {
                                                 AddressText = e.Address.AddressText
                                             })
                                             .Take(10)
                                             .ToList();

            foreach (var a in addresses)
            {
                sb.AppendLine(a.AddressText);
            }
                                                                                

            return sb.ToString().TrimEnd();
        }

        // Task 7

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Where(e => e.EmployeesProjects
                                                        .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                                                        .Select(e => new
                                                        {
                                                            e.FirstName,
                                                            e.LastName,
                                                            ManagerFirstName = e.Manager.FirstName,
                                                            ManagerLastname = e.Manager.LastName,
                                                            Projects = e.EmployeesProjects
                                                            .Select(p => new
                                                            {
                                                                ProjectName = p.Project.Name,
                                                                StartDate = p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                                                                EndDate = p.Project.EndDate != null ? p.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                                                            })
                                                        })
                                                        .Take(10)
                                                        .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastname}");

                foreach (var p in e.Projects)
                {

                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        // Task 8

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses.Select(a => new
            {
                a.AddressText,
                TownName = a.Town.Name,
                EmployeesCount = a.Employees.Count
            }).OrderByDescending(a => a.EmployeesCount)
                                      .ThenBy(a => a.TownName)
                                      .ThenBy(a => a.AddressText)
                                      .Take(10)
                                      .ToList();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }


        //Task 9

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees.Where(e => e.EmployeeId == 147).Select(e => new { 
                                                                                    FirstName = e.FirstName,
                                                                                    LastName = e.LastName,
                                                                                    JobTitle = e.JobTitle,
                                                                                    Projects = e.EmployeesProjects
                                                                                    .Select(ep => new { 
                                                                                        ProjectName = ep.Project.Name    
                                                                                    }).OrderBy(p => p.ProjectName).ToList()
                                                                                                
                                                                                    }).ToList();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"{p.ProjectName}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        // Task 10

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments.Where(d => d.Employees.Count > 5)
                                                 .OrderBy(d => d.Employees.Count)
                                                 .ThenBy(d => d.Name)
                                                 .Select(d => new
                                                 {
                                                     DepartmentName = d.Name,
                                                     ManagerFirstName = d.Manager.FirstName,
                                                     ManagerLastName = d.Manager.LastName,
                                                     Employees = d.Employees.Select(e => new
                                                     {
                                                         EmployeeFirstName = e.FirstName,
                                                         EmployeeLastName = e.LastName,
                                                         JobTitle = e.JobTitle
                                                     }).OrderBy(e => e.EmployeeFirstName).ThenBy(e => e.EmployeeLastName).ToList()
                                                 }).ToList();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.DepartmentName} - {d.ManagerFirstName} {d.ManagerLastName}");

                foreach (var e in d.Employees)
                {
                    sb.AppendLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        // Task 11

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects.OrderByDescending(p => p.StartDate).ThenBy(p => p.Name).Take(10)
                                                                     .Select(p => new
                                                                     {
                                                                         ProjectName = p.Name,
                                                                         Desription = p.Description,
                                                                         StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt",                                                                                        CultureInfo.InvariantCulture)
                                                                     }).ToList();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.ProjectName}");
                sb.AppendLine($"{p.Desription}");
                sb.AppendLine($"{p.StartDate}");
            }

            return sb.ToString().TrimEnd();
        }


        // Task 12

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var increasedEmployees = context.Employees.Where(e => e.Department.Name == "Engineering" ||
                                                                  e.Department.Name == "Tool Design" ||
                                                                  e.Department.Name == "Marketing" ||
                                                                  e.Department.Name == "Information Services"
                                                                    );

            foreach (var e in increasedEmployees)
            {
                e.Salary += e.Salary * 0.12m;
            }

            context.SaveChanges();

            var infoEmployees = increasedEmployees.Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Salary
            }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName);

            foreach (var e in infoEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }


        // Task 13

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Where(e => e.FirstName.ToLower().StartsWith("sa")).Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                e.Salary
            }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }
            return sb.ToString().TrimEnd();
        }


        // Task 14

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projectToDel = context.Projects.Find(2);

            var emplProj = context.EmployeesProjects.Where(ep => ep.ProjectId == 2).ToList();

            foreach (var ep in emplProj)
            {
                context.EmployeesProjects.Remove(ep);
            }

            context.Projects.Remove(projectToDel);
            context.SaveChanges();

            var rest10Projects = context.Projects.Select(p => new
            {
                p.Name
            }).Take(10).ToList();

            foreach (var p in rest10Projects)
            {
                sb.AppendLine(p.Name);
            }
            return sb.ToString().TrimEnd();
        }


        // Task 15

        public static string RemoveTown(SoftUniContext context)
        {
            Town town = context.Towns.FirstOrDefault(t => t.Name == "Seattle");

            var addressesToDel = context.Addresses.Where(a => a.TownId == town.TownId);

            int count = addressesToDel.Count();

            var employees = context.Employees.Where(e => addressesToDel.Any(a => a.AddressId == e.AddressId));

            foreach (var e in employees)
            {
                e.AddressId = null;
            }

            foreach (var a in addressesToDel)
            {
                context.Addresses.Remove(a);
            }

            context.Towns.Remove(town);

            context.SaveChanges();
            return $"{count} addresses in {town.Name} were deleted";
        }
    }
}
