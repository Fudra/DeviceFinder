namespace DeviceFinder.Data.Interface.Vo
{
    public interface ICommonTranslateSizingWordVo
    {
        int LangId { get; }
        int WordId { get; }
        string Word { get; }
    }
}