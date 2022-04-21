namespace Assets.CoreData.Interfaces
{
    public interface INamed
    {
        string Name { get; set; }
    }

    public interface IWithLenghtInt : IWithLenght<int> { }
    public interface IWithLenghtFloat : IWithLenght<float> { }
    public interface IWithLenght<TLength>
    {
        TLength Length { get; set; }
    }
}