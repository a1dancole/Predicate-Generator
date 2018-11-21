namespace PredicateGenerator.Generator
{
    public interface IPredicateGenerator<T1>
    {
        PredicateGenerator<T1> GeneratePredicate<T2>(T2 searchParametersDto);
    }
}