<%@ Page Title="Login - " Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="WebShop.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH" runat="server">
    <script type="text/javascript">
        // fix for using AJAX UpdatePanel together with jQuery, for more reading see: 
        // http://blog.dreamlabsolutions.com/post/2009/02/24/jQuery-document-ready-and-ASP-NET-Ajax-asynchronous-postback.aspx or
        // http://stackoverflow.com/questions/256195?tab=newest#tab-top
        //    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //    function EndRequestHandler(sender, args) {
        //        if (args.get_error() == undefined) {
        //            InitFocus("PackagesListTable");
        //        }
        //    }

        $(document).ready(function () {
            InitFocus("UserName");
        });
    </script>
    <div class="SolidDiv Padding CenterAlignColumn" style="width: 96%;">
        <asp:Login ID="UserLogin" runat="server">
        </asp:Login>
    </div>
</asp:Content>
