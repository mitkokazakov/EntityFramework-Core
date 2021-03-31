namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var departmentsDTOs = JsonConvert.DeserializeObject<ImportDepartmentsCellsDTO[]>(jsonString);

            List<Department> departments = new List<Department>();

            foreach (var dep in departmentsDTOs)
            {
                if (!IsValid(dep))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department() 
                {
                    Name = dep.Name
                };

                bool isValidDep = true;

                foreach (var c in dep.Cells)
                {
                    if (!IsValid(c))
                    {
                        isValidDep = false;
                        break;
                    }

                    Cell cell = new Cell()
                    {
                        CellNumber = c.CellNumber,
                        HasWindow = c.HasWindow
                    };

                    department.Cells.Add(cell);
                }

                if (!isValidDep)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (department.Cells.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");

            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var prisonersDTO = JsonConvert.DeserializeObject<ImportPrisonersMailsDTO[]>(jsonString);

            List<Prisoner> prisoners = new List<Prisoner>();

            foreach (var p in prisonersDTO)
            {
                if (!IsValid(p))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime incarcDate;
                bool incarDateValid = DateTime.TryParseExact(p.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,out incarcDate);

                if (!incarDateValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime releaseDate;

                bool isValidreleaseDate = DateTime.TryParseExact(p.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                if (!isValidreleaseDate)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }


                Prisoner prisoner = new Prisoner() 
                {
                    FullName = p.FullName,
                    Nickname = p.Nickname,
                    Age = p.Age,
                    IncarcerationDate = incarcDate,
                    ReleaseDate = releaseDate,
                    Bail = p.Bail,
                    CellId = p.CellId

                };

                bool mailValid = true;

                foreach (var m in p.Mails)
                {
                    if (!IsValid(m))
                    {
                        mailValid = false;
                        break;
                    }

                    Mail mail = new Mail()
                    {
                        Description = m.Description,
                        Sender = m.Sender,
                        Address = m.Address
                    };
                    prisoner.Mails.Add(mail);
                }

                if (!mailValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                prisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportOfficerPrisonerDTO[]), new XmlRootAttribute("Officers"));

            ImportOfficerPrisonerDTO[] officers = (ImportOfficerPrisonerDTO[])xmlSerializer.Deserialize(new StringReader(xmlString));

            List<Officer> officersToAdd = new List<Officer>();

            foreach (var off in officers)
            {
                object position;
                bool isValidPosition = Enum.TryParse(typeof(Position), off.Position, out position);

                if (!isValidPosition)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                object weapon;
                bool isValidWeapon = Enum.TryParse(typeof(Weapon),off.Weapon,out weapon);

                if (!isValidWeapon)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (!IsValid(off))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer() 
                {
                    FullName = off.Name,
                    Salary = off.Money,
                    Position = (Position)position,
                    Weapon = (Weapon)weapon,
                    DepartmentId = off.DepartmentId
                };

                foreach (var priss in off.Prisoners)
                {
                    OfficerPrisoner officerPrisoner = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = priss.Id
                    };

                    officer.OfficerPrisoners.Add(officerPrisoner);
                }

                officersToAdd.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officersToAdd);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}