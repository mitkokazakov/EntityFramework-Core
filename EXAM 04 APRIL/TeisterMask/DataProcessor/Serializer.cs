namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects.ToArray()
                .Where(p => p.Tasks.Count >= 1)
                .Select(p => new ExportProjectsDTO()
                {
                    TasksCount = p.Tasks.Count,
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks.Select(t => new ExportSingleTaskDTO()
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    }).OrderBy(t => t.Name).ToArray()
                }).OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToArray();



            XmlSerializer serializer = new XmlSerializer(typeof(ExportProjectsDTO[]), new XmlRootAttribute("Projects"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), projects, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {

            var employees = context.Employees
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .ToArray()
                .Select(e => new ExportEmployeeDTO()
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks.Select(et => new ExportTaskDTO()
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    }).OrderByDescending(t => t.DueDate)
                    .ThenBy(t => t.TaskName)
                    .ToArray()
                }).OrderByDescending(e => e.Tasks.Count())
                .ThenBy(e => e.Username)
                .Take(10);

            
            string res = JsonConvert.SerializeObject(employees);

            return res;
        }
    }
}