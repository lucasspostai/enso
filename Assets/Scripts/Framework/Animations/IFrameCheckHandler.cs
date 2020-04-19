namespace Framework.Animations
{
    public interface IFrameCheckHandler
    {
        void OnHitFrameStart();
        void OnHitFrameEnd();
        void OnLastFrameStart();
        void OnLastFrameEnd();
    }
}
