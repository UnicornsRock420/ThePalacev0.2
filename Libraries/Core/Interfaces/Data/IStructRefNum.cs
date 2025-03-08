using sint32 = int;

namespace ThePalace.Core.Interfaces.Data;

public interface IStructRefNum : IStruct
{
    sint32 RefNum { get; set; }
}