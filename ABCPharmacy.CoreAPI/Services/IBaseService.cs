using ABCPharmacy.CoreAPI.Models.DTOs;

namespace ABCPharmacy.CoreAPI.Services
{
    public interface IBaseService<TEntity, TCreateDto>
        where TEntity : class 
        where TCreateDto : class
    {
        /// <summary>
        /// Add a new record
        /// </summary>
        /// <param name="entity">object to be added</param>
        /// <returns>A <see cref="ResponseDTO"/> containing the added entity and a success message,
        /// or an error message if the operation fails.
        /// </returns>
        Task<ResponseDTO> AddAsync(TCreateDto entity);
        /// <summary>
        /// Update a existing record
        /// </summary>
        /// <param name="id">unique id of entity to update</param>
        /// <param name="entity">The Updated entity object containing new data</param>
        /// <returns>A <see cref="ResponseDTO"/> containing the updated entity and a success message,
        /// or an error message if the entity is not found or the update fails.
        /// </returns>
        Task<ResponseDTO> UpdateAsync(int id, TCreateDto entity);
        /// <summary>
        /// Delete existing record
        /// </summary>
        /// <param name="id">unique id of entity to delete</param>
        /// <returns> A <see cref="ResponseDTO"/> indicating whether the deletion was successful,
        /// or an error message if the entity is not found or the operation fails.
        /// </returns></returns>
        Task<ResponseDTO> DeleteAsync(int id);
        /// <summary>
        /// Retrieves a single record by its unique id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="ResponseDTO"/> containing the requested entity if found, 
        /// or an error message if not found or retrieval fails.
        /// </returns>
        Task<ResponseDTO> GetAsync(int id);
        /// <summary>
        /// Retrieves all records from a database
        /// </summary>
        /// <returns>A <see cref="ResponseDTO"/> containing the list of all entities 
        /// and a success or failure message.</returns>
        Task<ResponseDTO> GetAllAsync();
    }
}
