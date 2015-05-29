<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Analysis.aspx.cs" Inherits="Analysis.Analysis" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript" language="javascript" >
    function validate()
    {
        if(document.getElementById("<%=fuReview.ClientID %>").value == "")
        {
            alert('Please select file.');
            return false;
        }
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table style='width:100%; ' cellpadding='0' cellspacing='0' >
        <tr>
            <td style='padding-left:10px; ' ></td>
            <td>
            <table style='width:100%; ' cellpadding='0' cellspacing='0' >
                <tr><td colspan='2' ><asp:Label runat='server' ID="lblHead" Text="Review Analysis" Font-Bold="true" ></asp:Label></td></tr>
                <tr><td style='height:10px; ' ></td></tr>
                <tr>
                    <td style='width:200px; ' ><asp:Label runat="server" Text="Select review file" ></asp:Label></td>
                    <td><asp:FileUpload runat="server" ID="fuReview" /></td>
                </tr>
                <tr><td style='height:10px; ' ></td></tr>
                <tr>
                    <td></td>
                    <td><asp:Button runat="server" ID="btnAnalysis" Text="Analysis" onclick="btnAnalysis_Click" OnClientClick="return validate()" /></td>
                </tr>                
                <tr><td style='height:10px; ' ></td></tr>
                <tr>
                    <td colspan='2' >
                    <%--<asp:Panel runat="server" ID="pnlAnalysis" Height="500px" ScrollBars="Auto" >--%>
                    <asp:Table runat="server" ID="tblAnalysis" ></asp:Table>
                    <%--</asp:Panel>--%>
                    </td>
                </tr>
            </table>
            </td>
            <td style='padding-left:10px; ' ></td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
