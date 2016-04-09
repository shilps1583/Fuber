Build the solution
Run Fuber\APIHost\src\APIHost\bin\Debug as an Administrator
Test using Postman:
To create a booking, make a POST request to http://localhost:9000/bookings with the following schema

{
    "UserId" : "shilpa.kundapur@gmail.com",
    "PickupLocation" : { "Latitude" : "12.97721", "Longitude" : "77.60205"},
    "Destination" : { "Latitude" : "12.943", "Longitude" : "77.57407"},
    "Time" : "2016-04-07T13:27:25.277Z",
    "CabType" : "Pink"
}
To view all bookings, make a GET request to http://localhost:9000/bookings

To view all cabs, make a GET request to http://localhost:9000/cabs

To start a trip, make a POST request to http://localhost:9000/trips with the following schema
{
    "BookingId" : "30ebd6ab-51a6-4cf1-92d8-2450677b4734",
    "StartLocation" : { "Latitude" : "12.97721", "Longitude" : "77.60205"},
    "StartTime" : "2016-04-07T15:27:25.277Z"
}

To end a trip, make a POST request to http://localhost:9000/trips/{tripId} with the following schema
{    
    "EndLocation" : { "Latitude" : "12.97721", "Longitude" : "77.60205"},
    "EndTime" : "2016-04-07T15:27:25.277Z"
}

P.S: The repositories are in memory, so data created will be lost if the program is stopped. 
You can start the program as a windows service by using "APIHost.exe install" and "APIHost.exe start"