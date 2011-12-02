// DataAnalyzer.h

#pragma once

using namespace System;

public ref class DataAnalyzer
{
private:
	int i;
	int n;
	int max;

	double startElevation;
	double startLatitude;
	double startLongitude;
	DateTime startDateTime;

	double endElevation;
	double endLatitude;
	double endLongitude;
	DateTime endDateTime;

	double* distances;
	double* flatDistances;
	double* verticalDistances;

	double* velocities;
	double* flatVelocities;
	double* verticalVelocities;

	double* times;
	double* courses;

public:
	DataAnalyzer(int segments);
	DataAnalyzer(int segments, double pStartElevation);
	DataAnalyzer(int segments, double pStartElevation, DateTime pStartDateTime);
	DataAnalyzer(int segments, double pStartElevation, DateTime pStartDateTime, double pStartLatitude, double pStartLongitude);

	bool AddSegment(double distance, double time, double course, double vertical, double flat);

	//Common
	double GetAverageDistance();
	double GetTotalDistance();
	double GetAverageTime();
	double GetTotalTime();
	double GetAverageVelocity();
	double GetMaxVelocity();
	double GetMinElevation();
	double GetMaxElevation();
	
	//Hiking / Jogging
	int GetNumberHikingRests();
	double GetHikingRestTime();
	double GetHikingSpeed();
	double GetAverageUpSpeed();
	double GetAverageDownSpeed();
	
	//Skiing
	int GetNumberRuns();
	int GetNumberFalls();
	double GetAverageLiftSpeed();
	double GetAverageSkiSpeed();
	double GetAverageLiftWaitTime();
	double GetTotalLiftWaitTime();
	double GetAverageLiftTime();
	double GetTotalLiftTime();
	double GetAverageRunTime();
	double GetTotalSkiTime();
	double GetAverageBindingTime();
	double GetTotalBindingTime();
	double GetSkiDistance();

	//Snowmobile / Car
	int GetNumberStops();
	double GetMaximumAcceleration();
	double GetMaximumDeceleration();
	double GetStoppedTime();
	double GetCoastTime();
	double GetAcceleratingTime();
	double GetDeceleratingTime();

	//Internal
	double Velocity(double distance, double time);
	double Acceleration(double distance1, double time1, double distance2, double time2);
	double Acceleration(double velocity1, double velocity2, double time);

	//Private (for testing)
	int GetI();
	int GetN();
	int GetMax();
	
	double GetStartElevation();
	double GetStartLatitude();
	double GetStartLongitude();
	DateTime GetStartDateTime();
	
	double GetEndElevation();
	double GetEndLatitude();
	double GetEndLongitude();
	DateTime GetEndDateTime();

	String^ GetDistances();
	String^ GetFlatDistances();
	String^ GetVerticalDistances();

	String^ GetVelocities();
	String^ GetFlatVelocities();
	String^ GetVerticalVelocities();

	String^ GetTimes();
	String^ GetCourses();
};
