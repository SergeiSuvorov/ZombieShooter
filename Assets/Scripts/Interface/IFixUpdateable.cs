namespace Tools
{
    public interface IFixUpdateable
    {
        bool IsActive { get; }

        void FixUpdateExecute();
    }
}



