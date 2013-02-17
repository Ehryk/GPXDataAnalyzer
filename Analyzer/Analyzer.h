// Analyzer.h

#pragma once

using namespace System;

	public ref class Analyzer
	{
	private:
		int i;
		int n;
		double* distances;
		double* verticals;
		double* velocities;
		double* times;
		double* courses;

	public:
		Analyzer(int segments);
		bool AddSegment(double distance, double time, double course, double vertical, double velocity);

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
	};
