using Lib.Common.Interfaces.Core;
using MediatR;

namespace Lib.Core.Interfaces.EventsBus;

public interface IEventParams : IID, INotification
{
    DateTime OccurredOn { get; }
}