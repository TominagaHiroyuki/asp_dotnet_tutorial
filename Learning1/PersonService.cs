/*
    @file PersonService.cs
    @brief PersonService class
*/

namespace MyWebApp.Services;

public class PersonService : IPersonService
{
    public string GetName()
    {
        return "John Doe";
    }
}

