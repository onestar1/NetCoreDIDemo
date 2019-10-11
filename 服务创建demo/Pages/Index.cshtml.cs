using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using static 中间件跟服务.DIServiceDemo1;

namespace 服务创建demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly iservicedemo _hhh;
        public IndexModel(ILogger<IndexModel> logger,iservicedemo iservicedemo)
        {
            _logger = logger;
            _hhh = iservicedemo;
        }

        public void OnGet()
        {
            ViewData["hhh"]=_hhh.getname();


        }
    }
}
