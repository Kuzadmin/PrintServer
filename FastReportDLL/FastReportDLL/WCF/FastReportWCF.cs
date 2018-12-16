using FastReport;
using FastReport.Export;
using FastReportService;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
using System.Xml.Linq;

namespace FastReportDLL.WCF
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    class FastReportWCF : IFastService
    {
        private ConfigClass conf;

        public FastReportWCF(ConfigClass _conf)
        {
            conf = _conf;
        }
        

        private Stream GetFile(string xml)
        {
            MessageList mes_list = new MessageList();
            MessageLog logs = new MessageLog(MessageLog.INFO, "GetFile", "Создание потока");
            mes_list.Add_Message(logs);
            XDocument doc = XDocument.Parse(xml);

            FRDataForm form = new FRDataForm();
            ConverterClass convert_class = new ConverterClass();
            List<DataTable> list = convert_class.Converter(doc, out form);

            using (Report report = new Report())
            {
                //conf.GetValue("FolderFR")  "\\Report\\"
                
                String name_file = conf.GetValue("PathService") + "\\"+ conf.GetValue("FolderFR") + "\\" + form.product + "\\" + form.form + ".frx";
                MemoryStream mem = new MemoryStream();

                logs = new MessageLog(MessageLog.INFO, "GetFile", "Отчет " + name_file);
                mes_list.Add_Message(logs);

                ExportBase export = Format(form);
                
                report.Load(name_file);
                EnvironmentSettings s = new EnvironmentSettings();
                s.ReportSettings.ShowProgress = false;
                foreach (DataTable st in list)
                {
                    report.RegisterData(st, st.TableName);
                }
                //report.Show();
                //report.Design();
                report.Prepare();
                //FileStream fl = new FileStream("555.pdf", FileMode.Create);
                //report.Export(export, fl);
                //fl.Close();
                //report.SavePrepared(fl);
                report.Export(export, mem);

                // }
                // nt.Flush();
                mem.Position = 0;
                //byte[] file = mem.ToArray();                
                logs = new MessageLog(MessageLog.INFO, "GetFile", "Отчет сформирован и отправлен"); 
                mes_list.Add_Message(logs);

                Logger.getInstance().Write(mes_list);

                return mem;
            }
        }

        private ExportBase Format(FRDataForm form)
        {
            switch (form.format)
            {
                case "PDF":  return new FastReport.Export.Pdf.PDFExport();
                case "ODT":  return new FastReport.Export.Odf.ODTExport();
                case "ODS":  return new FastReport.Export.Odf.ODSExport();
                case "XLSX": return new FastReport.Export.OoXML.Excel2007Export();
                case "DOCX": return new FastReport.Export.OoXML.Word2007Export();
                case "BMP":
                case "JPG":
                case "JPEG":
                case "PNG":
                case "TIFF":
                    {
                        FastReport.Export.Image.ImageExport image_export = new FastReport.Export.Image.ImageExport();
                        switch (form.format)
                        {
                            case "BMP":  image_export.ImageFormat = FastReport.Export.Image.ImageExportFormat.Bmp; break;
                            case "JPG":  
                            case "JPEG": image_export.ImageFormat = FastReport.Export.Image.ImageExportFormat.Jpeg; break;
                            case "PNG":  image_export.ImageFormat = FastReport.Export.Image.ImageExportFormat.Png; break;
                            case "TIFF": image_export.ImageFormat = FastReport.Export.Image.ImageExportFormat.Tiff; break;
                            default: image_export.ImageFormat = FastReport.Export.Image.ImageExportFormat.Jpeg; break;
                        }
                        return (ExportBase)image_export;
                    }
                default: return new FastReport.Export.Pdf.PDFExport();
            }
        }

        public Stream GetPrintFile(XmlElement xml)
        {
            return GetFile(xml.OuterXml);
        }
    }
}
