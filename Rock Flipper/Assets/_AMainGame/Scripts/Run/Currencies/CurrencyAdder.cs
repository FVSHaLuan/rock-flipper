using UnityEngine;

namespace Agame.Run
{
    public class CurrencyAdder : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private Currency currency;

        public void AddCurrency(int amount)
        {
            RunData.AddCurrency(currency, amount);
        }
    }

}