namespace Controller
{
    public class RemoutePlayerCharacterController : BasePlayerCharacterController
    {
        public RemoutePlayerCharacterController(CharacterView view)
        {
           InitView(view);
            _photonCharacterController = new RemotePhotonCharacterSynchronizeController(view);
        }

        public override void UpdateExecute()
        {
            _photonCharacterController.SynhronizeExecute();
        }

    }
}

