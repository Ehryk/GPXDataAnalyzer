// DataAnalyzerConsole.cpp : main project file.

#include "stdafx.h"
#include "DataAnalyzerConsole.h"

using namespace System;

#pragma region Main

int main(array<System::String^>^ args)
{
    Console::WriteLine("GPX Data Analyzer Console Interface");
    Console::WriteLine();
    Console::Write("Initializing DataAnalyzer Object... ");
	
    DataAnalyzer^ analyzer = gcnew DataAnalyzer(100, 220.27917, System::DateTime::Now, -150.17818630, 61.89438680);
	
    Console::WriteLine("Done.");
    Console::Write("Loading 10 Sample Segments... ");

    AddSampleSegments(analyzer);
	
    Console::WriteLine("Done.");
    Console::WriteLine();

	bool loop = true;
	while (loop)
	{
		try
		{
			Console::WriteLine("Number of Segments = {0}", analyzer->GetN());
			Console::WriteLine("Max Segments = {0}", analyzer->GetMax());
			Console::WriteLine();
			Console::WriteLine("Average Distance = {0}", analyzer->GetAverageDistance());
			Console::WriteLine("Total Distance = {0}", analyzer->GetTotalDistance());
			Console::WriteLine("Average Velocity = {0}", analyzer->GetAverageVelocity());
			Console::WriteLine();

			Console::Write("> ");
			String^ entry = Console::ReadLine();
		
			String^ command = entry->Split(' ')[0]->ToUpper();
			array<String^>^ segments = entry->Split(' ');
		
			if (command == "QUIT" || command == "Q" || command == "EXIT" || command == "E")
				break;
			else if (command == "ADD" || command == "A")
			{
				analyzer->AddSegment(Double::Parse(segments[1]), Double::Parse(segments[2]), Double::Parse(segments[3]), Double::Parse(segments[4]), Double::Parse(segments[5]));
				Console::WriteLine("Added segment.");
			}
			else if (command == "CLEAR" || command == "C")
			{
				analyzer = gcnew DataAnalyzer(100);
				Console::WriteLine("Data Analyzer Cleared.");
			}
			else if (command == "DEFAULT" || command == "RESET" || command == "R")
			{
				analyzer = gcnew DataAnalyzer(100);
				Console::WriteLine("Data Analyzer Cleared.");
			}
			else if (command == "ADDDEFAULT" || command == "AD" || command == "ADDSAMPLE" || command == "AS")
			{
				AddSampleSegments(analyzer);
				Console::WriteLine("Sample Segments Added.");
			}
			else if (command == "MAXELEVATION" || command == "ME")
			{
				Console::WriteLine("Maximum Elevation = {0}", analyzer->GetMaxElevation());
			}
			//Hiking
			
			else if (command == "NUMHIKERESTS" || command == "NHR")
			{
				Console::WriteLine("Number of Hiking Rests = {0}", analyzer->GetNumberHikingRests());
			}
			else if (command == "HIKERESTTIME" || command == "HRT")
			{
				Console::WriteLine("Hiking Rest Time = {0}", analyzer->GetHikingRestTime());
			}
			else if (command == "HIKESPEED" || command == "HS")
			{
				Console::WriteLine("Average Hiking Speed = {0}", analyzer->GetHikingSpeed());
			}
			else if (command == "AVERAGEUPSPEED" || command == "AUS")
			{
				Console::WriteLine("Average Hiking Speed = {0}", analyzer->GetAverageUpSpeed());
			}
			else if (command == "AVERAGEDOWNSPEED" || command == "ADS")
			{
				Console::WriteLine("Average Hiking Speed = {0}", analyzer->GetAverageDownSpeed());
			}
			//Skiing/Snowboarding
			else if (command == "NUMRUNS" || command == "NR")		
			{
				Console::WriteLine("Number of Runs = {0}", analyzer->GetNumberRuns());
			}
			else if (command == "GETAVGLIFTSPEED" || command == "ALS")
			{
				Console::WriteLine("Average Lift Speed = {0}", analyzer->GetAverageLiftSpeed());
			}
			else if (command == "AVGSKISPEED" || command == "ASS")
			{
				Console::WriteLine("Average Ski Speed = {0}", analyzer->GetAverageSkiSpeed());
			}
			else if (command == "GETAVERAGELIFTWAIT" || command == "ALW")		
			{
				Console::WriteLine("Average Lift Wait Time = {0}", analyzer->GetAverageLiftWaitTime());
			}
			else if (command == "TOTALWAITTIME" || command == "TWT")
			{
				Console::WriteLine("Total Lift Wait TIME = {0}", analyzer->GetTotalLiftWaitTime());
			}
			else if (command == "GETAVERAGELIFT" || command == "ALT")	//	
			{
				Console::WriteLine("Average Lift TIME = {0}", analyzer->GetAverageLiftTime());
			}
			else if (command == "GETTOTALLIFT" || command == "TLT")		//	
			{
				Console::WriteLine("TOTAL Lift TIME = {0}", analyzer->GetTotalLiftTime());
			}
			else if (command == "GETAVGRUNTIME" || command == "ART")
			{
				Console::WriteLine("Average Run TIME = {0}", analyzer->GetAverageRunTime());
			}
			else if (command == "TOTALSKITIME" || command == "TST")			//no ski time
			{
				Console::WriteLine("Total Ski Time = {0}", analyzer->GetTotalSkiTime());
			}
			else if (command == "AVERAGEBINDINGTIME" || command == "ABT")	//	
			{
				Console::WriteLine("Average binding Time = {0}", analyzer->GetAverageBindingTime());
			}
			else if (command == "TTLBINDTIME" || command == "TBT")    //
			{
				Console::WriteLine("Total binding Time = {0}", analyzer->GetTotalBindingTime());
			}
			else if (command == "SKIDISTANCE" || command == "SD")				
			{
				Console::WriteLine("Total Ski Distance = {0}", analyzer->GetSkiDistance());
			}
			//Snowmachine, Car, etc.
			else if (command == "NUMBERSTOPS" || command == "NS")				
			{
				Console::WriteLine("Total number of Stops = {0}", analyzer->GetNumberStops());
			}
			else if (command == "MAXACCELERATION" || command == "MA")				
			{
				Console::WriteLine("Maximum Acceleration = {0}", analyzer->GetMaximumAcceleration());
			}
			else if (command == "MAXDECELERATION" || command == "MD")				
			{
				Console::WriteLine("Maximum Deceleration = {0}", analyzer->GetMaximumDeceleration());
			}
			else if (command == "VEHICLERESTTIME" || command == "VRT")				
			{
				Console::WriteLine("Total Rest Time = {0}", analyzer->GetVehicleRestTime());
			}
			else if (command == "ACCELERATINGTIME" || command == "AT")				
			{
				Console::WriteLine("Total Time While Accelerating = {0}", analyzer->GetAcceleratingTime());
			}
			else if (command == "DECELERATINGTIME" || command == "DT")				
			{
				Console::WriteLine("Total Time While Decelerating= {0}", analyzer->GetDeceleratingTime());
			}
			//Lists
			else if (command == "LIST" || command == "L")
			{
				Console::WriteLine(analyzer->PrintList());
			}
			else if (command == "LISTD" || command == "LD")
			{
				Console::WriteLine(analyzer->GetDistances());
			}
			else if (command == "LISTV" || command == "LV")
			{
				Console::WriteLine(analyzer->GetVelocities());
			}
			else if (command == "LISTT" || command == "LT")
			{
				Console::WriteLine(analyzer->GetTimes());
			}
			else if (command == "HELP" || command == "H" || command == "?")
			{
				PrintHelp();
			}

			Console::WriteLine();
		}
		catch (Exception^ e)
		{
			Console::WriteLine("Error: {0}" + e->Message);
			Console::WriteLine();
		}
	}

    Console::WriteLine();
    Console::WriteLine("Application ended. Press any key to continue...");
    Console::ReadKey(true);
}

#pragma endregion

#pragma region DataAnalyzer Testing

static void AddSampleSegments(DataAnalyzer^ pDA)
{
	//SampleSegments(Distance, Time, Course, Vertical, Horizontal)
    pDA->AddSegment(393.93, 8, 205.98, 9.61, 393.81);
    pDA->AddSegment(495.3, 10, 199.94, 10.09, 495.20);
    pDA->AddSegment(514.3, 10, 192.42, 4.33, 514.28);
    pDA->AddSegment(468.76, 9, 188.40, 3.85, -50.87);
    pDA->AddSegment(2, 10, 188.40, -1, 1);
    pDA->AddSegment(468.76, 9, 188.40, -3.85, 468.74);
    pDA->AddSegment(253.65, 5, 164.79, -5.77, 253.58);
    pDA->AddSegment(253.65, 5, 164.79, -5.77, 253.58);
    pDA->AddSegment(253.65, 5, 164.79, -5.77, 253.58);
    pDA->AddSegment(253.65, 5, 164.79, -5.77, 253.58);
    pDA->AddSegment(200.45, 4, 139.13, 4.33, 200.41);
    pDA->AddSegment(1, 10, 188.40, 1, 1);
    pDA->AddSegment(1, 10, 188.40, -2.7, 1);
    pDA->AddSegment(1, 10, 188.40, -1, 1);
    pDA->AddSegment(1, 10, 188.40, 1, 1);
    pDA->AddSegment(203.53, 4, 117.75, 2.40, 203.52);
    pDA->AddSegment(155.14, 3, 100.85, -0.96, 155.13);
    pDA->AddSegment(355.56, 7, 86.89, 4.81, 355.53);
    pDA->AddSegment(242.13, 5, 70.93, 11.54, 241.85);
}

static void PrintHelp()
{
    Console::WriteLine("  --- Commands ---");
    Console::WriteLine("CLS (C)			  - Clears the Screen");
    Console::WriteLine("Reset (R)		  - Resets the DataAnalyzer to default sample segments");
    Console::WriteLine("Clear (C)		  - Clears the DataAnalyzer");
    Console::WriteLine("AddSample (AS)	  - Adds the default sample segments");
    Console::WriteLine();
    Console::WriteLine("Add (A)           - Adds a segment: <Distance> <Time> <Course> <Vertical> <Flat>");
    Console::WriteLine("                      Example: Add 393.93 8 180 9.61 393.81");
    Console::WriteLine("MaxElevation (ME) - Displays the Max Elevation");
    Console::WriteLine();
    Console::WriteLine("Exit (E)		  - Quits the Application");
    Console::WriteLine("Quit (Q)		  - Quits the Application");
}

#pragma endregion