using FluentValidation;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Application.Services
{
    public class PdfService
    {
        private readonly IMedicalRecordRepository medicalRecordRepository;

        public PdfService(IMedicalRecordRepository medicalRecordRepository)
        {
            this.medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<byte[]> GeneratePDF(Guid recordId)
        {
            StringReader htmlContent = new StringReader(await CreateHtmlContent(recordId));
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            byte[] bytes;
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(htmlContent);
                pdfDoc.Close();

                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }
            return bytes;
        }

        public async Task<string> CreateHtmlContent(Guid recordId)
        {
            var record = await medicalRecordRepository.GetMedicalRecordForPDFAsync(recordId);
            var lastWeight = record.Animal?.Weight == null ? record.Animal.Weight + " kg" : "nem ismert";
            var currentDate = DateTime.Now;
            var html =
                @"<div style=""display: flex; justify-content: center"" > " +
                    @"<p style=""margin: 0px; padding: 0px"">Veterinary állatkórház - kórlap</p>" + @$"<p style=""margin: 0px; padding: 0px"">Generálva:" +
                $" {currentDate.ToLocalTime().ToString("yyyy. MM. dd. HH:mm")}</p>" +
                "</div>" +
                "<div>" +
                    @"<h1 style=""text-align:center"" > Kórlap</h1>" +
                "</div>" +
                @"<div style=""display: flex; justify-content: center"" > " +
                    @"<table style=""background-color: #f5f5f5; width: 100%"">" +
                          "<tr>" +
                                "<th><u>Tulajdonos adatai:</u></th>" +
                                "<th><u>Állat adatai:</u></th>" +
                          "</tr>" +
                          "<tr>" +
                                @"<td style=""height: 10px !important"" colspan =""2"" ></td>" +
                          "</tr>" +
                          "<tr>" +
                                $"<td>{record.Owner?.Name ?? ""}</td>" +
                                $"<td>Állat kód: {record.Animal?.Id.ToString() ?? ""}</td>" +
                          "</tr>" +
                          "<tr>" +
                                "<td>E-mail:</td>" +
                                "<td>Név: Madzag</td>" +
                          "</tr>" +
                          "<tr>" +
                                $"<td>{record.OwnerEmail}</td>" +
                                $"<td>Fajta: {record.Animal?.Species?.Name}</td>" +
                          "</tr>" +
                          "<tr>" +
                                "<td>Telefonszám:</td>" +
                                $"<td>Fajta: {record.Animal?.Species?.Name}</td>" +
                          "</tr>" +
                          "<tr>" +
                                $"<td>{record.Owner?.PhoneNumber}</td>" +
                                $"<td>Születési dátum: {record.Animal?.DateOfBirth.ToLocalTime().ToString("yyyy. MM. dd.")}</td>" +
                          "</tr>" +
                         "<tr>" +
                                $"<td>Tkód: {record.Owner.Id}</td>" +
                                $"<td>Nem: {record.Animal?.Sex}</td>" +
                          "</tr>" +
                          "<tr>" +
                                "<td></td>" +
                                $"<td>Legutóbbi súly: {lastWeight}</td>" +
                          "</tr>" +
                    "</table>" +
                "</div>" +
                "<div>" +
                    "<b><u>Kórlap:</u></b>" +
                "</div>" +

                "<div>" +
                    $"<b>Dátum: {record.Date.ToLocalTime()}</b>" +
                "</div>" +

                @"<div style=""margin-bottom: 30px"">" +
                    $"<b>Klinika, orvos: {record.Doctor.Name}</b>" +
                "</div>" +

                @"<div style=""margin-bottom: 30px"" > </div>" +

                @"<div style=""margin-bottom: 30px"" > " +
                $"{record.HtmlContent}</div>" +

                @"<div style=""margin-top: 30px; margin-bottom: 30px"" > </div>" +
                "<b><u>Alkalmazott gyógyszerek:</u></b>" +
                    @"<table style=""width: 100%; margin-bottom: 30px"" > " +
                        "<tr>" +
                              "<th>Azonosító</th>" +
                              "<th>Megnevezés</th>" +
                              "<th>Mennyiség</th>" +
                        "</tr>";

            foreach (var medication in record.MedicationRecords)
            {
                html += "<tr>" +
                          $"<td>{medication.MedicationId}</td>" +
                          $"<td>{medication.Medication.Name}</td>" +
                          $"<td>{medication.Amount} {medication.Medication.UnitName}</td>" +
                        "</tr>";
            }


            html += "</table>" +
              "<b><u>Alkalmazott szolgáltatások:</u></b>" +
                  @"<table style=""width: 100%"" > " +
                      "<tr>" +
                            "<th>Azonosító</th>" +
                            "<th>Megnevezés</th>" +
                            "<th>Mennyiség</th>" +
                      "</tr>";
            foreach (var therapia in record.TherapiaRecords)
            {
                html += "<tr>" +
                          $"<td>{therapia.TherapiaId}</td>" +
                          $"<td>{therapia.Therapia.Name}</td>" +
                          $"<td>{therapia.Amount} db</td>" +
                        "</tr>";
            }
                       html += "</table>" +
                "</div>";

            return html;
        }
    }
}
