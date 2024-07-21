using AniView.Entities; 
using AniView.Service; 

namespace AniView.Controller;

public class UserController(UserService service)
{
    private readonly UserService _userService = service;
}