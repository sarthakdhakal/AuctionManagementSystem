#pragma checksum "D:\DotNetCore\SEIIIAssignment\SEIIIAssignment\Views\Home\AdHoc.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "755466a4b99fd04fe85a9d75e114d8c9ac0737de"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_AdHoc), @"mvc.1.0.view", @"/Views/Home/AdHoc.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\DotNetCore\SEIIIAssignment\SEIIIAssignment\Views\_ViewImports.cshtml"
using SEIIIAssignment;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\DotNetCore\SEIIIAssignment\SEIIIAssignment\Views\_ViewImports.cshtml"
using SEIIIAssignment.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"755466a4b99fd04fe85a9d75e114d8c9ac0737de", @"/Views/Home/AdHoc.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"efd6c4cc725aa7cf17490d4e5b48b95fd0e65203", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_AdHoc : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\DotNetCore\SEIIIAssignment\SEIIIAssignment\Views\Home\AdHoc.cshtml"
  
    ViewData["Title"] = "Ad Hoc Report Page";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div id=""printableData"">
    <div class=""align-content-center"" style=""width:600px; height: 600px;"">
        <canvas id=""myChart""></canvas>
        <h3 id=""heading"">The piechart representing user ratio</h3>
    </div>

    <canvas id=""lineChart"" style=""height: 600px; width:750px""></canvas>
</div>
<input type=""button"" class=""btn btn-success"" value=""Print"" onclick=""printPartOfPage();""/>  
");
            DefineSection("scripts", async() => {
                WriteLiteral(@"
    <script src=""//cdn.jsdelivr.net/npm/chart.js@2.8.0""></script>
    <script type=""text/javascript"">
        $(function(){
           
        var ctx = document.getElementById('myChart').getContext('2d');
        var chart = new Chart(ctx, {
            type: 'pie',
            data: {
             labels: [""Admin"",""Buyer/Seller""],
                datasets: [{
                    
                    backgroundColor:[ ""#2ecc71"",""#3498db"" ],
                  
                    data: [");
#nullable restore
#line 27 "D:\DotNetCore\SEIIIAssignment\SEIIIAssignment\Views\Home\AdHoc.cshtml"
                      Write(ViewData["Admin"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(",");
#nullable restore
#line 27 "D:\DotNetCore\SEIIIAssignment\SEIIIAssignment\Views\Home\AdHoc.cshtml"
                                         Write(ViewData["Client"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("]\r\n                }]\r\n            }\r\n        });});\r\n    </script>\r\n    <script type=\"text/javascript\">\r\n        $(function () {\r\n            var labels = \'");
#nullable restore
#line 34 "D:\DotNetCore\SEIIIAssignment\SEIIIAssignment\Views\Home\AdHoc.cshtml"
                     Write(TempData["nameOfIndexes"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\';\r\n            var datas = \'");
#nullable restore
#line 35 "D:\DotNetCore\SEIIIAssignment\SEIIIAssignment\Views\Home\AdHoc.cshtml"
                    Write(TempData["values"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"';
            var labelsArray = new Array();
            var dataArray = new Array();
            for (var i = 0; i < labels.split(',').length; i++) {
                labelsArray.push(labels.split(',')[i]);
                dataArray.push(datas.split(',')[i]);
             
                
            }
            var ctx1 = document.getElementById(""lineChart"").getContext('2d');
            var lineChart = new Chart(ctx1, {
                type: 'bar',
                data: {
                    labels: labelsArray,
                    datasets: [{
                       
                        backgroundColor: '#3EB9DC',
                        data: dataArray
                    }]
                },
                options: {
                        legend: {display: false},
                      scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true,
                                    max:");
                WriteLiteral(@"20,
                                     stepSize: 1
                                }
                            }]
                        },
                    title: {
                         display: true,
                         text: ""Items in the system""
                       }
                    
                }
            });
        });
    </script>
    <script type=""text/javascript"">  

    function printPartOfPage() {  
        var heading = document.getElementById(""heading"");
  var canvas = document.getElementById(""myChart"");
                var img= canvas.toDataURL(""image/png"");
                var canvas2 = document.getElementById(""lineChart"");
                        var img2= canvas2.toDataURL(""image/png"");
var newWin=window.open('','Print-Window','width=400,height=400,top=100,left=100');
newWin.document.open();
newWin.document.write('<html><body   onload=""window.print()""><img src=""'+img+'"" alt=""""/><br>'+heading.innerHTML+'<br><br><img src=""'+img2+'"" alt=""""");
                WriteLiteral("/></body></html>\');\r\nnewWin.document.close();\r\nsetTimeout(function(){newWin.close();},10);\r\n             \r\n        \r\n        }\r\n \r\n    </script>\r\n");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
