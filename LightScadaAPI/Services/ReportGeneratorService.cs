using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using LightScadaAPI.Contexts;
using ScadaCommon;
using System.Text;

namespace LightScadaAPI.Controllers
{
    public interface IReportGenerator
    {
        public byte[] GenerateReport(int OrganizationID, ReportContent content);
    }

    public class PdfReportGenerator : IReportGenerator
    {
        private ReportDatasetReader m_reader;

        public PdfReportGenerator(IConfiguration configuration)
        {
            m_reader = new ReportDatasetReader(configuration.GetConnectionString("UserContextConnection"));
        }

        public byte[] GenerateReport(int OrganizationId, ReportContent content)
        {
            string name = m_reader.GetOrganizationName(OrganizationId);
            if (String.IsNullOrEmpty(name))
                return null;

            List<List<RegisterFrame>> data = m_reader.GetDataList(OrganizationId, content);

            string reportContent = @"<!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                        <meta charset=""utf-8"">
                        <style>
                        h1, h3 {text-align: center;}
                        p {text-align: center;}
                        div {text-align: center;}
                        table { border-collapse: collapse; width: 100% }
                        th, td {padding: 8px;text-align: left;border-bottom: 1px solid #ddd; margin: 4px 0 4px 0;}
                        table.print-friendly tr td, table.print-friendly tr th {page-break-inside: avoid;}
                        </style>
                        </head>
                        <body>" 
                        + $@"<h1>{name} Report - {DateTime.Now}</h1>";

            foreach (List<RegisterFrame> registerData in data)
            {
                if (!registerData.Any())
                    continue;

                reportContent += @$"<h3>Values from client {registerData[0].ClientId}, {(RegisterType)registerData[0].RegisterType} {registerData[0].RegisterAddress}</h3>
                <table class=""print-friendly"">
                <tr>
                <th>ID</th>
                <th>Timestamp</th>
                <th>Register Type</th>
                <th>Register Address</th>
                <th>Value</th>
                </tr>";
                int index = 1;
                foreach (RegisterFrame frame in registerData)
                {
                    reportContent += @$"<tr>
                    <td>{index}</td>
                    <td>{frame.Timestamp.ToLocalTime()}</td>
                    <td>{(RegisterType)frame.RegisterType}</td>
                    <td>{frame.RegisterAddress}</td>
                    <td><b>{frame.Value}</b></td>
                    </tr>";
                    index++;
                }
                reportContent += "</table>";
            }

            reportContent += @"</body>
                    </html>
                    ";
            return Encoding.UTF8.GetBytes(reportContent);
        }
    }
}