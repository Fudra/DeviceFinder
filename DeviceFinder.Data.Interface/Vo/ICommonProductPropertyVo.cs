namespace DeviceFinder.Data.Interface.Vo
{
    public interface ICommonProductPropertyVo
    {
        int ProductPropertyId { get; }
        int ProductPropertyNameId { get; }
        int ProductPropertyGroupId { get; }
        int DisplaySequence { get; }
    }
}