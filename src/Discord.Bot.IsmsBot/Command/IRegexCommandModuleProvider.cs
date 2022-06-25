namespace IsmsBot.Command
{
    public interface IRegexCommandModuleProvider
    {
        object GetModuleInstance(RegexCommandInstance commandInstance);
    }
}