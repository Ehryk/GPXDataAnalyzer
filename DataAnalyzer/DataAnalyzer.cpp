// This is the main DLL file.

#include "stdafx.h"
#include <Windows.h>
#include "DataAnalyzer.h"

BOOL WINAPI  DllMain (HANDLE hModule, DWORD dwFunction, LPVOID lpNot)
{
    switch (dwFunction)
    {
        case DLL_PROCESS_ATTACH:
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
        case DLL_PROCESS_DETACH:
            break;
    }

    return TRUE;
}


DataAnalyzer::DataAnalyzer(int segments)
{
	i = 0;
	n = segments;

	distances = new double[segments];
	times = new double[segments];
	courses = new double[segments];

	velocities = new double[segments];
}

bool DataAnalyzer::AddSegment(double distance, double time, double course, double vertical, double velocity)
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
