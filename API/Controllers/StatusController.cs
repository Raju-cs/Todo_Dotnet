using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    
    public class StatusController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStatus([FromBody] StatusDto statusDto)
        {
            var status = new Status()
            {
                Name = statusDto.Name,
                Color = statusDto.Color
            };

            _unitOfWork.Repository<Status>().Add(status);

            if (await _unitOfWork.Complete() <= 0)
                return BadRequest("Failed to save");

            return Ok(status);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatuses([FromQuery] int? id)
        {
            if (id is not null)
            {
                var status = await _unitOfWork.Repository<Status>().GetByIdAsync(id ?? 0);

                return status is null ? NotFound("Status not found") : Ok(status);
            }

            var statuses = await _unitOfWork.Repository<Status>().ListAllAsync();

            return Ok(statuses);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] StatusDto statusDto)
        {

            var status = await _unitOfWork.Repository<Status>().GetByIdAsync(id);

            if (status is null) return NotFound("Status not found");

            status.Color = statusDto.Color;
            status.Name = statusDto.Name;

            if (await _unitOfWork.Complete() <= 0) return BadRequest("Failed to save");

            return Ok(status);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus([FromRoute] int id)
        {
            var status = await _unitOfWork.Repository<Status>().GetByIdAsync(id);

            if (status is null) return NotFound("Status not found");

            _unitOfWork.Repository<Status>().Delete(status);

            if (await _unitOfWork.Complete() <= 0) return BadRequest("Failed to save");

            return NoContent();
        }
    }
}