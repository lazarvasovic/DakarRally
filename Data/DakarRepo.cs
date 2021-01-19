using DakarRally.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DakarRally.Dtos;

namespace DakarRally.Data
{
    public class DakarRepo : IDakarRepo
    {
        private readonly DakarDbContext _dakarDbContext;

        public DakarRepo(DakarDbContext dakarDbContext)
        {
            _dakarDbContext = dakarDbContext;
        }

        public void CreateRace(RaceModel race)
        {
            _dakarDbContext.Races.Add(race);
        }

        public VehicleSubtypeModel GetVehicleSubtype(string vehicleType, string vehicleSubtype)
        {
            return _dakarDbContext.VehicleSubtypes.Where(x => x.SubtypeName == vehicleSubtype)
                                                  .Include(x => x.VehicleType)
                                                  .Where(x => x.VehicleType.TypeName == vehicleType)
                                                  .FirstOrDefault();
        }

        public bool AddVehicleToRace(int raceId, VehicleModel vehicle, out string status)
        {
            RaceModel race = _dakarDbContext.Races.FirstOrDefault(x => x.Id == raceId);

            if (race != null && race.Status == RaceStatus.Pending)
            {
                vehicle.Race = race;
                _dakarDbContext.Vehicles.Add(vehicle);

                status = "Success.";
                return true;
            }

            if (race != null)
            {
                status = "Race is already started!";
                return false;
            }

            status = $"No race with identifier {raceId} found!";
            return false;
        }

        public VehicleModel GetVehicle(int id)
        {
            VehicleModel vehicle = _dakarDbContext.Vehicles.Include(x => x.Race).Where(x => x.Id == id).FirstOrDefault();

            return vehicle;
        }

        public VehicleModel UpdateVehicle(int vehicleId, out string status)
        {
            VehicleModel vehicleToUpdate = GetVehicle(vehicleId);
            if (vehicleToUpdate != null)
            {
                if (_dakarDbContext.Races.FirstOrDefault(x => x.Id == vehicleToUpdate.Race.Id &&
                    x.Status == RaceStatus.Pending) != null)
                {
                    status = "Success.";
                    return vehicleToUpdate;
                }

                status = "You can update vehicle only prior to the race start.";
                return null;
            }

            status = $"No vehicle with identifier {vehicleId} found!";
            return null;
        }

        public bool RemoveVehicle(int id, out string status)
        {
            VehicleModel vehicleToDelete = GetVehicle(id);

            if (vehicleToDelete != null)
            {
                if (_dakarDbContext.Races.FirstOrDefault(x => x.Id == vehicleToDelete.Race.Id &&
                    x.Status == RaceStatus.Pending) != null)
                {
                    _dakarDbContext.Vehicles.Remove(vehicleToDelete);
                    status = "Success.";
                    return true;
                }

                status = "You can remove vehicle only prior to the race start.";
                return false;
            }

            status = $"No vehicle with identifier {id} found!";
            return false;
        }

        public bool StartRace(int raceId, out string status)
        {
            RaceModel race = _dakarDbContext.Races.FirstOrDefault(x => x.Id == raceId);
            if (race != null)
            {
                if (!_dakarDbContext.Races.Any(x => x.Status == RaceStatus.Running))
                {
                    if (GetVehiclesFromRace(raceId).Count > 0)
                    {
                        if(race.Status == RaceStatus.Pending)
                        {
                            race.Status = RaceStatus.Running;
                            status = "Success.";
                            return true;
                        }

                        status = "Race is finished!";
                        return false;
                    }

                    status = "No vehicles in race!";
                    return false;
                }

                status = "One race is already started!";
                return false;
            }

            status = $"No race with identifier {raceId} found!";
            return false;
        }

        public List<VehicleModel> GetAllVehiclesLeaderboard()
        {
            var vehicles = _dakarDbContext.Vehicles.Include(x => x.Race).Include(x => x.VehicleSubtype)
                .Include(x => x.VehicleSubtype.VehicleType).Where(x => x.Race.Status == RaceStatus.Running)
                .OrderByDescending(x => x.DistanceReached).ThenBy(x => x.FinishTime);

            return vehicles.ToList();
        }

        public List<VehicleModel> GetLeaderboardByType(string type)
        {
            var vehicles = _dakarDbContext.Vehicles.Include(x => x.Race)
                .Include(x => x.VehicleSubtype).Include(x => x.VehicleSubtype.VehicleType)
                .Where(x => x.Race.Status == RaceStatus.Running && x.VehicleSubtype.VehicleType.TypeName == type)
                .OrderByDescending(x => x.DistanceReached).ThenBy(x => x.FinishTime);

            return vehicles.ToList();
        }

        public List<MalfunctionStatisticModel> GetVehicleMalfunctions(int id)
        {
            var malfunctions = _dakarDbContext.MalfunctionStatistics
                .Include(x => x.Vehicle).Where(x => x.Vehicle.Id == id);

            return malfunctions.ToList();
        }

        public List<VehicleModel> FindVehicles(string team = null, string model = null,
            DateTime? manufacturingDate = null, string status = null, double? distance = null, string sortOrder = null)
        {
            var vehicles = _dakarDbContext.Vehicles.Include(x => x.VehicleSubtype)
                .Include(x => x.VehicleSubtype.VehicleType).Where(x => 
                (team == null || x.TeamName == team) &&
                (model == null || x.Model == model) &&
                (manufacturingDate == null || x.ManufacturingDate == manufacturingDate.Value) &&
                
                (distance == null || x.DistanceReached == distance.Value)
            );

            try
            {
                VehicleStatus s = (VehicleStatus)Enum.Parse(typeof(VehicleStatus), status);
                vehicles = vehicles.Where(x => x.VehicleStatus == s);
            }
            catch
            {
                vehicles = vehicles.Where(x => x.VehicleStatus == null);
            }

            if (sortOrder == "asc")
            {
                vehicles = vehicles.OrderBy(x => x.TeamName)
                    .ThenBy(x => x.Model)
                    .ThenBy(x => x.ManufacturingDate)
                    .ThenBy(x => x.VehicleStatus)
                    .ThenBy(x => x.DistanceReached);
            }
            else if (sortOrder == "desc")
            {
                vehicles = vehicles.OrderByDescending(x => x.TeamName)
                    .ThenByDescending(x => x.Model)
                    .ThenByDescending(x => x.ManufacturingDate)
                    .ThenByDescending(x => x.VehicleStatus)
                    .ThenByDescending(x => x.DistanceReached);
            }

            return vehicles.ToList();
        }

        public List<VehiclesByStatusDto> GetVehiclesGroupedByStatus(int raceId)
        {
            var vehicles = _dakarDbContext.Vehicles.Include(x => x.Race).Where(x => x.Race.Id == raceId)
                .GroupBy(x => x.VehicleStatus)
                .Select(group => new VehiclesByStatusDto
                {
                    VehicleStatus = group.Key.ToString(),
                    Count = group.Count()
                });

            if (vehicles == null)
            {
                return null;
            }

            return vehicles.ToList();
        }

        public List<VehiclesByTypeDto> GetVehiclesGroupedByType(int raceId)
        {
            var vehicles = _dakarDbContext.Vehicles.Include(x => x.Race).Where(x => x.Race.Id == raceId)
                .Include(x => x.VehicleSubtype).Include(x => x.VehicleSubtype.VehicleType)
                .GroupBy(x => x.VehicleSubtype.VehicleType.TypeName)
                .Select(group => new VehiclesByTypeDto
                {
                    VehicleType = group.Key,
                    Count = group.Count()
                });

            if (vehicles == null)
            {
                return null;
            }

            return vehicles.ToList();
        }

        public RaceModel GetRace(int raceId)
        {
            RaceModel race = _dakarDbContext.Races.FirstOrDefault(x => x.Id == raceId);

            return race;
        }

        public List<VehicleModel> GetVehiclesFromRace(int raceId)
        {
            var vehicles = _dakarDbContext.Vehicles.Include(x => x.Race).Where(x => x.Race.Id == raceId)
                .Include(x => x.VehicleSubtype).Include(x => x.VehicleSubtype.VehicleType);

            return vehicles.ToList();
        }

        public void AddMalfunctionStatistic(MalfunctionStatisticModel malfunctionStatistic)
        {
            _dakarDbContext.MalfunctionStatistics.Add(malfunctionStatistic);
        }

        public void FinishRace()
        {
            _dakarDbContext.Races.FirstOrDefault(x => x.Status == RaceStatus.Running).Status = RaceStatus.Finished;
        }

        public bool SaveChanges()
        {
            return _dakarDbContext.SaveChanges() > 0;
        }


    }
}
