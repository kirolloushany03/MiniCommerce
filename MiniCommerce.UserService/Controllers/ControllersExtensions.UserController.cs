namespace MiniCommerce.UserService.Controllers;
public partial class ControllersExtensions
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var groupName = "Users";

        var group = app.MapGroup("/api/users").WithTags(groupName);

        group.MapPost("", PostUserAsync)
            .WithTags(groupName)
            .WithSummary(nameof(PostUserAsync))
            .WithDescription("Create a new user");

        group.MapGet("", GetAllUsersAsync)
            .WithTags(groupName)
            .WithSummary(nameof(GetAllUsersAsync))
            .WithDescription("Get all users");
        
        group.MapGet("/{id:guid}", GetUserByIdAsync)
            .WithTags(groupName)
            .WithSummary(nameof(GetUserByIdAsync))
            .WithDescription("Get user by ID");
        
        group.MapPut("/{id:guid}", PutUserAsync)
            .WithTags(groupName)
            .WithSummary(nameof(PutUserAsync))
            .WithDescription("Update user details");
        
        group.MapDelete("/{id:guid}", DeleteUserAsync)
            .WithTags(groupName)
            .WithSummary(nameof(DeleteUserAsync))
            .WithDescription("Delete a user");
        
        group.MapPost("/{id:guid}/add-balance", AddBalanceAsync)
            .WithTags(groupName)
            .WithSummary(nameof(AddBalanceAsync))
            .WithDescription("Add balance to user wallet");
        
        return app;
    }
}
