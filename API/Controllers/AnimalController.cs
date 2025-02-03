using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEFmysql.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly AnimalRepository _animalRepository;

        public AnimalController(AnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        [HttpGet("GetListAnimals")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
        {
            try
            {
                var animals = await _animalRepository.GetAnimalsAsync();
                return Ok(animals);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving animals: {ex.Message}");
            }
        }
        [HttpGet("GetListBreed")]
        public async Task<ActionResult<IEnumerable<string>>> GetBreeds()
        {
            try
            {
                var breeds = await _animalRepository.GetDistinctBreedsAsync();
                return Ok(breeds);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving breeds: {ex.Message}");
            }
        }
        [HttpGet("GetListBreedByType/{type}")]
        public async Task<ActionResult<IEnumerable<string>>> GetBreeds(string type)
        {
            try
            {
                var breeds = await _animalRepository.GetDistinctBreedsAsync(type);
                return Ok(breeds);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving breeds: {ex.Message}");
            }
        }
        [HttpGet("GetListType")]
        public async Task<ActionResult<IEnumerable<string>>> GetTypes()
        {
            try
            {
                var types = await _animalRepository.GetDistinctTypesAsync();
                return Ok(types);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving types: {ex.Message}");
            }
        }

        [HttpGet("GetAnimal/{id}")]
        public async Task<ActionResult<Animal>> GetAnimalById(int id)
        {
            try
            {
                var animal = await _animalRepository.GetAnimalsByIdAsync(id);

                if (animal == null)
                {
                    return NotFound($"Animal with ID {id} not found");
                }

                return Ok(animal);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving animal: {ex.Message}");
            }
        }
        [HttpPost("NewAnimal")]
        public async Task<ActionResult<User>> CreateAnimal([FromBody] Animal animal)
        {
            try
            {
                var createdAnimal = await _animalRepository.CreateAnimalAsync(animal);
                return CreatedAtAction(nameof(GetAnimalById), new { id = createdAnimal.animal_id }, createdAnimal);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating animal: {ex.Message}");
            }
        }

        [HttpPut("UpdateAnimalStatus/{id}")]
        public async Task<ActionResult<User>> UpdateAnimal(int id, [FromBody] Animal animal)
        {
            try
            {
                var existingAnimal = await _animalRepository.GetAnimalsByIdAsync(id);

                if (existingAnimal == null)
                {
                    return NotFound($"Animal with ID {id} not found");
                }
                existingAnimal.animal_status = animal.animal_status;

                await _animalRepository.UpdateAnimalAsync(animal);

                return Ok(animal);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating animal: {ex.Message}");
            }
        }

        [HttpDelete("DeleteAnimal/{id}")]
        public async Task<ActionResult> DeleteAnimal(int id)
        {
            try
            {
                var animalToDelete = await _animalRepository.GetAnimalsByIdAsync(id);

                if (animalToDelete == null)
                {
                    return NotFound($"Animal with ID {id} not found");
                }

                await _animalRepository.DeleteAnimalAsync(animalToDelete);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting animal: {ex.Message}");
            }
        }
        [HttpGet("AnimalPicture/{id}")]
        public async Task<ActionResult> GetAnimalPicture(int id)
        {
            try
            {
                var animal = await _animalRepository.GetAnimalsByIdAsync(id);

                if (animal == null)
                {
                    return NotFound($"Animal with ID {id} not found");
                }

                return File(animal.animal_picture, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving animal picture: {ex.Message}");
            }
        }
    }
}
