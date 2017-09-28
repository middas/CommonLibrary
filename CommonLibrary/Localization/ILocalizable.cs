namespace CommonLibrary.Localization
{
    public interface ILocalizable
    {
        bool Localized { get; }

        void Localize();
    }
}