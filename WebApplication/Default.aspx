<%@ Page Title="GPX Data Analyzer" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication._Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:Panel ID="pnlFiles" CssClass="Center" style="width:100%;" runat="server">
        <h1 class="Center Section">File<asp:ImageButton ID="ibFiles" OnClick="ToggleFiles" ImageUrl="~/Images/collapse.jpg" style="margin-left:20px;" runat="server" /></h1>
        <asp:Panel ID="cpFiles" runat="server" Visible="true">
            <label class="Instructions">Select a File: </label>
            <asp:DropDownList ID="ddlFiles" runat="server" OnSelectedIndexChanged="FileChanged" AutoPostBack="true" Width="300px" />
        </asp:Panel>
    </asp:Panel>
    
    <asp:Panel ID="pnlTracks" CssClass="Center" style="width:100%;" Visible="false" runat="server">
        <h1 class="Center Section">Track<asp:ImageButton ID="ibTracks" OnClick="ToggleTracks" ImageUrl="~/Images/collapse.jpg" style="margin-left:20px;" runat="server" /></h1>
        <asp:Panel ID="cpTracks" runat="server" Visible="true">
            <label class="Instructions">Select a Track:</label>
            <asp:DropDownList ID="ddlTracks" runat="server" OnSelectedIndexChanged="TrackChanged" AutoPostBack="true" DataValueField="name" DataTextField="name" />
        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlAnalysis" Visible="false" runat="server">
        <h1 class="Center Section">Data<asp:ImageButton ID="ibData" OnClick="ToggleData" ImageUrl="~/Images/expand.jpg" style="margin-left:20px;" runat="server" /></h1>
        <asp:Panel ID="cpData" runat="server" Visible="false">
            <h1 class="Center">Track Points: <asp:Label ID="lblTrackPointCount" runat="server" Text="" /> <asp:ImageButton ID="ibTrackPoints" OnClick="ToggleTrackPoints" ImageUrl="~/Images/expand.jpg" style="margin-left:20px;" runat="server" /></h1>
            <asp:Panel ID="cpTrackPoints" runat="server" Visible="false">
                <asp:GridView ID="gvTrackPoints" runat="server" AutoGenerateColumns="false" CssClass="GridView" 
                    RowStyle="GridRow" AlternatingRowStyle="GridAlternateRow" HeaderStyle-CssClass="GridHeader">
                    <Columns>
                        <asp:BoundField HeaderText="Time"      DataField="time" />
                        <asp:BoundField HeaderText="Latitude"  DataField="lon" DataFormatString="{0:N8}&deg;" />
                        <asp:BoundField HeaderText="Longitude" DataField="lat" DataFormatString="{0:N8}&deg;" />
                        <asp:BoundField HeaderText="Elevation" DataField="ele" DataFormatString="{0:N5} m" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <h1 class="Center">Segments <asp:Label ID="lblSegmentCount" runat="server" Text="" /> <asp:ImageButton ID="ibSegments" OnClick="ToggleSegments" ImageUrl="~/Images/expand.jpg" style="margin-left:20px;" runat="server" /></h1>
            <asp:Panel ID="cpSegments" runat="server" Visible="false">
                <asp:GridView ID="gvSegments" runat="server" AutoGenerateColumns="false" CssClass="GridView" 
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
            </asp:Panel>
            <h1 class="Center">Totals<asp:ImageButton ID="ibTotals" OnClick="ToggleTotals" ImageUrl="~/Images/expand.jpg" style="margin-left:20px;" runat="server" /></h1>
            <asp:Panel ID="cpTotals" runat="server" Visible="false">
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
            </asp:Panel>
        </asp:Panel>

        <h1 class="Center Section">Graphs<asp:ImageButton ID="ibGraphs" OnClick="ToggleGraphs" ImageUrl="~/Images/expand.jpg" style="margin-left:20px;" runat="server" /></h1>
        <asp:Panel ID="cpGraphs" runat="server" Visible="false">
            <h1 class="Center">Distance, Velocity, Acceleration<asp:ImageButton ID="ibDVA" OnClick="ToggleDVA" ImageUrl="~/Images/collapse.jpg" style="margin-left:20px;" runat="server" /></h1>
            <asp:Panel ID="cpDVA" runat="server" Visible="true">
                <%--<asp:Chart ID="chartDVA" runat="server">
                    <series><asp:Series Name="Series1"></asp:Series></series>
                    <chartareas><asp:ChartArea Name="ChartArea1"></asp:ChartArea></chartareas>
                </asp:Chart>--%>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    
    <asp:Panel ID="pnlActivity" CssClass="Center" style="width:100%;" Visible="false" runat="server">
        <h1 class="Center Section">Activity<asp:ImageButton ID="ibActivity" OnClick="ToggleActivity" ImageUrl="~/Images/collapse.jpg" style="margin-left:20px;" runat="server" /></h1>
        <asp:Panel ID="cpActivity" runat="server" Visible="true">
            <label class="Instructions">Select an Activity:</label>
            <asp:DropDownList ID="ddlActivity" runat="server" OnSelectedIndexChanged="ActivityChanged" AutoPostBack="true">
                <asp:ListItem Text="Not Sure" Value="NotSure"></asp:ListItem>
                <asp:ListItem Text="Hiking" Value="Hiking"></asp:ListItem>
                <asp:ListItem Text="Jogging" Value="Jogging"></asp:ListItem>
                <asp:ListItem Text="Downhill Skiing" Value="Downhill"></asp:ListItem>
                <asp:ListItem Text="Cross Country Skiing" Value="CrossCountry"></asp:ListItem>
                <asp:ListItem Text="Snowboarding" Value="Downhill"></asp:ListItem>
                <asp:ListItem Text="Snowmobiling" Value="Vehicle"></asp:ListItem>
                <asp:ListItem Text="4-Wheeling" Value="Vehicle"></asp:ListItem>
                <asp:ListItem Text="Driving" Value="Vehicle"></asp:ListItem>
                <asp:ListItem Text="Flying" Value="Flying"></asp:ListItem>
            </asp:DropDownList>
    
            <asp:Panel ID="pnlNotSure" style="width:100%;" Visible="false" runat="server">
                Enter an activity to see more information
            </asp:Panel>
    
            <asp:Panel ID="pnlHiking" style="width:100%;" Visible="false" runat="server">
                Hiking Information Here<br />
                <div>Total Time: <asp:Label ID="lblHikingTotalTime" runat="server" /> s</div>
            </asp:Panel>
    
            <asp:Panel ID="pnlJogging" style="width:100%;" Visible="false" runat="server">
                Jogging Information Here
            </asp:Panel>
    
            <asp:Panel ID="pnlDownhill" style="width:100%;" Visible="false" runat="server">
                Downhill Information Here
            </asp:Panel>
    
            <asp:Panel ID="pnlCrossCountry" style="width:100%;" Visible="false" runat="server">
                Cross Country Information Here
            </asp:Panel>
    
            <asp:Panel ID="pnlVehicle" style="width:100%;" Visible="false" runat="server">
                Vehicle Information Here
            </asp:Panel>
    
            <asp:Panel ID="pnlFlight" style="width:100%;" Visible="false" runat="server">
                Flight Information Here
            </asp:Panel>

        </asp:Panel>

    </asp:Panel>

</asp:Content>
