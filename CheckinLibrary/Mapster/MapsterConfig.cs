using CheckinLibrary.Models;
using Mapster;

public static class MapsterConfig
{
    public static void Register()
    {
        TypeAdapterConfig<EmployeeDTO, Employee>.NewConfig()
            .Map(dest => dest.IsCheckedIn,
                 src => src.OnSiteTimes.Any(t => t.DepartureTime == null));

        TypeAdapterConfig<OnSiteTimeDTO, OnSiteTime>.NewConfig();

        TypeAdapterConfig<GroupDTO, Group>.NewConfig()
            .Map(dest => dest.Members,
                 src => src.EmployeeGroups.Select(eg => eg.Employee).Adapt<List<Employee>>());

        TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
    }
}
