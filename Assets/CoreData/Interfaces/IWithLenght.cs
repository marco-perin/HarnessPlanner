namespace Assets.CoreData.Interfaces
{
    public interface IWithLenghtInt : IWithLenght<int> { }
    public interface IWithLenghtFloat : IWithLenght<float> { }
    public interface IWithLenghtDouble : IWithLenght<double> { }

    public interface IWithLenght<TLength>
    {
        TLength Length { get; set; }
    }
}