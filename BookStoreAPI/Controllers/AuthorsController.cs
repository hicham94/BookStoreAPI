using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreAPI.Contracts;
using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, ILoggerService logger, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// GET All Authors
        /// </summary>
        /// <returns>List Of Authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _logger.LogInfo("trying to get all Authors...");
                var authors = await _authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDTO>>(authors);
                _logger.LogInfo("succes to get all Authors");
                return Ok(response);
            }
            catch (Exception e)
            {
                /*_logger.LogError($"{e.Message} - {e.InnerException}");
                return StatusCode(500, "une erreur se produit");*/
                return InternalError($"{e.Message} - {e.InnerException}");
            }

        }
        /// <summary>
        /// GET an Author by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>An Author</returns>

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthor(int Id)
        {
            try
            {
                _logger.LogInfo($"Trying to get the author with id:{Id}...");
                var author = await _authorRepository.FindById(Id);
                if (author == null)//test if author found or not
                {
                    _logger.LogWarn($"The author with id:{Id} was not found");
                    return NotFound();
                }
                var response = _mapper.Map<AuthorDTO>(author);
                _logger.LogInfo($"Succes to get the author with id:{Id}");
                return Ok(response);
            }
            catch (Exception e)
            {
               return InternalError($"{e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// CREATE an Author
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDTO authorDTO)
        {
            try
            {
                _logger.LogInfo($"Traying to add an Author...");
                if (authorDTO == null)
                {
                    _logger.LogWarn($"Author data empty");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Author data incomplited");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDTO);
                var isSucces = await _authorRepository.Create(author);
                if (!isSucces)
                {
                    return InternalError($"Creating Author is failed");
                }
                _logger.LogInfo("Author Created");
                return Created("Create", new { author });
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// UPDATE an Author
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="authorEditDTO"></param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int Id, [FromBody] EditAuthorDTO authorEditDTO)
        {
            try
            {
                _logger.LogWarn($"Traying to EDIT the Author with id:{Id}.....");
                if (Id < 1 || authorEditDTO == null || Id != authorEditDTO.Id)
                {
                    _logger.LogWarn($"Author data Empty");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Author data incomplited");
                    return BadRequest();
                }
                var isExist = await _authorRepository.IsExist(Id);
                if (!isExist)
                {
                    _logger.LogWarn($"Author not exist");
                    return NotFound();
                }
                var author = _mapper.Map<Author>(authorEditDTO);
                var isSucces = await _authorRepository.Update(author);
                if (!isSucces)
                {
                    return InternalError($"UPDATE Author failed");
                }
            }
            catch (Exception e)
            {

                return InternalError($"{e.Message} - {e.InnerException}");
            }
            _logger.LogInfo($"UPDATE of Authod with id:{Id} is succed");
            return NoContent();
        }
        /// <summary>
        /// DELETE Author by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                _logger.LogWarn($"Traying to DELETE the Author with id:{Id}.....");
                if (Id < 1 )
                {
                    _logger.LogWarn($"Author Id invalid");
                    return BadRequest();
                }
                var isExist = await _authorRepository.IsExist(Id);
                if (!isExist)
                {
                    _logger.LogWarn($"Author not exist");
                    return NotFound();
                }
                var author = await _authorRepository.FindById(Id);
                var isSucces = await _authorRepository.Delete(author);
                if (!isSucces)
                {
                    return InternalError($"DELETE Author failed");
                }
            }
            catch (Exception e)
            {

                return InternalError($"{e.Message} - {e.InnerException}");
            }
            _logger.LogInfo($"DELETE of Authod with id:{Id} is succed");
            return NoContent();
        }
        private ObjectResult InternalError(string Message)
        {
            _logger.LogError(Message);
            return StatusCode(500, "une erreur se produit");
        }
    }
}