namespace Meeg.Kentico.Configuration.Cms
{
    internal interface IQueryHandler<in TQuery, out TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}
