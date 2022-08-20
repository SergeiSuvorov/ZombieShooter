using Photon.Realtime;
using System;

public interface IDamageReceiver
{
    event Action<int,Player> onGetDamage;
    void GetDamage(int damage, Player player);
}



