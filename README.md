# DakarRally

## Web API

+ ### Create race
***Route***: /api/race/create/{year}  
***Request***: POST

+ ### Add vehicle to race
***Route***: /api/race/addVehicle/{raceId}  
***Request***: POST  
***Parameters***:
{
    "teamName": "team",
    "model": "VW",
    "manufacturingDate": "2020-01-11",
    "vehicleType": "Car",
    "vehicleSubtype": "Sports"
}  
***Note***: "vehicleType" can be *Car*, *Truck* or *Motorcycle*. When "vehicleType" is *Car* then "vehicleSubtype" can be *Terrain* or *Sports*. When "vehicleType" is *Motorcycle* then "vehicleSubtype" can be *Sport* or *Cross*. When vehicle type is *Truck* then "vehicleSubtype" attribute needs to be removed.

+ ### Update vehicle
***Route***: /api/race/updateVehicle  
***Request***: PUT  
***Parameters***:
{
    "id": 1,
    "teamName": "team",
    "model": "VW",
    "manufacturingDate": "2020-01-11",
    "vehicleType": "Motorcycle",
    "vehicleSubtype": "Sport"
}  
***Note***: "vehicleType" can be *Car*, *Truck* or *Motorcycle*. When "vehicleType" is *Car* then "vehicleSubtype" can be *Terrain* or *Sports*. When "vehicleType" is *Motorcycle* then "vehicleSubtype" can be *Sport* or *Cross*. When vehicle type is *Truck* then "vehicleSubtype" attribute needs to be removed.

+ ### Remove vehicle from race
***Route***: /api/race/startRace/{raceId}  
***Request***: DELETE  


+ ### Start the race
***Route***: /api/race/removeVehicle/{vehicleId}  
***Request***: POST  

+ ### Get leaderboard including all vehicles
***Route***: /api/race/getLeaderboard  
***Request***: GET  

+ ### Get leaderboard for specific vehicle type
***Route***: /api/race/getLeaderboard/{typeName}  
***Request***: GET  
***Note***: typeName can be *Car, Motorcycle* or *Truck*

+ ### Get vehicle statistics
***Route***: /api/race/getVehicleStatistic/{vehicleId}  
***Request***: GET  


+ ### Find vehicle(s)
***Route***: /api/race/findVehicles?{team?}&{model?}&{manufacturingDate?}&{status?}&{distance?}&{sortOrder?}  
***Request***: GET  
***Note***: manufacturingDate is in format 'YYYY-MM-DD'; status can be *Pending, Running, Repairing, Wracked, Finished*; sortOrder can be *asc* or *desc*

+ ### Get vehicle statistics
***Route***: /api/race/GetRaceStatus/{raceId}  
***Request***: GET  
