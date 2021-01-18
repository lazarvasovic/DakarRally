using DakarRally.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DakarRally.Simulations
{
    public class Vehicle
    {
        public VehicleModel VehicleModel { get; set; }
        private static object lockObject = new object();
        public MalfunctionStatisticModel MalfunctionStatistic { get; set; }


        public Vehicle(VehicleModel vehicle)
        {
            VehicleModel = vehicle;
        }

        public void Step()
        {
            if (VehicleModel.VehicleStatus == VehicleStatus.Repairing)
            {
                if (VehicleModel.RepairTimeLeft > 0)
                {
                    VehicleModel.RepairTimeLeft -= Simulation.dt;
                    return;
                }
                else
                {
                    VehicleModel.VehicleStatus = VehicleStatus.Running;
                }
            }

            if (VehicleModel.DistanceReached + Simulation.dt * VehicleModel.VehicleSubtype.MaxSpeed >= Simulation.RaceDistance)
            {
                // Last step
                VehicleModel.VehicleStatus = VehicleStatus.Finished;
                VehicleModel.FinishTime = Simulation.t + (Simulation.RaceDistance - VehicleModel.DistanceReached) / VehicleModel.VehicleSubtype.MaxSpeed;
                VehicleModel.DistanceReached = Simulation.RaceDistance;
            }
            else
            {
                VehicleModel.DistanceReached += Simulation.dt * VehicleModel.VehicleSubtype.MaxSpeed;
                VehicleModel.VehicleStatus = VehicleStatus.Running;

                double probability;

                lock(lockObject)
                {
                    probability = Simulation.random.NextDouble();
                }

                if (probability < VehicleModel.VehicleSubtype.HeavyMalfunProbab * Simulation.dt)
                {
                    VehicleModel.VehicleStatus = VehicleStatus.Wracked;
                    MalfunctionStatistic = new MalfunctionStatisticModel
                    {
                        MalfunctionType = MalfunctionType.Heavy,
                        Time = Simulation.t + Simulation.dt,
                        Vehicle = VehicleModel
                    }; 
                }
                else if (probability < (VehicleModel.VehicleSubtype.HeavyMalfunProbab + VehicleModel.VehicleSubtype.LigthMalfunProbab) * Simulation.dt)
                {
                    VehicleModel.VehicleStatus = VehicleStatus.Repairing;
                    VehicleModel.RepairTimeLeft = VehicleModel.VehicleSubtype.VehicleType.RepairLength;
                    MalfunctionStatistic = new MalfunctionStatisticModel
                    {
                        MalfunctionType = MalfunctionType.Light,
                        Time = Simulation.t + Simulation.dt,
                        Vehicle = VehicleModel
                    };
                }
            }
        }
    }
}
