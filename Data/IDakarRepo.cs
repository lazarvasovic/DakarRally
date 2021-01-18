using DakarRally.Dtos;
using DakarRally.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Data
{
    public interface IDakarRepo
    {
        public void CreateRace(RaceModel race);

        public VehicleSubtypeModel GetVehicleSubtype(string vehicleType, string vehicleSubtype);

        public bool AddVehicleToRace(int raceId, VehicleModel vehicle, out string status);

        public VehicleModel GetVehicle(int id);

        public VehicleModel UpdateVehicle(int vehicleId, out string status);

        public bool RemoveVehicle(int id, out string status);

        public bool StartRace(int raceId, out string status);

        public List<VehicleModel> GetAllVehiclesLeaderboard();

        public List<VehicleModel> GetLeaderboardByType(string type);

        public List<MalfunctionStatisticModel> GetVehicleMalfunctions(int id);

        public List<VehicleModel> FindVehicles(string team = null, string model = null,
            DateTime? manufacturingDate = null, string status = null, double? distance = null, string sortOrder = null);

        public List<VehiclesByStatusDto> GetVehiclesGroupedByStatus(int raceId);

        public List<VehiclesByTypeDto> GetVehiclesGroupedByType(int raceId);

        public RaceModel GetRace(int raceId);

        public List<VehicleModel> GetVehiclesFromRace(int raceId);

        void AddMalfunctionStatistic(MalfunctionStatisticModel malfunctionStatistic);

        public void FinishRace();

        public bool SaveChanges();
    }
}
