namespace Tools
{
    public interface ILateUpdateable
    {
        bool IsActive { get; }

        void LateUpdateExecute();
    }
}



