using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DakarRally.Data;
using DakarRally.Dtos;
using DakarRally.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DakarRally.Simulations;

namespace DakarRally.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        private readonly IDakarRepo _dakarRepo;
        private readonly IMapper _mapper;
        private readonly Simulation _simulation;

        public RaceController(IDakarRepo dakarRepo, IMapper mapper, Simulation simulation)
        {
            _dakarRepo = dakarRepo;
            _mapper = mapper;
            _simulation = simulation;
        }

        [HttpPost]
        [Route("Create/{year}")]
        public ActionResult<RaceDto> CreateRace(int year)
        {
            RaceModel race = new RaceModel
            {
                Year = year
            };

            _dakarRepo.CreateRace(race);
            _dakarRepo.SaveChanges();

            return Ok(_mapper.Map<RaceDto>(race));
        }

        [HttpPost]
        [Route("AddVehicle/{raceId}")]
        public ActionResult<VehicleModel> AddVehicleToRace(int raceId, VehicleCreateDto vehicle)
        {
            VehicleSubtypeModel subtypeModel = _dakarRepo.GetVehicleSubtype(vehicle.VehicleType, vehicle.VehicleSubtype);

            if(subtypeModel == null)
            {
                return BadRequest("Invalid vehicle type and/or subtype.");
            }

            VehicleModel vehicleToCreate = _mapper.Map<VehicleModel>(vehicle);
            vehicleToCreate.VehicleSubtype = subtypeModel;

            if (_dakarRepo.AddVehicleToRace(raceId, vehicleToCreate, out string status))
            {
                _dakarRepo.SaveChanges();
                return Ok(vehicleToCreate);
            }

            return BadRequest(status);
        }

        [HttpPut]
        [Route("UpdateVehicle")]
        public ActionResult<VehicleModel> UpdateVehicle(VehicleUpdateDto vehicle)
        {
            VehicleModel vehicleToUpdate = _dakarRepo.UpdateVehicle(vehicle.Id, out string status);

            if (vehicleToUpdate == null)
            {
                return BadRequest(status);
            }

            VehicleSubtypeModel subtypeModel = _dakarRepo.GetVehicleSubtype(vehicle.VehicleType, vehicle.VehicleSubtype);

            if (subtypeModel == null)
            {
                return BadRequest("Invalid vehicle type and/or subtype.");
            }

            _mapper.Map(vehicle, vehicleToUpdate);
            vehicleToUpdate.VehicleSubtype = subtypeModel;

            _dakarRepo.SaveChanges();

            return Ok(vehicleToUpdate);

        }

        [HttpDelete]
        [Route("RemoveVehicle/{id}")]
        public ActionResult RemoveVehicle(int id)
        {
            if (_dakarRepo.RemoveVehicle(id, out string status))
            {
                _dakarRepo.SaveChanges();
                return Ok(status);
            }

            return BadRequest(status);
        }

        [HttpPost]
        [Route("StartRace/{id}")]
        public ActionResult StartRace(int id)
        {
            if(_simulation.StartRace(id, out string status))
            {
                return Ok("Race started.");
            }

            return BadRequest(status);
        }

        [HttpGet]
        [Route("GetLeaderboard")]
        public ActionResult<IEnumerable<VehicleLeaderboardDto>> GetLeaderboard()
        {
            List<VehicleModel> vehicles = _dakarRepo.GetAllVehiclesLeaderboard();

            List<VehicleLeaderboardDto> vehiclesLeaderboard = _mapper.Map<List<VehicleLeaderboardDto>>(vehicles);

            for (int i = 0; i < vehiclesLeaderboard.Count(); i++)
            {
                vehiclesLeaderboard[i].CurrentPlace = i + 1;
            }

            return Ok(vehiclesLeaderboard);
        }

        [HttpGet]
        [Route("GetLeaderboard/{type}")]
        public ActionResult<IEnumerable<VehicleLeaderboardDto>> GetLeaderboard(string type)
        {
            List<VehicleModel> vehicles = _dakarRepo.GetLeaderboardByType(type);

            List<VehicleLeaderboardDto> vehiclesLeaderboard = _mapper.Map<List<VehicleLeaderboardDto>>(vehicles);

            for (int i = 0; i < vehiclesLeaderboard.Count(); i++)
            {
                vehiclesLeaderboard[i].CurrentPlace = i + 1;
            }

            return Ok(vehiclesLeaderboard);
        }

        [HttpGet]
        [Route("GetVehicleStatistic/{id}")]
        public ActionResult<VehicleStatisticDto> GetVehicleStatistic(int id)
        {
            VehicleModel vehicle = _dakarRepo.GetVehicle(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            List<MalfunctionStatisticDto> malfunctions = _mapper.Map<List<MalfunctionStatisticDto>>(_dakarRepo.GetVehicleMalfunctions(id));

            VehicleStatisticDto vehicleStatistic = _mapper.Map<VehicleStatisticDto>(vehicle);
            vehicleStatistic.Malfunctions = malfunctions;

            return Ok(vehicleStatistic);
        }

        [HttpGet]
        [Route("FindVehicles")]
        public ActionResult<IEnumerable<VehicleInfoDto>> FindVehicles(string team = null, string model = null,
            DateTime? manufacturingDate = null, string status = null, double? distance = null, string sortOrder = null)
        {
            List<VehicleModel> vehicles = _dakarRepo.FindVehicles(team, model,
                manufacturingDate, status, distance, sortOrder);

            return Ok(_mapper.Map<List<VehicleInfoDto>>(vehicles));
        }

        [HttpGet]
        [Route("GetRaceStatus/{id}")]
        public ActionResult<RaceStatusDto> GetRaceStatus(int id)
        {
            List<VehiclesByStatusDto> vehicleByStatus = _dakarRepo.GetVehiclesGroupedByStatus(id);
            List<VehiclesByTypeDto> vehicleByType = _dakarRepo.GetVehiclesGroupedByType(id);
            RaceModel race = _dakarRepo.GetRace(id);

            if (race == null)
            {
                return NotFound();
            }

            RaceStatusDto raceStatus = new RaceStatusDto
            {
                TimeElapsed = race.TimeElapsed,
                RaceStatus = race.Status.ToString(),
                VehiclesByType = vehicleByType,
                VehiclesByStatus = vehicleByStatus
            };

            return Ok(raceStatus);
        }
    }
}
