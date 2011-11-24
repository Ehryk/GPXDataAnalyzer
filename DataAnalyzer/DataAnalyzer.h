// DataAnalyzer.h

#pragma once

using namespace std;

struct Result
{
	public:
	//Result(string type, string detail, double min, double avg, double max);
	
	double Minimum;
	double Average;
	double Maximum;
	double Total;
	string Type;
	string Detail;
};

class DataAnalyzer
{
	//public:
	//Result TotalDistance(trkpt[] trackPoints);

	////Velocity
	//Result Velocity(trkpt[] trackPoints);
	//Result VerticalVelocity(trkpt[] trackPoints);
	//Result FlatEarthVelocity(trkpt[] trackPoints);

	////Acceleration
	//Result Acceleration(trkpt[] trackPoints);
	//Result VerticalAcceleration(trkpt[] trackPoints);
	//Result FlatEarthAcceleration(trkpt[] trackPoints);
};
