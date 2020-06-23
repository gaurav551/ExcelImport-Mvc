using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExcelImport.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading;
using OfficeOpenXml;
using ExcelImport.Data;

namespace ExcelImport.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.UserInfos.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
    [HttpPost]  
public async Task<IActionResult> Import(IFormFile file, CancellationToken cancellationToken)  
{  
    if (file == null || file.Length <= 0)  
    {  
        System.Console.WriteLine("Empty file");
        return Ok( "file is empty");  
        
    }  
  
    if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))  
    {  
        System.Console.WriteLine("Not supported");
        return Ok( "Not Support file extension");  
    }  
  
    var list = new List<UserInfo>();  
  
    using (var stream = new MemoryStream())  
    {  
        await file.CopyToAsync(stream, cancellationToken);  
  
        using (var package = new ExcelPackage(stream))  
        {  
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];  
            var rowCount = worksheet.Dimension.Rows;  
  
            for (int row = 2; row <= rowCount; row++)  
            {  
                list.Add(new UserInfo  
                {  
                    UserName = worksheet.Cells[row, 1].Value.ToString().Trim(),  
                    Age = int.Parse(worksheet.Cells[row, 2].Value.ToString().Trim()), 
                   
                }

                );  
            }  
            foreach(var x in list)
            {
              _context.UserInfos.Add(x);
              _context.SaveChanges();
            }
        }  
    }  
  
    // add list to db ..  
    // here just read and return  
  
    return RedirectToAction(nameof(Index));  
}  
}
}
