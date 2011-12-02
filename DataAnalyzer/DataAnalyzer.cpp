// This is the main DLL file.

#include "stdafx.h"

#include "DataAnalyzer.h"

#pragma region Ehryk

DataAnalyzer::DataAnalyzer(int segments)
{
	n = 0;
	max = segments;

	double startElevation = 0;
	double startLatitude = 0;
	double startLongitude = 0;
	DateTime startDateTime = DateTime::MinValue;

	distances = new double[segments];
	flatDistances = new double[segments];
	verticalDistances = new double[segments];

	times = new double[segments];
	courses = new double[segments];

	velocities = new double[segments];
	flatVelocities = new double[segments];
	verticalVelocities = new double[segments];
}

DataAnalyzer::DataAnalyzer(int segments, double pStartElevation)
{
	n = 0;
	max = segments;

	double startElevation = pStartElevation;
	DateTime startDateTime = DateTime::MinValue;
	double startLatitude = 0;
	double startLongitude = 0;

	distances = new double[segments];
	flatDistances = new double[segments];
	verticalDistances = new double[segments];

	times = new double[segments];
	courses = new double[segments];

	velocities = new double[segments];
	flatVelocities = new double[segments];
	verticalVelocities = new double[segments];
}

DataAnalyzer::DataAnalyzer(int segments, double pStartElevation, DateTime pStartDateTime)
{
	n = 0;
	max = segments;

	double startElevation = pStartElevation;
	DateTime startDateTime = pStartDateTime;
	double startLatitude = 0;
	double startLongitude = 0;

	distances = new double[segments];
	flatDistances = new double[segments];
	verticalDistances = new double[segments];

	times = new double[segments];
	courses = new double[segments];

	velocities = new double[segments];
	flatVelocities = new double[segments];
	verticalVelocities = new double[segments];
}

DataAnalyzer::DataAnalyzer(int segments, double pStartElevation, DateTime pStartDateTime, double pStartLatitude, double pStartLongitude)
{
	n = 0;
	max = segments;

	double startElevation = pStartElevation;
	DateTime startDateTime = pStartDateTime;
	double startLatitude = pStartLatitude;
	double startLongitude = pStartLongitude;

	distances = new double[segments];
	flatDistances = new double[segments];
	verticalDistances = new double[segments];

	times = new double[segments];
	courses = new double[segments];

	velocities = new double[segments];
	flatVelocities = new double[segments];
	verticalVelocities = new double[segments];
}


bool DataAnalyzer::AddSegment(double distance, double time, double course, double vertical, double flat)
{
	if (n >= max)
		return false;
	
	distances[n] = distance;
	flatDistances[n] = flat;
	verticalDistances[n] = vertical;

	times[n] = time;
	courses[n] = course;

	//Compute each velocity as the segment is added
	velocities[n] = Velocity(distance, time);
	flatVelocities[n] = Velocity(flat, time);
	verticalVelocities[n] = Velocity(vertical, time);

	//Add Segment values to running total
	endElevation += vertical;
	endDateTime.AddSeconds(time);

	n++;

	return true;
}

#pragma endregion

#pragma region External

//Common
double DataAnalyzer::GetAverageDistance()
{
	double total = GetTotalDistance();

	double average = total / n;

	return average;
}

double DataAnalyzer::GetTotalDistance()
{
	double total = 0;

	for (i = 0; i < n; i++)
		total += distances[i];

	return total;
}

double DataAnalyzer::GetAverageTime()
{
	double total = GetTotalTime();

	double average = total / n;

	return average;
}

double DataAnalyzer::GetTotalTime()
{
	double total = 0;

	for (i = 0; i < n; i++)
		total += times[i];

	return total;
}

double DataAnalyzer::GetAverageVelocity()
{
	double total = 0;

	for (i = 0; i < n; i++)
		total += velocities[i];

	double average = total / n;

	return average;
}

double DataAnalyzer::GetMaxVelocity()
{
	double max = velocities[0];

	for (i = 1; i < n; i++)
		if (max < velocities[i])
			max = velocities[i];

	return max;
}

double DataAnalyzer::GetMinElevation()
{
	return -1;
}

double DataAnalyzer::GetMaxElevation()
{
	return -1;
}

	
//Hiking / Jogging
int DataAnalyzer::GetNumberHikingRests()
{
	return -1;
}

double DataAnalyzer::GetHikingRestTime()
{
	return -1;
}

double DataAnalyzer::GetHikingSpeed()
{
	return -1;
}

double DataAnalyzer::GetAverageUpSpeed()
{
	return -1;
}

double DataAnalyzer::GetAverageDownSpeed()
{
	return -1;
}

	
//Skiing
int DataAnalyzer::GetNumberRuns()
{
	return -1;
}

int DataAnalyzer::GetNumberFalls()
{
	return -1;
}

double DataAnalyzer::GetAverageLiftSpeed()
{
	return -1;
}

double DataAnalyzer::GetAverageSkiSpeed()
{
	return -1;
}

double DataAnalyzer::GetAverageLiftWaitTime()
{
	return -1;
}

double DataAnalyzer::GetTotalLiftWaitTime()
{
	return -1;
}

double DataAnalyzer::GetAverageLiftTime()
{
	return -1;
}

double DataAnalyzer::GetTotalLiftTime()
{
	return -1;
}

double DataAnalyzer::GetAverageRunTime()
{
	return -1;
}

double DataAnalyzer::GetTotalSkiTime()
{
	return -1;
}

double DataAnalyzer::GetAverageBindingTime()
{
	return -1;
}

double DataAnalyzer::GetTotalBindingTime()
{
	return -1;
}

double DataAnalyzer::GetSkiDistance()
{
	return -1;
}


//Snowmobile / Car
int DataAnalyzer::GetNumberStops()
{
	return -1;
}

double DataAnalyzer::GetMaximumAcceleration()
{
	return -1;
}

double DataAnalyzer::GetMaximumDeceleration()
{
	return -1;
}

double DataAnalyzer::GetStoppedTime()
{
	return -1;
}

double DataAnalyzer::GetCoastTime()
{
	return -1;
}

double DataAnalyzer::GetAcceleratingTime()
{
	return -1;
}

double DataAnalyzer::GetDeceleratingTime()
{
	return -1;
}

#pragma endregion

#pragma region Internal

double DataAnalyzer::Velocity(double distance, double time)
{
	double velocity = distance / time;
	return velocity;
}

double DataAnalyzer::Acceleration(double distance1, double time1, double distance2, double time2)
{
	double velocity1 = Velocity(distance1, time1);
	double velocity2 = Velocity(distance2, time2);

	double acceleration = (velocity2 - velocity1) / time2;
	return acceleration;
}

double DataAnalyzer::Acceleration(double velocity1, double velocity2, double time)
{
	double acceleration = (velocity2 - velocity1) / time;
	return acceleration;
}

#pragma endregion

#pragma region Testing

int DataAnalyzer::GetI() {return i;}
int DataAnalyzer::GetN() {return n;}
int DataAnalyzer::GetMax() {return max;}
	
double DataAnalyzer::GetStartElevation() {return startElevation;}
double DataAnalyzer::GetStartLatitude() {return startLatitude;}
double DataAnalyzer::GetStartLongitude() {return startLongitude;}
DateTime DataAnalyzer::GetStartDateTime() {return startDateTime;}
	
double DataAnalyzer::GetEndElevation() {return endElevation;}
double DataAnalyzer::GetEndLatitude() {return endLatitude;}
double DataAnalyzer::GetEndLongitude() {return endLongitude;}
DateTime DataAnalyzer::GetEndDateTime() {return endDateTime;}

String^ DataAnalyzer::GetDistances()
{
	String^ str = "";
	for (i = 0; i < n; i ++)
	{
		str += distances[i] + "\n";
	}
	return str;
}

String^ DataAnalyzer::GetFlatDistances()
{
	String^ str = "";
	for (i = 0; i < n; i ++)
	{
		str += flatDistances[i] + "\n";
	}
	return str;
}

String^ DataAnalyzer::GetVerticalDistances()
{
	String^ str = "";
	for (i = 0; i < n; i ++)
	{
		str += verticalDistances[i] + "\n";
	}
	return str;
}


String^ DataAnalyzer::GetVelocities()
{
	String^ str = "";
	for (i = 0; i < n; i ++)
	{
		str += velocities[i] + "\n";
	}
	return str;
}

String^ DataAnalyzer::GetFlatVelocities()
{
	String^ str = "";
	for (i = 0; i < n; i ++)
	{
		str += flatVelocities[i] + "\n";
	}
	return str;
}

String^ DataAnalyzer::GetVerticalVelocities()
{
	String^ str = "";
	for (i = 0; i < n; i ++)
	{
		str += verticalVelocities[i] + "\n";
	}
	return str;
}


String^ DataAnalyzer::GetTimes()
{
	String^ str = "";
	for (i = 0; i < n; i ++)
	{
		str += times[i] + "\n";
	}
	return str;
}

String^ DataAnalyzer::GetCourses()
{
	String^ str = "";
	for (i = 0; i < n; i ++)
	{
		str += courses[i] + "\n";
	}
	return str;
}

#pragma endregion