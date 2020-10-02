﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XMemes.Models.InputModels;
using XMemes.Models.ViewModels;
using XMemes.Services.Abstractions;

namespace XMemes.Api.Controllers
{
    public abstract class XMemesControllerBase<TViewModel, TInput>
        : ControllerBase
        where TViewModel: BaseViewModel
        where TInput: BaseInput
    {
        protected readonly ILogger<XMemesControllerBase<TViewModel, TInput>> Logger;
        protected IService<TViewModel, TInput> Service;

        protected XMemesControllerBase(
            ILogger<XMemesControllerBase<TViewModel, TInput>> logger,
            IService<TViewModel, TInput> service)
        {
            Logger = logger;
            Service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<TViewModel>> Get(
            [FromQuery] string? keyword,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 20) =>
            string.IsNullOrWhiteSpace(keyword)
                ? await Service.GetAll(pageIndex, pageSize)
                : await Service.Search(keyword, pageIndex, pageSize);

        [HttpGet("{id}")]
        public async Task<ActionResult<TViewModel>> GetById(
            [FromRoute] string id)
        {
            var isValidId = Guid.TryParse(id, out var guid);
            if (!isValidId)
            {
                return BadRequest("The ID parameter is not a valid Guid");
            }

            var exists = await Service.Exists(guid);

            if (!exists)
            {
                return NotFound();
            }

            var item = await Service.GetById(Guid.Parse(id));
            return item;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TInput input)
        {
            if (!ModelState.IsValid)
            {
                var errors =
                    ModelState.Values
                        .SelectMany(modelState => modelState.Errors)
                        .Aggregate(
                            string.Empty,
                            (current, error) => current + "\n" + error);

                return BadRequest(errors);
            }

            if (await Service.Insert(input))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] TInput input)
        {
            if (!ModelState.IsValid)
            {
                var errors =
                    ModelState.Values
                        .SelectMany(modelState => modelState.Errors)
                        .Aggregate(
                            string.Empty,
                            (current, error) => current + "\n" + error);

                return BadRequest(errors);
            }

            if (!Guid.TryParse(input.Id, out var guid))
                return BadRequest("The ID of the item is not valid");

            if (!await Service.Exists(guid))
                return NotFound("The item to be updated was not found.");

            if (await Service.Update(input))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var isValidId = Guid.TryParse(id, out var guid);
            if (!isValidId)
            {
                return BadRequest("The ID parameter is not a valid Guid");
            }

            return await Service.Delete(guid);
        }
    }
}