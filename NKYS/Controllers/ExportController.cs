using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NKYS.Account.Model;
using NKYS.Models;
using NKYS.Models.ViewModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace NKYS.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SalariesController salariesController ;
        private readonly Context db;

        public ExportController(Context context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
            salariesController = new SalariesController(db, _userManager);
            
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> ExportSalariesWorkingHours(long CycleId)
        {
            var list = await salariesController.SalariesSearchData(null, null, CycleId, true);


            if (list.Count() > 0)
            {
                var formatedList = new List<SalaryExportModel>();
                foreach (var item in list)
                {
                    SalaryExportModel newItme = (SalaryExportModel)item;
                    formatedList.Add(newItme);
                }
                var exportName = "SalariesWorkingHours";
                var memory = ExportExcel(formatedList.ToList<dynamic>(), exportName);
                return File(memory, "application/vnd.ms-excel", exportName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
            }
            else
            {
                return NotFound();
            }
        }


        public MemoryStream ExportExcel(List<dynamic> List, string ExportName)
        {
            if (List != null && List.Count() > 0)
            {
                /*Step0: Create file */
                var newFile =  "/" + ExportName + ".xls";
                if (System.IO.File.Exists(newFile))
                {
                    System.IO.File.Delete(newFile);
                }

                using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
                {

                    /* Step1: Get export model */
                    var ExportConfiguration = db.ExportConfiguration.Where(p => p.ExportName == ExportName).FirstOrDefault();
                    List<ExportModel> ExportConfigurationModel = null;
                    if (ExportConfiguration != null && ExportConfiguration.ExportModel != null && ExportConfiguration.ExportModel != "")
                    {

                        ExportConfigurationModel = JsonConvert.DeserializeObject<List<ExportModel>>(ExportConfiguration.ExportModel);
                    }
                    /* Step2: Calcul the targeted Column */

                    // Get columns title from first object in the list
                    var columns = List[0].GetType().GetProperties();
                    var targetColumns = new List<string>();
                    var targetCoulmnsWithOrder = new List<ExportModel>();

                    foreach (var item in columns)
                    {
                        if (ExportConfigurationModel != null)
                        {
                            var temp = ExportConfigurationModel.Where(p => p.Name == item.Name).FirstOrDefault();
                            if (temp != null)
                            {
                                targetCoulmnsWithOrder.Add(temp);
                            }


                        }
                        else
                        {
                            targetColumns.Add(item.Name);
                        }
                    }
                    targetCoulmnsWithOrder = targetCoulmnsWithOrder.OrderBy(x => x.Order).ToList();

                    /*Step3: Create Excel flow */
                    IWorkbook workbook = new XSSFWorkbook();
                    var sheet = workbook.CreateSheet(ExportName);
                    var header = sheet.CreateRow(0);

                    /* Bold the title */
                    XSSFFont headerFont = (XSSFFont)workbook.CreateFont();
                    headerFont.Boldweight = (short)FontBoldWeight.Bold; // bold
                    XSSFCellStyle firstTitleStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    firstTitleStyle.SetFont(headerFont);

                    /*Step4: Add headers*/
                    int columnsCounter = 0;
                    foreach (var item in targetCoulmnsWithOrder)
                    {

                        if (ExportConfigurationModel != null)
                        {
                            var temp = ExportConfigurationModel.Where(p => p.Name == item.Name).Select(p => p.DisplayName).FirstOrDefault();
                            var cell = header.CreateCell(columnsCounter);
                            cell.CellStyle = firstTitleStyle;
                            cell.SetCellValue(temp != null ? temp : item.Name);

                        }
                        else
                        {
                            header.CreateCell(columnsCounter).SetCellValue(item.Name);
                        }
                        columnsCounter++;
                    }
                    /*Step5: Add body */
                    var rowIndex = 1;
                    foreach (var item in List)
                    {
                        var datarow = sheet.CreateRow(rowIndex);

                        columnsCounter = 0;
                        foreach (var column in targetCoulmnsWithOrder)
                        {

                            string valueFormatted = null;
                            var value = item.GetType().GetProperty(column.Name).GetValue(item, null);
                            if (value != null)
                            {
                                var valueType = value.GetType();

                                if (valueType.Name == "Boolean")
                                {
                                    valueFormatted = value ? "是" : "否";
                                }
                                else if (valueType.Name == "DateTime")
                                {
                                    valueFormatted = value.ToString();
                                }
                         
                                if (column.Name.Contains("Price"))
                                {
                                    value = value + "€(HT)";
                                }

                            }
                            else
                            {
                                value = "";
                            }
                            if (value is IList && value.GetType().IsGenericType)
                            {
                                valueFormatted = "";
                            }

                            var cell = datarow.CreateCell(columnsCounter);

                            cell.SetCellValue(valueFormatted != null ? valueFormatted : value);

                            columnsCounter++;
                        }

                        rowIndex++;
                    }
                    /* Adapt the width of excel */
                    for (int columnNum = 0; columnNum < targetCoulmnsWithOrder.Count(); columnNum++)
                    {
                        int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                        //5为开始修改的行数，默认为0行开始
                        for (int rowNum = 0; rowNum <= sheet.LastRowNum; rowNum++)
                        {
                            IRow currentRow = sheet.GetRow(rowNum);
                            if (currentRow.GetCell(columnNum) != null)
                            {
                                ICell currentCell = currentRow.GetCell(columnNum);
                                int length = Encoding.Default.GetBytes(currentCell.ToString()).Length + 1;
                                if (columnWidth < length)
                                {
                                    columnWidth = length;
                                }
                            }
                        }
                        sheet.SetColumnWidth(columnNum, columnWidth * 256);
                    }


                    workbook.Write(fs);
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(newFile, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;

                return memory;
            }

            return null;
        }

    }
}
