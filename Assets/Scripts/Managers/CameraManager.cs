using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    // 해당 카메라 호출 시에 인스턴스 생성 후 전달
    public CinemachineBrain cinemachineBrain;
    public SideView_Camera SideViewCamera;
    public CinemachineVirtualCamera TpsViewCamera;

    public bool activeSideView;

    public void Init()
    {
        base.SingletonInit();
    }

    public void SwitchSideViewCamera(bool active)
    {
        if (active)
        {
            activeSideView = true;
            SideViewCamera.virtualCamera.Priority = 99;
        }
        else
        {
            activeSideView = false;
            SideViewCamera.virtualCamera.Priority = 0;
        }
    }


}