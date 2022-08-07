namespace Tools
{
    public interface IUpdateable
    {
        bool IsActive { get; }

        void UpdateExecute();
    }
}



