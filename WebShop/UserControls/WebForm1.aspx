<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebShop.UserControls.WebForm1"
     %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="../scripts/jquery-1.4.2.js"></script>

    <script type="text/javascript">
        $("document").ready(function() {
            $("div.ControlDiv").mouseover(function(evt) {
                $(this).css("display", "none");
                //$(this).html("pageX: " + evt.pageX + ", pageY: " + evt.pageY + ", type: " + evt.type + ", target: " + evt.target);

            });

            $("div.ControlDiv").mouseleave(function(evt) {
                $(this).css("display", "block");
                //$(this).html("pageX: " + evt.pageX + ", pageY: " + evt.pageY + ", type: " + evt.type + ", target: " + evt.target);
            });


            $("#theList tr").hover(
            function() {
                $(this).toggleClass("highlight");
            },
            function() {
                $(this).toggleClass("highlight");
            }
        );
        });

        //$("div.ControlDiv").css("display", "none");

        //            var counter = 0;

        //            $("div.ControlDiv").each(function() {
        //                $(this).attr("id", "ControlId" + counter);

        //                $("this").hover(HideControl, HideControl);
        //                //                $(this).bind("mouseover", function(evt) {
        //                //                    $(this).attr("class", "highlighted");
        //                //                });

        //                //                $(this).bind("mouseleave", function(evt) {
        //                //                $(this).attr("class", "ControlDiv");
        //                //                });

        //                counter++;
        //            });

        //$("#para2").hover(HideControl, HideControl);


        //$("a").attr("target", "_blank");
        //            $("div:not([id=header]) " + strWhichTag).each(function() {
        //                $(this).html("<a name='bookmark" + cAnchorCount + "'></a>" + $(this).html());
        //                oList.append($("<li><a href='#bookmark" + cAnchorCount++ + "'> " + $(this).text() + "</a></li>"));
        //            });

        //.css("display", "none")
        //});

        function HideControl(evt) {
            $("#para2").toggleClass("highlighted")
        }
    </script>

    <style type='text/css'>
        .ControlDiv
        {
            display: block;
            border: solid 1px red;
        }
        .highlight
        {
            background-color: #CCC;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="background-color: White; margin: 15px 15px 15px 15px; padding: 15px 15px 15px 15px;">
        <shaz:ImageManager ID="ImageManager1" runat="server" FolderUrl="~/ProductImages/" />
        <div style="border: solid black 1px;">
            <p id="para2" class="ControlDiv">
                test</p>
        </div>
    </div>
    </form>
</body>
</html>
