namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new ExportPrisonersDTO() 
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(o => new ExportOfficersDTO() 
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name
                    }).OrderBy(o => o.OfficerName).ToArray(),
                    TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(o => o.Officer.Salary),2)
                }).OrderBy(p => p.Name)
                .ThenBy(p => p.Id).ToArray();

            string jsonRes = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return jsonRes;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] names = prisonersNames.Split(",",StringSplitOptions.RemoveEmptyEntries).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportPrisonerXMLDto[]), new XmlRootAttribute("Prisoners"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var prisoners = context.Prisoners
                .Where(p => names.Contains(p.FullName))
                .Select(p => new ExportPrisonerXMLDto()
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture),
                    EncryptedMessages = p.Mails
                                         .Select(m => new ExportMessagesDTO()
                                         {
                                             Description = String.Join("", m.Description.Reverse())
                                         }).ToArray()
                }).OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray() ;

            StringBuilder stringBuilder = new StringBuilder();

            StringWriter stringWriter = new StringWriter(stringBuilder);

            serializer.Serialize(stringWriter, prisoners,namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}