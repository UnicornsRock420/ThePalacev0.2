using sint32 = int;

namespace Lib.Core.Interfaces.Data;

public interface IStructRefNum : IStruct
{
    sint32 RefNum { get; set; }
}