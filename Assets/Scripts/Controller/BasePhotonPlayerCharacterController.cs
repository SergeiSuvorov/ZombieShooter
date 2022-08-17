using Photon.Pun;
using Tools;

namespace Controller
{
    abstract public class BasePhotonPlayerCharacterController : BaseController, IPhotonCharacterController
    {
        abstract public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
        abstract public void SynhronizeExecute();
    }
}

