using MediatR;

namespace Enterprise.MediatR.Adapters.Abstract;

/// <summary>
/// Marker interface used for command adapters.
/// This is primarily used for constraints for implementations of MediatR types like IPipelineBehavior.
/// </summary>
public interface ICommandAdapter : IRequest
{

}