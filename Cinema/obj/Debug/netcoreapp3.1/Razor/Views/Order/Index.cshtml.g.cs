#pragma checksum "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5d2b8bbed320e2375808f007d677186ba4456dc3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Order_Index), @"mvc.1.0.view", @"/Views/Order/Index.cshtml")]
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
#line 1 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\_ViewImports.cshtml"
using Cinema;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\_ViewImports.cshtml"
using Cinema.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5d2b8bbed320e2375808f007d677186ba4456dc3", @"/Views/Order/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a023e9d7789314764daf058c827f547f57c6aebf", @"/Views/_ViewImports.cshtml")]
    public class Views_Order_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<Cinema.Models.Domain.Order>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
  
    ViewBag.Title = "Tickets";
    Layout = "_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h2>Orders</h2>\r\n\r\n");
#nullable restore
#line 10 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
 if (Model == null || Model.Count < 1 || !Model.Any())
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h2>No Orders!</h2>\r\n");
#nullable restore
#line 13 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <table class=""table"">
        <thead class=""thead-dark"">
        <tr>
            <th class=""col-md-1"">Customer</th>
            <th class=""col"">Movie Name</th>
            <th class=""col"">Seats</th>
            <th class=""col"">Total Price</th>
            <th class=""col"">Booking Date</th>
            <th class=""col"">Booking Time</th>
        </tr>
        </thead>

        <tbody>
");
#nullable restore
#line 29 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
         foreach (var order in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n");
            WriteLiteral("                <td></td>\r\n                <td>");
#nullable restore
#line 34 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
               Write(order.MovieName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td>[\r\n");
#nullable restore
#line 36 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
                     foreach (var seatNumber in order.Seats.OrderBy(s => s))
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <span>");
#nullable restore
#line 38 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
                         Write(seatNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </span>\r\n");
#nullable restore
#line 39 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    ]\r\n                </td>\r\n\r\n                <td>");
#nullable restore
#line 43 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
               Write(order.TotalPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 44 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
               Write(order.BookingDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 45 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
               Write(order.BookingTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n");
#nullable restore
#line 47 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n        <tfoot></tfoot>\r\n    </table>\r\n    <a");
            BeginWriteAttribute("href", " href=\"", 1292, "\"", 1334, 1);
#nullable restore
#line 51 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"
WriteAttributeValue("", 1299, Url.Action("GeneratePdf", "Order"), 1299, 35, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Export to PDF</a>\r\n");
#nullable restore
#line 52 "C:\Users\Ace\Desktop\Cinema\Cinema\Views\Order\Index.cshtml"

}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<Cinema.Models.Domain.Order>> Html { get; private set; }
    }
}
#pragma warning restore 1591
