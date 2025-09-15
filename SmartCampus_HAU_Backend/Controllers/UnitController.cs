using SmartCampus_HAU_Backend.Services.Interfaces;
using SmartCampus_HAU_Backend.Models.DTOs.Units;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SmartCampus_HAU_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitController
    {
        private readonly IUnitService _unitService;
        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet("room/unit/getall/{roomId}")]
        public async Task<IActionResult> GetAllUnits([FromRoute] int roomId)
        {
            try
            {
                var units = await _unitService.GetAllUnitsAsync(roomId);
                return new OkObjectResult(units);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("room/unit/get/{unitId}")]
        public async Task<IActionResult> GetUnitById([FromRoute] int unitId)
        {
            try
            {
                var unit = await _unitService.GetUnitByIdAsync(unitId);
                return new OkObjectResult(unit);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("room/unit/add")]
        public async Task<IActionResult> AddUnit([FromBody] CreateUnitDTO createUnitDTO)
        {
            if (createUnitDTO == null)
            {
                return new BadRequestObjectResult("Invalid unit data.");
            }
            try
            {
                var result = await _unitService.AddUnitAsync(createUnitDTO);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut("room/unit/update/{unitId}")]
        public async Task<IActionResult> UpdateUnit([FromRoute] int unitId, [FromBody] UnitDTO unitDTO)
        {
            if (unitDTO == null)
            {
                return new BadRequestObjectResult("Invalid unit data.");
            }
            try
            {
                var result = await _unitService.UpdateUnitAsync(unitId, unitDTO);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("room/unit/delete/{unitId}")]
        public async Task<IActionResult> DeleteUnit([FromRoute] int unitId)
        {
            try
            {
                var success = await _unitService.DeleteUnitAsync(unitId);
                if (success)
                {
                    return new OkObjectResult($"Unit with ID {unitId} deleted successfully.");
                }
                else
                {
                    return new NotFoundObjectResult($"Unit with ID {unitId} not found.");
                }
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
