#pragma checksum "C:\Arm\Proj\2C2P.TechnicalTest\2C2P.FileUploader\2C2P.FileUploader\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "da5e51967460c221de5e9075b2d93027b414b4d1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
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
#line 1 "C:\Arm\Proj\2C2P.TechnicalTest\2C2P.FileUploader\2C2P.FileUploader\Views\_ViewImports.cshtml"
using _2C2P.FileUploader;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Arm\Proj\2C2P.TechnicalTest\2C2P.FileUploader\2C2P.FileUploader\Views\_ViewImports.cshtml"
using _2C2P.FileUploader.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"da5e51967460c221de5e9075b2d93027b414b4d1", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"55864f3b34ec939ac684987e0a2780cb792741e6", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Arm\Proj\2C2P.TechnicalTest\2C2P.FileUploader\2C2P.FileUploader\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""text-center"">
    <h1 class=""display-4"">Welcome</h1>
</div>

<dl>
    <dt>
        <label for=""FileUpload_FormFile"">File</label>
    </dt>
    <dd>
        <input id=""FileUpload_FormFile"" type=""file"" accept="".csv, .xml""
               name=""FileUpload.FormFile"" />
    </dd>
</dl>

<button class=""btn btn-primary"" onclick=""uploadFile()"">Upload</button>

<div style=""margin-top:15px"">
    <output id=""result""></output>
    <p id=""error-detail""></p>
</div>


<script>

    function uploadFile() {
        var resultElement = document.getElementById(""result"");
        var detailElement = document.getElementById(""error-detail"");
        var formData = new FormData();
        var file = $('#FileUpload_FormFile')[0].files[0];
        formData.append('formFile', file);

        $.ajax({
            url: '/api/Transactions',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (r");
            WriteLiteral(@"esponse) {
                console.log(response);
                resultElement.value = 'Result : ' + response;
                detailElement.innerText = '';
            },
            error: function (response) {
                console.log(response);
                resultElement.value = 'Result : ' + response.status;
                detailElement.innerText = response.responseText;
            }
        });
    }
</script>");
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
