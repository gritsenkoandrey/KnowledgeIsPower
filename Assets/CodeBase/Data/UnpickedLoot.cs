using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class UnpickedLoot
    {
        public List<Loot> Loot = new List<Loot>();
    }
}