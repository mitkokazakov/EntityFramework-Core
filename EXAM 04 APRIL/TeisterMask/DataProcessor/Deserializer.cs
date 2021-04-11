namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportProjectDTO[]), new XmlRootAttribute("Projects"));

            ImportProjectDTO[] projects = (ImportProjectDTO[])serializer.Deserialize(new StringReader(xmlString));

            List<Project> projToAdd = new List<Project>();

            foreach (var pr in projects)
            {
                DateTime openDate;
                bool isValidOpenDate = DateTime.TryParseExact(pr.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out openDate);

                DateTime dueDate;
                bool isValidDueDate = DateTime.TryParseExact(pr.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDate);


                if (!IsValid(pr) || !isValidOpenDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Project project = new Project()
                {
                    Name = pr.Name,
                    OpenDate = openDate,
                    DueDate = dueDate,

                };

                foreach (var ts in pr.Tasks)
                {

                    DateTime openDateTask;
                    bool isValidOpenDateTask = DateTime.TryParseExact(ts.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out openDateTask);

                    DateTime dueDateTask;
                    bool isValidDueDateTask = DateTime.TryParseExact(ts.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDateTask);

                    object execType;
                    bool isValidExecType = Enum.TryParse(typeof(ExecutionType), ts.ExecutionType, out execType);

                    object labelType;
                    bool isValidLabelType = Enum.TryParse(typeof(LabelType), ts.LabelType, out labelType);

                    if (!IsValid(ts) || !isValidDueDateTask || !isValidOpenDateTask || !isValidExecType || !isValidLabelType)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if ((openDateTask < openDate) || (dueDateTask > dueDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task()
                    {
                        Name = ts.Name,
                        OpenDate = openDateTask,
                        DueDate = dueDateTask,
                        ExecutionType = (ExecutionType)execType,
                        LabelType = (LabelType)labelType
                    };

                    project.Tasks.Add(task);
                }

                projToAdd.Add(project);
                sb.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(projToAdd);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var employees = JsonConvert.DeserializeObject<ImportEmployeeDTO[]>(jsonString);

            foreach (var emp in employees)
            {
                if (!IsValid(emp))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee()
                {
                    Username = emp.Username,
                    Email = emp.Email,
                    Phone = emp.Phone
                };

                var tasks = emp.Tasks.Distinct();

                foreach (var ts in tasks)
                {
                    Task task = context.Tasks.FirstOrDefault(t => t.Id == ts);

                    if (task == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask employeeTask = new EmployeeTask()
                    {
                        Employee = employee,
                        Task = task
                    };

                    employee.EmployeesTasks.Add(employeeTask);
                }

                context.Employees.Add(employee);
                context.SaveChanges();
                sb.AppendLine(String.Format(SuccessfullyImportedEmployee,employee.Username,employee.EmployeesTasks.Count));
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}