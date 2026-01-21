using restaraunt.Application.Services;
using restaraunt.API.Contracts;

namespace restaraunt.API.Controlers
{
    public static class UsersEndpoints
    {
        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("register", Register);

            app.MapPost("login", Login);

            return app;
        }
        public static async Task<IResult> Register(
            RegisterUserRequest request,
            UserService userService)
        {
            await userService.Register(request.UserName, request.Email, request.Password);
            return Results.Ok();
        }
        public static async Task<IResult> Login(
            LoginUserRequest request,
            UserService userService,
            HttpContext context
            )
        {
            var token = await userService.Login(request.Email, request.Password);

            context.Response.Cookies.Append("tasty", token);
            return Results.Ok();
        }
    }
}