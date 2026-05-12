using Microsoft.AspNetCore.Mvc;
using MiniCommerce.UserService.Models;
using MiniCommerce.UserService.Services.Foundations;
using static MiniCommerce.UserService.Models.DTOs.UserDtos;

namespace MiniCommerce.UserService.Controllers;

public static partial class ControllersExtensions
{
    static async ValueTask<IResult> PostUserAsync(IUserService userService, [FromBody] CreateUserDto dto)
    {
        try
        {
            // Manual Mapping (DTO -> Entity)
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                WalletBalance = dto.InitialBalance
            };

            await userService.AddUserAsync(user);
            //return Results.Created($"/api/users/{user.Id}", user);
            return Results.Created();
        }
        catch (EmailAlreadyInUseException ex)
        {
            return Results.Conflict(ex.Message); 
        }
    }

    static async ValueTask<IResult> GetAllUsersAsync(IUserService userService) =>
        Results.Ok(await userService.RetrieveAllUsersAsync()); 

    static async ValueTask<IResult> GetUserByIdAsync(Guid id, IUserService userService)
    {
        var user = await userService.RetrieveUserByIdAsync(id);
        return user is not null ? Results.Ok(user) : Results.NotFound(new { Message = "User  not found" });
    }

    static async ValueTask<IResult> PutUserAsync(Guid id, IUserService userService, [FromBody] UpdateUserDto dto)
    {
        try
        {
            var existingUser = await userService.RetrieveUserByIdAsync(id);
            if (existingUser is null) return Results.NotFound(new { Message = $"User not found" });

            existingUser.Name = dto.Name;
            existingUser.Email = dto.Email;

            await userService.ModifyUserAsync(existingUser);
            return Results.Ok(existingUser); 
        }
        catch (UserNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (EmailAlreadyInUseException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    static async ValueTask<IResult> DeleteUserAsync(Guid id, IUserService userService)
    {
        try
        {
            await userService.RemoveUserByIdAsync(id);
            return Results.NoContent(); 
        }
        catch (UserNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }

    static async ValueTask<IResult> AddBalanceAsync(Guid id, [FromBody] AddBalanceDto dto, IUserService userService)
    {
        try
        {
            await userService.AddBalanceAsync(id, dto.Amount);
            return Results.Ok(new { Message = "Balance added successfully." }); 
        }
        catch (UserNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (InvalidAmountException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
