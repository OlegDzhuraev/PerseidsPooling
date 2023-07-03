namespace InsaneOne.PerseidsPooling
{
    /// <summary> Use this interface for all components, which should be resetted after re-loading of object from pool. </summary>
    public interface IResettable
    {
        void ResetPooled();
    }
}