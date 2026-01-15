using CheckinLibrary.Models;
using Mapster;
using System;
using System.Collections.ObjectModel;
using System.Linq;

public static class MapsterConfig
{
    public static void Register()
    {
        // -------------------------------------------------
        // Employee
        // -------------------------------------------------
        TypeAdapterConfig<EmployeeDTO, Employee>.NewConfig()
            .Map(dest => dest.ID, src => src.ID)
            .Map(dest => dest.CardID, src => src.CardID)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.MiddleName, src => src.MiddleName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.IsOffSite, src => src.IsOffSite)
            .Map(dest => dest.OffSiteUntil, src => src.OffSiteUntil)
            // Latest onsite time
            .Map(dest => dest.ArrivalTime,
                src => src.OnSiteTimes
                    .OrderByDescending(t => t.ArrivalTime)
                    .Select(t => (DateTime?)t.ArrivalTime)
                    .FirstOrDefault())
            .Map(dest => dest.DepartureTime,
                src => src.OnSiteTimes
                    .OrderByDescending(t => t.ArrivalTime)
                    .Select(t => t.DepartureTime)
                    .FirstOrDefault())
            .Map(dest => dest.IsCheckedIn,
                src => src.OnSiteTimes.Any(t => t.DepartureTime == null))
            .IgnoreNonMapped(true);

        // -------------------------------------------------
        // OnSiteTime
        // -------------------------------------------------
        TypeAdapterConfig<OnSiteTimeDTO, OnSiteTime>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.EmployeeID, src => src.EmployeeID)
            .Map(dest => dest.ArrivalTime, src => src.ArrivalTime)
            .Map(dest => dest.DepartureTime, src => src.DepartureTime)
            .IgnoreNonMapped(true);

        // -------------------------------------------------
        // EmployeeGroup → Employee (bridge entity)
        // -------------------------------------------------
        TypeAdapterConfig<EmployeeGroupDTO, Employee>.NewConfig()
            .MapWith(src => src.Employee.Adapt<Employee>())
            .IgnoreNonMapped(true);

        // -------------------------------------------------
        // Group
        // -------------------------------------------------
        TypeAdapterConfig<GroupDTO, Group>.NewConfig()
            .Map(dest => dest.ID, src => src.ID)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Isvisible, src => src.Isvisible)
            // Eagerly load members using the bridge entity mapping
            .Map(dest => dest.Members,
                src => new ObservableCollection<Employee>(
                    src.EmployeeGroups.Select(eg => eg.Employee.Adapt<Employee>())
                ))
            .IgnoreNonMapped(true);

        // -------------------------------------------------
        // Absence
        // -------------------------------------------------
        TypeAdapterConfig<AbsenceDTO, Absence>.NewConfig()
            .Map(dest => dest.ID, src => src.ID)
            .Map(dest => dest.EmployeeId, src => src.EmployeeId)
            .Map(dest => dest.FromDate, src => src.FromDate)
            .Map(dest => dest.ToDate, src => src.ToDate)
            .Map(dest => dest.Note, src => src.Note)
            .Map(dest => dest.AbsenceReasonId, src => src.AbsenceReasonId)
            .AfterMapping(dest =>
            {
                dest.FromTime = TimeOnly.FromDateTime(dest.FromDate);
                dest.ToTime = TimeOnly.FromDateTime(dest.ToDate);
            })
            .IgnoreNonMapped(true);

        // -------------------------------------------------
        // AdminUser
        // -------------------------------------------------
        TypeAdapterConfig<AdminUserDTO, AdminUser>.NewConfig()
            .Map(dest => dest.ID, src => src.ID)
            .Map(dest => dest.Username, src => src.Username)
            .IgnoreNonMapped(true);

        // -------------------------------------------------
        // Safety: require explicit mapping
        // -------------------------------------------------
        TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
    }
}
