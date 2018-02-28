<%@ Page Title="GPX Data Analyzer" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="WebApplication._Upload" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <asp:Panel ID="pnlUpload" CssClass="Center" style="width:100%;" runat="server">
        <div>
            <h1 class="Center Section">Upload a New .gpx File</h1>
            <label class="Instructions">Select a .gpx file to upload:</label>
            <div class="FileInput">
                <input type="text" id="corrected" name="corrected" class="FileInputTextbox" style="width:350px;margin-right:10px;" />
                <div class="FileInputDiv">
                    <input type="button" value="Select File" class="BigRedButton Down" />
                    <asp:FileUpload ID="fileUpload" class="FileInputHidden" runat="server" onchange="javascript:document.getElementById('corrected').value = this.value.split('\\').pop().split('/').pop()" />
                </div>
            </div>
            <asp:Button ID="btnUpload" CssClass="RedButton Down" style="float:left;" OnClick="UploadClicked" Text="Upload" runat="server" />
        </div>
        <div style="clear:both;">
            <asp:Label ID="lblStatus" Text="Upload Status:" style="margin-top:10px;" runat="server" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlFiles" runat="server">
        <h1 class="Center Section">Uploaded Files (<asp:Label ID="lblFileCount" runat="server" Text="" />)</h1>
        <br />
        <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" OnDataBound="FilesDataBound" CssClass="GridView" 
            RowStyle="GridRow" AlternatingRowStyle="GridAlternateRow" HeaderStyle-CssClass="GridHeader">
            <Columns>
                <asp:BoundField HeaderText=""            DataField="Number" ItemStyle-Width="30px" />
                <asp:BoundField HeaderText="Name"        DataField="Name" />
                <asp:BoundField HeaderText="Size"        DataField="SizeKB" DataFormatString="{0} kB" ItemStyle-Width="60px" />
                <asp:BoundField HeaderText="Uploaded On" DataField="Uploaded" ItemStyle-Width="130px" />
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="btnSave" Text="Save" ToolTip="Download this file" OnCommand="SaveFile" CommandArgument='<%# Eval("Name") %>' CssClass="BlueButton" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

</asp:Content>
