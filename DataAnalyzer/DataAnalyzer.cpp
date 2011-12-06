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

#pragma region External

#pragma region Common

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
	double max = 0;

	for (i = 0; i < n; i++)
		if (max < velocities[i])
			max = velocities[i];

	return max;
}

double DataAnalyzer::GetMinElevation()
{    
    double min = startElevation;

    for(i=0;i<n;i++)
	{
        if(min < verticalDistances[i])
		{
            min = verticalDistances[i];
        }
    }

    return min;
}

double DataAnalyzer::GetMaxElevation()			//I wrote
{
	double startElevation;
	
	for(i=0; i<n; i++) {
		if( max > verticalDistances[i]) {
			max = verticalDistances[i];
		}
	}
	return max;
}

#pragma endregion

#pragma region Hiking / Jogging

int DataAnalyzer::GetNumberHikingRests()
{    
	int j = 0;

	for(i=0;i<n;i++)
	{
        if(velocities[i]<.2)
		{
            j++;
        }
	}

	return j;
}

double DataAnalyzer::GetHikingRestTime() 
{
    int j=0;

    for(i=0;i<n;i++)
	{
        if(velocities[i] < .2)
		{
            j += times[i];
        }
	}

    return j;
}

double DataAnalyzer::GetHikingSpeed()			//I WROTE
{
	double speed = 0;
	int j = 0;

	for(i=0; i<n; i++) 
	{
		if (velocities[i] > .2)
		{
			speed += velocities[i];
			j ++;
		}
	}
	speed = speed/j;
	return speed;
}

double DataAnalyzer::GetAverageUpSpeed()
{
	double max = 0, min=4000, total = 0;
	int counter = 0, j = 0;

    for (i = 0; i < n; i++){
        if (max > velocities[i]){
            max=velocities[i];
            i=j;
        }
    }
    for(i=0;i<n;i++){
        if(min<velocities[i]){
            min=velocities[i];
            i=k;
        }
    }
    for(i=k;i<j;i++){
        total+=velocities[i];
        counter++;
    }
	return total/counter;
}

double DataAnalyzer::GetAverageDownSpeed()
{
	double max = 0, min=4000;
	int counter = 0, j = 0, total = 0;

    for (i = 0; i < n; i++){
        if (max > velocities[i]){
            max=velocities[i];
            i=j;
        }
    }
    for(i=0;i<n;i++){
        if(min<velocities[i]){
            min=velocities[i];
            i=k;
        }
    }
    for(i=j;i>k;i++){
        total+=velocities[i];
        counter++;
    }
	return total/counter;
}

#pragma endregion

#pragma region Skiing / Snowboarding

int DataAnalyzer::GetNumberRuns()				//I WROTE
{
	int runs = 0;
	//for(i=0; i<n; i++) {
	//	if(verticals[i] == GetMinElevation() ± 2) {
	//		runs++;
	//	}
	//}
	return runs;
}

double DataAnalyzer::GetAverageLiftSpeed()
{
	double max = 0, min=4000;
	int counter = 0, j = 0, total = 0;

    for (i = 0; i < n; i++){
        if (max > velocities[i]){
            max=velocities[i];
            i=j;
        }
    }
    for(i=0;i<n;i++){
        if(min<velocities[i]){
            min=velocities[i];
            i=k;
        }
    }
    for(i=k;i<j;i++){
        total+=velocities[i];
        counter++;
    }
	return total/counter;
}

double DataAnalyzer::GetAverageSkiSpeed()
{
	double max = 0, min=4000;
	int counter = 0, total = 0;

    for (i = 0; i < n; i++){
        if (max > velocities[i]){
            max=velocities[i];
            i=j;
        }
    }
    for(i=0;i<n;i++){
        if(min<velocities[i]){
            min=velocities[i];
            i=k;
        }
    }
    for(i=j;i>k;i++){
        total+=velocities[i];
        counter++;
    }
	return total/counter;
}

double DataAnalyzer::GetAverageLiftWaitTime()
{
	int j = 0;
	//for(i=0; i<n; i++) {
	//	while(verticalDistances[i] == GetMinElevation() ± 2) {
	//		j=0;
	//		for(i=0;i<n;i++){
	//			if(velocity<.2){
	//				j+=time[i];
	//			}
	//		}
	//	}
	//}
	return j/GetNumberRuns();
}

double DataAnalyzer::GetTotalLiftWaitTime()
{
	int j = 0;
	//for(i=0; i<n; i++) {
	//	while(verticalDistances[i] == GetMinElevation() ± 2) {
	//		j=0;
	//		for(i=0;i<n;i++){
	//			if(velocity<.2){
	//				j+=time[i];
	//			}
	//		}
	//	}
	//}
	return j;
}
	
double DataAnalyzer::GetAverageLiftTime()
{	
	double total_time;
	for(i=0; i < n - 1; i++) {
		while(verticalDistances[i+1] > verticalDistances[i]) {
			total_time = total_time + times[i+i];
		}
	}
	return total_time/GetNumberRuns();
}

double DataAnalyzer::GetTotalLiftTime()
{
	double total_time;
	for(i=0; i < n - 1; i++) {
		while(verticalDistances[i+1] > verticalDistances[i]) {
			total_time = total_time + times[i+i];
		}
	}
	return total_time;
}

double DataAnalyzer::GetAverageRunTime()
{
	double max = 0, min=4000;
	int counter = 0, total = 0;

    for (i = 0; i < n; i++){
        if (max > velocities[i]){
            max = velocities[i];
            i=j;
		}
    }
    for(i = j; i< n - 1; i++){
        while(verticalDistances[i+1] < verticalDistances[i]){
            counter++;
            total+=times[i];
        }
    }
    return total/counter;
}

double DataAnalyzer::GetTotalSkiTime()
{
	double max = 0, min=4000, total = 0;

    for (i = 0; i < n; i++){
        if (max > velocities[i]){
            max=velocities[i];
            i=j;
        }
    }
    for(i=0;i<n;i++){
        if(min<velocities[i]){
            min=velocities[i];
            i=k;
        }
    }
    for(i=j;i>k;i++){
        total+=times[i];
    }
	return total;
}

double DataAnalyzer::GetAverageBindingTime()
{
	j = 0;

	//for(i=0; i<n; i++) {
	//	while(verticals[i] == GetMaxElevation() ± 2) {
	//		j=0;
	//		for(i=0;i<n;i++){
	//			if(velocity<.2){
	//				j+=time[i]
	//			}
	//		}
	//	}
	//}
	return j/GetNumberRuns();
}

double DataAnalyzer::GetTotalBindingTime()
{
	int j = 0;

	//for(i=0; i<n; i++) {
	//	while(verticals[i] == GetMaxElevation() ± 2) {
	//		j=0;
	//		for(i=0;i<n;i++){
	//			if(velocity<.2){
	//				j+=time[i]
	//			}
	//		}
	//	}
	//}
	return j;
}

double DataAnalyzer::GetSkiDistance()
{
	double total=0;
    for(i=0;i<n;i++)
	{
		if(verticalDistances[i+1] < verticalDistances[i])
		{
			total+=distances[i];
		}
    }
    return total;
}

#pragma endregion

#pragma region Vehicle (Snowmobile/4-Wheeler/Car)

int DataAnalyzer::GetNumberStops()
{
	for(i=0;i<n;i++){
        if(velocities[i]<.2){
            j++;
        }
	}
	return j;
}

double DataAnalyzer::GetMaximumAcceleration()
{
	double acceleration = 0, max_acceleration = 0;
	for(i=0; i<n; i++)
	{
		acceleration = (velocities[i+1] - velocities[i])/times[i+1];
		if(acceleration > max_acceleration) 
		{
			max_acceleration = acceleration;
		}
	}
	return max_acceleration;
}

double DataAnalyzer::GetMaximumDeceleration()
{
	double acceleration = 0, min_acceleration = 0;
	for(i=0; i<n; i++) 
	{
		acceleration = (velocities[i+1] - velocities[i])/times[i+1];
		if(acceleration < min_acceleration) 
		{
			min_acceleration = acceleration;
		}
	}
	return min_acceleration;
}

double DataAnalyzer::GetStoppedTime()
{
	j=0;
    for(i=0;i<n;i++)
	{
        if(velocities[i] < .2)
		{
            j+=times[i];
        }
	}
    return j;
}

double DataAnalyzer::GetAcceleratingTime()
{
	double acceleration = 0, total_time = 0;
	for(i=0; i<n; i++) {
		acceleration = (velocities[i+1] - velocities[i])/times[i+1];
		if(acceleration > 0) {
			total_time += times[i+1];
		}
	}
	return total_time;
}

double DataAnalyzer::GetDeceleratingTime()
{
	double acceleration = 0, total_time = 0;
	for(i=0; i<n; i++)
	{
		acceleration = (velocities[i+1] - velocities[i])/times[i+1];
		if(acceleration < 0)
		{
			total_time += times[i+1];
		}
	}
	return total_time;
}

#pragma endregion

#pragma endregion