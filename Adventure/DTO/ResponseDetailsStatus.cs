using System.Runtime.Serialization;

namespace Adventure.DTO;

[DataContract()]
public class ResponseDetailsStatus {

    [DataMember(IsRequired = true)]
    public string? Description {get; set;} 

    [DataMember(IsRequired = true)]
    public bool Success {get; set;}  

    [DataMember(IsRequired = true)]
    public object? Data {get; set;}   


}