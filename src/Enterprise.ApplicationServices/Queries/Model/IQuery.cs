namespace Enterprise.ApplicationServices.Queries.Model;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a query object.
/// It is used primarily for constraint purposes.
/// </summary>
public interface IQuery;

/// <summary>
/// This is a marker interface that signifies that an implementing class is a query object.
/// It is used primarily for constraint purposes.
/// This interface allows for defining an explicit return type that is associated with the query.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<TResponse> : IQuery;