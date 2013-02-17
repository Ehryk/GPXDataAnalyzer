// This is the main DLL file.

#include "stdafx.h"

#include "Analyzer.h"

Analyzer::Analyzer(int segments)
{
	i = 0;
	n = segments;

	distances = new double[segments];
	times = new double[segments];
	courses = new double[segments];

	velocities = new double[segments];
}

bool Analyzer::AddSegment(double distance, double time, double course, double vertical, double velocity)
{
	if (i >= n)
		return false;
	
	distances[i] = distance;
	times[i] = time;
	courses[i] = course;
	verticals[i] = vertical;
	velocities[i] = velocity;

	i++;

	return true;
}

//Common
double Analyzer::GetAverageDistance()
{
	double total = GetTotalDistance();

	double average = total / n;

	return average;
}

double Analyzer::GetTotalDistance()
{
	double total = 0;

	for (i = 0; i < n; i++)
		total += distances[i];

	return total;
}

double Analyzer::GetAverageTime()
{
	double total = GetTotalTime();

	double average = total / n;

	return average;
}

double Analyzer::GetTotalTime()
{
	double total = 0;

	for (i = 0; i < n; i++)
		total += times[i];

	return total;
}

double Analyzer::GetAverageVelocity()
{
	double total = 0;

	for (i = 0; i < n; i++)
		total += velocities[i];

	double average = total / n;

	return average;
}

double Analyzer::GetMaxVelocity()
{
	double max = velocities[0];

	for (i = 1; i < n; i++)
		if (max < velocities[i])
			max = velocities[i];

	return max;
}

double Analyzer::GetMinElevation()
{
	return -1;
}

double Analyzer::GetMaxElevation()
{
	return -1;
}

	
//Hiking / Jogging
int Analyzer::GetNumberHikingRests()
{
	return -1;
}

double Analyzer::GetHikingRestTime()
{
	return -1;
}

double Analyzer::GetHikingSpeed()
{
	return -1;
}

double Analyzer::GetAverageUpSpeed()
{
	return -1;
}

double Analyzer::GetAverageDownSpeed()
{
	return -1;
}

	
//Skiing
int Analyzer::GetNumberRuns()
{
	return -1;
}

int Analyzer::GetNumberFalls()
{
	return -1;
}

double Analyzer::GetAverageLiftSpeed()
{
	return -1;
}

double Analyzer::GetAverageSkiSpeed()
{
	return -1;
}

double Analyzer::GetAverageLiftWaitTime()
{
	return -1;
}

double Analyzer::GetTotalLiftWaitTime()
{
	return -1;
}

double Analyzer::GetAverageLiftTime()
{
	return -1;
}

double Analyzer::GetTotalLiftTime()
{
	return -1;
}

double Analyzer::GetAverageRunTime()
{
	return -1;
}

double Analyzer::GetTotalSkiTime()
{
	return -1;
}

double Analyzer::GetAverageBindingTime()
{
	return -1;
}

double Analyzer::GetTotalBindingTime()
{
	return -1;
}

double Analyzer::GetSkiDistance()
{
	return -1;
}


//Snowmobile / Car
int Analyzer::GetNumberStops()
{
	return -1;
}

double Analyzer::GetMaximumAcceleration()
{
	return -1;
}

double Analyzer::GetMaximumDeceleration()
{
	return -1;
}

double Analyzer::GetStoppedTime()
{
	return -1;
}

double Analyzer::GetCoastTime()
{
	return -1;
}

double Analyzer::GetAcceleratingTime()
{
	return -1;
}

double Analyzer::GetDeceleratingTime()
{
	return -1;
}
