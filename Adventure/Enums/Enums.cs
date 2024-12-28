namespace Adventure.Enums;


public enum Role
{
    Admin ,
    Employee,
    Customer
}

public static class RoleConstants
{
    public const string Employee = "Employee";
    public const string Admin = "Admin";
    public const string Customer = "Customer";
}


public enum ClaimsPermissions
{
    None = 0,
    Engineering,
    ToolDesign,
    Sales,
    Marketing,
    Purchasing,
    ResearchandDevelopment,
    Production,
    ProductionControl,
    HumanResources,
    Finance,
    InformationServices,
    DocumentControl,
    QualityAssurance,
    FacilitiesandMaintenance,
    ShippingandReceiving,
    Executive
}