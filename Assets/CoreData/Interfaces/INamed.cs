namespace Assets.CoreData.Interfaces
{
    public interface INamed
    {
        string Name { get; set; }
    }

    public interface IWithLenghtInt : IWithLenght<int> { }
    public interface IWithLenghtFloat : IWithLenght<float> { }
    public interface IWithLenghtDouble : IWithLenght<double> { }
    public interface IWithLenght<TLength>
    {
        TLength Length { get; set; }
    }
}