/*
    @file WelcomService.cs
    @brief Welcome service implementation
*/

using MyApp.Interface;

namespace MyApp.Services;

public class WelcomService : IWelcomService
{
    private DateTime _serviceCreated;
    private Guid _serviceId;

    public WelcomService()
    {
        _serviceCreated = DateTime.Now;
        _serviceId = Guid.NewGuid();
    }

    public string GetWelcomeMessage()
    {
        return $"Welcome to Contoso! The current time is {_serviceCreated}. This service instance has an ID of {_serviceId}";
    }
}