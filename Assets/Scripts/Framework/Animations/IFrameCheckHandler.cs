namespace Framework.Animations
{
    public interface IFrameCheckHandler
    {
        void OnHitFrameStart();
        void OnHitFrameEnd();
        void OnCanCutAnimation();
        void OnLastFrameStart();
        void OnLastFrameEnd();
    }
}
