<%@ Page Title="GPX Data Analyzer" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication._Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        label
        {
            font-weight:bold;
            margin:2px;
        }
    </style>
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
                <div>Total Distance: <asp:Label ID="lblTotalDistance" runat="server" /></div>
                <div>Total Vertical Distance: <asp:Label ID="lblTotalVerticalDistance" runat="server" /></div>
                <div>Total Flat Earth Distance: <asp:Label ID="lblTotalFlatEarthDistance" runat="server" /></div>
                <br />
                <div>Total Time: <asp:Label ID="lblTotalTime" runat="server" /></div>
                <br />
                <div>Average Distance: <asp:Label ID="lblAverageDistance" runat="server" /></div>
                <div>Average Vertical Distance: <asp:Label ID="lblAverageVerticalDistance" runat="server" /></div>
                <div>Average Flat Earth Distance: <asp:Label ID="lblAverageFlatEarthDistance" runat="server" /></div>
                <br />
                <div>Average Time: <asp:Label ID="lblAverageTime" runat="server" /></div>
                <div>Average Course: <asp:Label ID="lblAverageCourse" runat="server" /></div>
                <br />
                <div>Average Velocity: <asp:Label ID="lblAverageVelocity" runat="server" /></div>
                <div>Average Vertical Velocity: <asp:Label ID="lblAverageVerticalVelocity" runat="server" /></div>
                <div>Average Flat Earth Velocity: <asp:Label ID="lblAverageFlatEarthVelocity" runat="server" /></div>
            </asp:Panel>
        </asp:Panel>

        <h1 class="Center Section">Graphs<asp:ImageButton ID="ibGraphs" OnClick="ToggleGraphs" ImageUrl="~/Images/expand.jpg" style="margin-left:20px;" runat="server" /></h1>
        <asp:Panel ID="cpGraphs" runat="server" Visible="false">
            <h1 class="Center">Velocity<asp:ImageButton ID="ibVelocity" OnClick="ToggleVelocity" ImageUrl="~/Images/collapse.jpg" style="margin-left:20px;" runat="server" /></h1>
            <asp:Panel ID="cpVelocity" runat="server" Visible="true">
                <asp:Chart ID="chartVelocity" runat="server"><Series>
                    <asp:Series Name="Velocity" ChartType="Area" Color="LightGray">
                        <Points>
                            <asp:DataPoint XValue="0" YValues="1000" />
                            <asp:DataPoint XValue="5" YValues="2500" />
                            <asp:DataPoint XValue="10" YValues="6000" />
                            <asp:DataPoint XValue="15" YValues="4000" />
                            <asp:DataPoint XValue="20" YValues="2500" />
                            <asp:DataPoint XValue="25" YValues="2000" />
                            <asp:DataPoint XValue="30" YValues="1500" />
                            <asp:DataPoint XValue="35" YValues="1200" />
                            <asp:DataPoint XValue="40" YValues="1000" />
                            <asp:DataPoint XValue="45" YValues="500" />
                            <asp:DataPoint XValue="50" YValues="0" />
                        </Points>
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="chaVelocity" BackColor="#f2f2f2">
                        <AxisY Title="Velocity" Interval="1000" IntervalType="Auto" IsMarginVisible="false">
                            <LabelStyle Font="Aerial, 8.25pt" />
                            <MajorGrid Enabled="false" />
                        </AxisY>
                        <AxisX Title="Time" Interval="10" IntervalType="Auto" IsStartedFromZero="true" Minimum="0" IsMarginVisible="false">
                            <LabelStyle Font="Aerial, 8.25pt" />
                            <MajorGrid Enabled="false" />
                        </AxisX>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
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
                <asp:ListItem Text="Snowboarding" Value="Snowboarding"></asp:ListItem>
                <asp:ListItem Text="Snowmobiling" Value="Snowmobiling"></asp:ListItem>
                <asp:ListItem Text="4-Wheeling" Value="4Wheeling"></asp:ListItem>
                <asp:ListItem Text="Driving" Value="Driving"></asp:ListItem>
                <asp:ListItem Text="Flying" Value="Flying"></asp:ListItem>
            </asp:DropDownList>
    
            <asp:Panel ID="pnlNotSure" style="width:100%;" Visible="false" runat="server">
                <h3 class="Center">Enter an activity to see more information!</h3>
            </asp:Panel>
    
            <asp:Panel ID="pnlSlow" style="width:100%;" Visible="false" runat="server">
                <h3 class="Center"><asp:Label ID="lblSlowTitle" runat="server" /></h3>

                <div>Average <%= ddlActivity.SelectedItem.Text %> Speed: <asp:Label ID="lblAverageHikeSpeed" runat="server" /></div>
                <div>Total <%= ddlActivity.SelectedItem.Text %> Time: <asp:Label ID="lblTotalHikeTime" runat="server" /></div>

                <div>Average Uphill Speed: <asp:Label ID="lblUphillHikeSpeed" runat="server" /></div>
                <div>Average Downhill Speed: <asp:Label ID="lblDownhillHikeSpeed" runat="server" /></div>
                
                <div>Lowest Elevation: <asp:Label ID="lblMinElevation" runat="server" /></div>
                <div>Highest Elevation: <asp:Label ID="lblMaxElevation" runat="server" /></div>
                
                <div>Starting Elevation: <asp:Label ID="lblStartElevation" runat="server" /></div>
                <div>Ending Elevation: <asp:Label ID="lblEndElevation" runat="server" /></div>

                <div>Number of Rests: <asp:Label ID="lblNumberHikingRests" runat="server" /></div>
                <div>Total Rest Time: <asp:Label ID="lblTotalHikeRestTime" runat="server" /></div>
            </asp:Panel>
    
            <asp:Panel ID="pnlDownhill" style="width:100%;" Visible="false" runat="server">
                <h3 class="Center"><asp:Label ID="lblDownhillTitle" runat="server" /></h3>
                
                <div>Number of Runs: <asp:Label ID="lblNumberOfRuns" runat="server" /></div>
                <div>Number of Lifts: <asp:Label ID="lblNumberOfLifts" runat="server" /></div>
                <div>Number of Falls: <asp:Label ID="lblNumberOfFalls" runat="server" /></div>
                
                <div>Total Distance: <asp:Label ID="lblTotalDownhillDistance" runat="server" /></div>
                <div>Vertical Difference: <asp:Label ID="lblVerticalDistance" runat="server" /></div>

                <div>Average Lift Speed: <asp:Label ID="lblAverageLiftSpeed" runat="server" /></div>
                <div>Average <%= ddlActivity.SelectedItem.Text %> Speed: <asp:Label ID="lblAverageSkiSpeed" runat="server" /></div>
            </asp:Panel>
    
            <asp:Panel ID="pnlFast" style="width:100%;" Visible="false" runat="server">

                <h3 class="Center"><asp:Label ID="lblFastTitle" runat="server" /></h3>

                <div>Total Distance: <asp:Label ID="lblTotalFastDistance" runat="server" /></div>
                
                <div>Number of Stops: <asp:Label ID="lblNumberStops" runat="server" /></div>
                <div>Maximum Acceleration: <asp:Label ID="lblMinDeceleration" runat="server" /></div>
                <div>Maximum Deceleration: <asp:Label ID="lblMaxDeceleration" runat="server" /></div>
                
                <div>Rest Time: <asp:Label ID="lblVehicleRestTime" runat="server" /></div>
                <div>Coasting Time: <asp:Label ID="lblCoastTime" runat="server" /></div>
                <div>Acceleration Time: <asp:Label ID="lblAccelerationTime" runat="server" /></div>
                <div>Deceleration Time: <asp:Label ID="lblDecelerationTime" runat="server" /></div>

            </asp:Panel>
    
            <asp:Panel ID="pnlFlight" style="width:100%;" Visible="false" runat="server">

                <h3 class="Center"><asp:Label ID="lblFlightTitle" runat="server" /></h3>
                
                <div>Total Time in Flight: <asp:Label ID="lblTotalFlightTime" runat="server" /></div>
                <div>Total Distance Flown: <asp:Label ID="lblTotalFlightDistance" runat="server" /></div>
                
                <div>Average Flight Velocity: <asp:Label ID="lvlAverageFlightVelocity" runat="server" /></div>
                <div>Average Climbing Velocity: <asp:Label ID="lblAverageClimbingVelocity" runat="server" /></div>
                <div>Average Descent Velocity: <asp:Label ID="lblAverageDescentVelocity" runat="server" /></div>

                <div>Average Velocity: <asp:Label ID="lblAverageFlightVelocity" runat="server" /></div>
                <div>Maximum Velocity: <asp:Label ID="lblMaximumVelocity" runat="server" /></div>
                <div>Maximum Acceleration: <asp:Label ID="lblMaximumAcceleration" runat="server" /></div>
                <div>Maximum Deceleration: <asp:Label ID="lblMaximumDeceleration" runat="server" /></div>

            </asp:Panel>

        </asp:Panel>

    </asp:Panel>

</asp:Content>
