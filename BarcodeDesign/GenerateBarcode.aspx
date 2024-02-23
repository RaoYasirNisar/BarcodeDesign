<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateBarcode.aspx.cs" Inherits="BarcodeDesign.GenerateBarcode" %>

<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Barcode Generation</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="css/main.css" rel="stylesheet" />
    <style>
        .qr-code-container {
            display: flex;
            flex-wrap: wrap;
        }

        .qr-code-item {
           margin-left:100px;
            margin-right: 20px;
            margin-bottom: 100px;
        }

        .qr-code {
            
            width: 200px;
            height: 200px;
        }

        .qr-code-id {
            display: block;
            text-align: center;
            margin-top: 10px;
        }

        @media print {
            @page :first {
                display: none;
            }
        }
    </style>
    <script>
        function printPage() {
            debugger;
            var firstPageContent = document.getElementById('first-page-content');
            firstPageContent.style.display = 'none';
            window.print();
            firstPageContent.style.display = '';
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-left: 20px">
            <div id="first-page-content">
                <header class="main-header">
                    <a>
                        <img src="../Images/MBSLogo.png" alt="MBS" class="logo" /></a>
                    <div class="site-name">
                        <h2>MBS QRCode Generation</h2>
                        <div class="SubHeading">QRCode Generation</div>
                    </div>
                </header>
                <div id="dvError" runat="server" visible="false" class="alert alert-primary show" role="alert">
                    <asp:Label ID="lblMessage" runat="server" Style="display:inline-block; font-weight: bold;"></asp:Label>
                </div>
                <div>
                    <asp:Panel ID="Panel1" CssClass="panelClass" runat="server">
                        <table width="100%" colspan="0" cellspacing="0" style="padding: 5px;">
                            <tr>
                                <td width="20%"></td>
                                <td width="30%">
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="import-button" />
                                </td>
                                <td width="10%" align="left">
                                    <asp:Button ID="btnRead" CssClass="import-button" runat="server" Text="Import Data" OnClick="btnRead_Click" />
                                </td>
                                <td align="center">
                                    <div>
                                        <asp:ImageButton ID="btnExportPrdTemp" runat="server" ImageUrl="~/Images/csv.png"
                                            ToolTip="Download Template" OnClick="btnExportPrdTemp_Click" />
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/Images/pdf.png" OnClick="btnExport_Click" ToolTip="Export QR Codes" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>

                <div class="row" style="margin-top: 20px;">
                    <div class="col-md-6 form-group">
                        <asp:Button ID="GenerateBarcodesButton" runat="server" Text="Generate Barcodes" CssClass="save-button" OnClick="BtnGenerateBarcode_Click" />
                    </div>
                    <div class="col-md-6 form-group" style="text-align: center;">
                        <asp:Button ID="PrintButton" runat="server" Text="Print QR Codes" CssClass="save-button"  OnClientClick="printPage(); return false;" />
                    </div>
                </div>
                <div class="row form-group">
                    <asp:Image ID="img" runat="server" />
                </div>
            </div>
            <asp:PlaceHolder ID="QRCodesPlaceHolder" runat="server"></asp:PlaceHolder>
            <%-- <asp:ScriptManager ID="asm" runat="server" />
            <asp:TextBox ID="searchInput" runat="server"></asp:TextBox>
            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                TargetControlID="searchInput"                        
                                ServiceMethod="SearchDataTable"
                                MinimumPrefixLength="1"
                                CompletionInterval="10"
                                >
                            </asp:AutoCompleteExtender>--%>
        </div>
        <div id="dvGrid" runat="server" class="grid-item">
            <asp:GridView ID="grvBarcode" runat="server" AutoGenerateColumns="False" CssClass="custom-gridview" Style="margin-top: 10px;"
                AllowPaging="true" PageSize="25" PagerStyle-CssClass="gridview-pager" OnPageIndexChanging="grvBarcode_PageIndexChanging">
                <PagerSettings Mode="NextPreviousFirstLast" />
                <PagerStyle CssClass="gridview-pager" />
                <Columns>
                    <%--   <asp:TemplateField HeaderText="Select To Generate" SortExpression="lblBarcodeGeneration">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelection" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField> --%>
                    <asp:TemplateField HeaderText="Barcode Generation ID" SortExpression="lblBarcodeGeneration">
                        <ItemTemplate>
                            <asp:Label ID="lblBarcodeGeneration" runat="server" Text='<%# Eval("barcode") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </div>
    </form>
    <%--  <script>
        function printPage() {
            window.print();
        }
    </script>--%>
</body>
</html>
