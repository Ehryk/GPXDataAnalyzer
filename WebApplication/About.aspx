<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="WebApplication.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <h1 class="Center Section">Abstract</h1>
    <p>
        This project allows you to upload GPX files and analyze track data contained within.  Initial Release: December 2011.
        Written in ASP.NET, C# and C++.
    </p>
    <h1 class="Center Section">Authors</h1>
    <p>
        Web Application, .gpx file management and parsing, baseline analysis and UI done by: <b>Eric Menze</b> <a href="mailto:ehryk42@gmail.com">Email</a>
    </p>
    <p>
        Planning, data collection, and activity specific analysis done by:<br />
        <b>Paul Winter</b> - UAA <a href="mailto:pjmwinter@hotmail.com">Email</a><br />
        <b>Travis Wilson</b> - UAA<br />
        <b>Gabe Degange</b> - UAA
    </p>
    <h1 class="Center Section">Details</h1>
    <p>
        This project consists of:<br />
        <br />
        <b>WebApplication</b> - Web Interface, UI, Upload support.  C# Web Application.  Eric Menze.<br />
        <b>GPX</b> - Parses a .gpx file for GPS data (lat, lon, ele, time).  C# DLL.  Eric Menze.<br />
        <b>DataAnalysis</b> - Provides detailed activity specific analysis.  C++ DLL.  Paul Winter, Gabe Degange, Travis Wilson.<br />
        <br />
        <b>ConsoleApplication</b> - Console Interface to Data Analysis and GPX DLLs.  C# Console Application.  Eric Menze.<br />
        <b>DataAnalysisConsole</b> - Console interface to Data Analysis DLL.  C++ Console Application.  Eric Menze, Paul Winter, Gabe Degange, Travis Wilson.<br />
    </p>

</asp:Content>
