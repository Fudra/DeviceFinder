namespace DeviceFinder.Data.Interface.Vo
{
    public interface ICommonLocaleVo
    {
        int LocaleId { get; }
        int LanguageId { get; }
        string LocaleCode { get; }
    }
}