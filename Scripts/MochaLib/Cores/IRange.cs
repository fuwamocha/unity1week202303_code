namespace MochaLib.Cores
{
    public interface IRange<T> where T : struct
    {
        T Min { get; set; }
        T Max { get; set; }
        T Mid { get; }
        T Random { get; }
    }
}