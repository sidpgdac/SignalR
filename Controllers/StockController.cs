using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OfficeOpenXml;

namespace SignalrProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IHubContext<SignalrHub> _hubContext;

        public StockController(IHubContext<SignalrHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetExcelData()
        {
            var filePath = @"C:\AspNetCore\Signalrdemo\signalRdummyData.xlsx";
            FileInfo fileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                var data = new List<List<string>>();

                for (int row = 1; row <= rowCount; row++)
                {
                    var rowData = new List<string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        rowData.Add(worksheet.Cells[row, col].Value?.ToString());
                    }
                    data.Add(rowData);
                }

                return Ok(data);
            }
        }
    }
}
