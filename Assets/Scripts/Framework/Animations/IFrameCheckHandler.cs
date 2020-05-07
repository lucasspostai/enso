namespace Framework.Animations
{
    public interface IFrameCheckHandler
    {
        void OnPlayAudio();
        void OnHitFrameStart();
        void OnHitFrameEnd();
        void OnCanCutAnimation();
        void OnStartMovement();
        void OnEndMovement();
        void OnLastFrameStart();
        void OnLastFrameEnd();
    }
}
