using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData
    {
        public Action Changed;
        
        public int Collected;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }
    }
}