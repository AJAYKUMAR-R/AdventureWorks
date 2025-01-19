namespace Adventure.Enums;


public enum Role
{
    Admin ,
    Employee,
    Customer
}

/// <summary>
/// 0 = Product is purchased, 1 = Product is manufactured in-house.
/// </summary>
public enum MakeFlag
{
    ProductPurchased = 0,
    ProductHome = 1
}

/// <summary>
/// 0 = Product is not a salable item. 1 = Product is salable.
/// </summary>
public enum FinishedGoodsFlag
{
    ProductNotSalable = 0,
    ProductSalable =1
}

/// <summary>
/// R = Road, M = Mountain, T = Touring, S = Standard
/// </summary>
public enum ProductLine
{
    R,
    M,
    T,
    S
}

/// <summary>
/// H = High, M = Medium, L = Low
/// </summary>
public enum Class 
{
    H,
    M,
    L
}


/// <summary>
/// W = Womens, M = Mens, U = Universal
/// </summary>
public enum Style
{
   W,
   M,
   U
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