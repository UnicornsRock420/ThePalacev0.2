using sint32 = System.Int32;

namespace ThePalace.Core.Interfaces.Data;

public interface IStructRefNum : IStruct
{
    sint32 RefNum { get; set; }
}