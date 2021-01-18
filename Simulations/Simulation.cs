using DakarRally.Data;
using DakarRally.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DakarRally.Simulations
{
    public class Simulation
    {
        public static double RaceDistance = 2000;
        public static double dt = 1;
        public static double t = 0;
        public static Random random = new Random();
        private static object lockObject = new object();
        private readonly IDakarRepo _dakarRepo;
        private int raceId;

        public Simulation(IDakarRepo dakarRepo)
        {
            _dakarRepo = dakarRepo;
        }
        public bool StartRace(int id, out string status)
        {
            lock (lockObject)
            {
                if (_dakarRepo.StartRace(id, out status))
                {
                    raceId = id;
                    _dakarRepo.SaveChanges();

                    new Thread(Start).Start();
                    return true;
                }

                return false;
            }

        }

        private void Start()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DakarDbContext>();
            optionsBuilder.UseSqlite("Data Source=DakarDatabase.db");

            using (var dataContext = new DakarDbContext(optionsBuilder.Options))
            {
                var _dakarRepo = new DakarRepo(dataContext);

                List<VehicleModel> vehicleModels = _dakarRepo.GetVehiclesFromRace(raceId);
                List<Vehicle> vehicles = new List<Vehicle>();
                Thread[] threads = new Thread[vehicleModels.Count];

                for (int i = 0; i < vehicleModels.Count; i++)
                {
                    vehicles.Add(new Vehicle(vehicleModels[i]));
                    vehicles[i].VehicleModel.VehicleStatus = VehicleStatus.Running;
                }

                RaceStatus raceStatus = RaceStatus.Running;

                while (raceStatus == RaceStatus.Running)
                {
                    for (int i = 0; i < vehicles.Count; i++)
                    {
                        if (vehicles[i].VehicleModel.VehicleStatus != VehicleStatus.Wracked &&
                            vehicles[i].VehicleModel.VehicleStatus != VehicleStatus.Finished)
                        {
                            threads[i] = new Thread(vehicles[i].Step);
                            threads[i].Start();
                        }
                    }

                    raceStatus = RaceStatus.Finished;

                    for (int i = 0; i < vehicles.Count; i++)
                    {
                        threads[i].Join();

                        if (vehicles[i].VehicleModel.VehicleStatus != VehicleStatus.Finished &&
                            vehicles[i].VehicleModel.VehicleStatus != VehicleStatus.Wracked)
                        {
                            raceStatus = RaceStatus.Running;
                        }

                        if (vehicles[i].MalfunctionStatistic != null)
                        {
                            _dakarRepo.AddMalfunctionStatistic(vehicles[i].MalfunctionStatistic);
                            vehicles[i].MalfunctionStatistic = null;
                        }
                    }

                    Thread.Sleep((int)(dt * 10000));

                    _dakarRepo.SaveChanges();

                    t += dt;
                }

                _dakarRepo.FinishRace();
                _dakarRepo.SaveChanges();
            }
            
        }
    }
}
