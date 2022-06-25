namespace IsmsBot.RegexCommand
{
    public interface IRegexCommandModuleProvider
    {
        object GetModuleInstance(RegexCommandInstance commandInstance);
    }
}