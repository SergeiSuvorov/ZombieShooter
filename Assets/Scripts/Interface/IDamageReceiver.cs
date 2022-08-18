using System;

public interface IDamageReceiver
{
    event Action<int> onGetDamage;
    void GetDamage(int damage);
}



