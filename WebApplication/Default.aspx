<%@ Page Title="GPX Data Analyzer" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <asp:Panel ID="pnlFile" CssClass="Center" style="width:100%;" runat="server">
        <div>
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
    
    <asp:Panel ID="pnlTracks" CssClass="Center" style="width:100%;" Visible="false" runat="server">
        <hr />
        <label class="Instructions">Select a Track:</label> <asp:DropDownList ID="ddlTracks" runat="server" OnSelectedIndexChanged="TrackChanged" AutoPostBack="true" DataValueField="name" DataTextField="name" />
    </asp:Panel>

    <asp:Panel ID="pnlAnalysis" Visible="false" runat="server">
        <hr />
        <h1 class="Center">Track Points: <asp:Label ID="lblTrackPointCount" runat="server" Text="" /></h1>
        <asp:GridView ID="gvTrackPoints" runat="server" AutoGenerateColumns="false" CssClass="GridView" 
            RowStyle="GridRow" AlternatingRowStyle="GridAlternateRow" HeaderStyle-CssClass="GridHeader">
            <Columns>
                <asp:BoundField HeaderText="Time"      DataField="time" />
                <asp:BoundField HeaderText="Latitude"  DataField="lon" />
                <asp:BoundField HeaderText="Longitude" DataField="lat" />
                <asp:BoundField HeaderText="Elevation" DataField="ele" />
            </Columns>
        </asp:GridView>
        <br />
        <h1 class="Center">Analysis</h1>
        <asp:GridView ID="gvBetween" runat="server" AutoGenerateColumns="false" CssClass="GridView" 
            RowStyle="GridRow" AlternatingRowStyle="GridAlternateRow" HeaderStyle-CssClass="GridHeader">
            <Columns>
                <asp:BoundField HeaderText="Name"       DataField="Name" />
                <asp:BoundField HeaderText="Distance"   DataField="Distance"          DataFormatString="{0:N2} m" />
                <asp:BoundField HeaderText="Vertical"   DataField="VerticalDistance"  DataFormatString="{0:N2} m" />
                <asp:BoundField HeaderText="Flat Earth" DataField="FlatEarthDistance" DataFormatString="{0:N2} m" />
                <asp:BoundField HeaderText="Time"       DataField="Time"              DataFormatString="{0:N2} s" />
                <asp:BoundField HeaderText="Velocity"   DataField="Velocity"          DataFormatString="{0:N2} m/s" />
                <asp:BoundField HeaderText="Vertical"   DataField="VerticalVelocity"  DataFormatString="{0:N2} m/s" />
                <asp:BoundField HeaderText="Flat Earth" DataField="FlatEarthVelocity" DataFormatString="{0:N2} m/s" />
                <asp:BoundField HeaderText="Course"     DataField="Course"            DataFormatString="{0:N2}" />
            </Columns>
            <EmptyDataTemplate>
                <div style="width:100%;text-align:center;color:Red;">
                    Not enough data to analyze
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
        <br />
        <h1 class="Center">Totals</h1>
        <div>Total Distance: <asp:Label ID="lblTotalDistance" runat="server" /> m</div>
        <div>Total Vertical Distance: <asp:Label ID="lblTotalVerticalDistance" runat="server" /> m</div>
        <div>Total Flat Earth Distance: <asp:Label ID="lblTotalFlatEarthDistance" runat="server" /> m</div>
        <br />
        <div>Total Time: <asp:Label ID="lblTotalTime" runat="server" /> s</div>
        <br />
        <div>Average Distance: <asp:Label ID="lblAverageDistance" runat="server" /> m</div>
        <div>Average Vertical Distance: <asp:Label ID="lblAverageVerticalDistance" runat="server" /> m</div>
        <div>Average Flat Earth Distance: <asp:Label ID="lblAverageFlatEarthDistance" runat="server" /> m</div>
        <br />
        <div>Average Time: <asp:Label ID="lblAverageTime" runat="server" /> s</div>
        <div>Average Course: <asp:Label ID="lblAverageCourse" runat="server" /></div>
        <br />
        <div>Average Velocity: <asp:Label ID="lblAverageVelocity" runat="server" /> m/s</div>
        <div>Average Vertical Velocity: <asp:Label ID="lblAverageVerticalVelocity" runat="server" /> m/s</div>
        <div>Average Flat Earth Velocity: <asp:Label ID="lblAverageFlatEarthVelocity" runat="server" /> m/s</div>
        <br />
    </asp:Panel>

</asp:Content>
